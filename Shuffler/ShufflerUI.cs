using WMPLib;
using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.ComponentModel;
using System;
using System.IO;

namespace Shuffler
{
	class ShufflerUI : INotifyPropertyChanged
	{

		public event PropertyChangedEventHandler? PropertyChanged;
		protected void OnPropertyChanged(String info)
		{
			if (PropertyChanged is not null)
				PropertyChanged(this, new PropertyChangedEventArgs(info));
		}

		public static WindowsMediaPlayer Player = new();

		FileManager FileManager { get; set; }
		PlayerControls PlayerControls { get; set; }

		const string PlayIcon = "▶";
		const string PauseIcon = "⏸";

		public ShufflerUI()
		{
			FileManager = new();
			PlayerControls = new(FileManager);

			FileManager.InvalidPath += InvalidPath;
			FileManager.MissingFile += MissingFile;
			PlayerControls.StartedPlaying += OnPlay;
		}

		string _buttonSymbol = PlayIcon;
		public string ButtonSymbol
		{
			get { return _buttonSymbol; }
			set
			{
				_buttonSymbol = value;
				OnPropertyChanged("ButtonSymbol");
			}
		}

		bool _buttonIsEnabled = false;
		public bool ButtonIsEnabled
		{
			get { return _buttonIsEnabled; }
			set
			{
				_buttonIsEnabled = value;
				OnPropertyChanged("ButtonIsEnabled");
			}
		}

		bool _buttonStateIsPlay;
		public bool ButtonStateIsPlay
		{
			get { return _buttonStateIsPlay; }
			set
			{
				_buttonStateIsPlay = value;
				OnPropertyChanged("ButtonStateIsPlay");
			}
		}

		bool _playBackSliderIsEnabled = false;
		public bool PlayBackSliderIsEnabled
		{
			get { return _playBackSliderIsEnabled; }
			set 
			{
				_playBackSliderIsEnabled = value;
				OnPropertyChanged("PlayBackSliderIsEnabled");
			}
		}

		public int Volume
		{
			get { return PlayerControls.Volume; }
			set
			{
				PlayerControls.Volume = value;
				OnPropertyChanged("Volume");
			}
		}

		public double CurrPositionPercent
		{
			get
			{
				if (Player?.controls?.currentPosition is null || Player?.currentMedia?.duration is null)
					return 0;
				return Player.controls.currentPosition / Player.currentMedia.duration * 100;
			}
			set
			{
				Player.controls.currentPosition = Player.currentMedia.duration * value / 100;
			}
		}

		public string CurrPositionString
		{
			get { return Player?.controls?.currentPositionString is "" ? "00:00" : Player.controls.currentPositionString; }
		}

		public string MaxPositionString
		{
			get { return Player?.currentMedia?.durationString ?? "00:00"; }
		}

		public string DirectoryPath
		{
			get { return FileManager.DirectoryPath; }
			set
			{
				FileManager.DirectoryPath = value;
			}
		}

		public string FileName
		{
			get { return Player is null ? "select directory" : Path.GetFileNameWithoutExtension(Player.URL); }
		}

		public void PickDirectory()
		{
			CommonOpenFileDialog dlg = new();
			dlg.IsFolderPicker = true;

			if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
				if (dlg.FileName != string.Empty)
					DirectoryPath = dlg.FileName;

			DirectorySelected();
		}

		void InvalidPath()
		{
			MessageBox.Show("Directory path is not valid, select a directory with files.", "Invalid Path", MessageBoxButton.OK, MessageBoxImage.Error);
			PickDirectory();
		}

		void MissingFile()
		{
			MessageBox.Show("File was deleted or moved. Pick another directory", "Invalid Path", MessageBoxButton.OK, MessageBoxImage.Error);
			PickDirectory();
		}

		void DirectorySelected()
		{
			ButtonIsEnabled = true;
			SetButtonPlay();
			UpdateProperties();
			OnPropertyChanged("DirectoryPath");
		}

		public void UpdateSlider()
		{
			if (Player.playState == WMPPlayState.wmppsPlaying)
				UpdateProperties();
		}

		public void UpdateSliderExcludingCurrPositionPercent()
		{
			OnPropertyChanged("CurrPositionString");
			OnPropertyChanged("MaxPositionString");
		}

		void UpdateProperties()
		{
			OnPropertyChanged("CurrPositionString");
			OnPropertyChanged("CurrPositionPercent");
			OnPropertyChanged("MaxPositionString");
		}

		public void SetButtonPlay()
		{
			ButtonSymbol = PlayIcon;
			ButtonStateIsPlay = false;
		}

		public void SetButtonPause()
		{
			ButtonSymbol = PauseIcon;
			ButtonStateIsPlay = true;

		}

		public void Play()
		{
			PlayerControls.Play();
			SetButtonPause();
		}

		public void Pause()
		{
			PlayerControls.Pause();
			SetButtonPlay();
		}

		void OnPlay()
		{
			PlayBackSliderIsEnabled = true;
			OnPropertyChanged("FileName");
		}
	}
}