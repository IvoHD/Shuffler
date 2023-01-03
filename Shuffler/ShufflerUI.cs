using WMPLib;
using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.ComponentModel;
using System;

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
			PlayerControls.StartedPlaying += () => PlayBackSliderIsEnabled = true;
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
				if ((double?)Player?.controls?.currentPosition is null || (double?)Player?.currentMedia?.duration is null)
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
			get
			{
				if (Player?.controls?.currentPositionString is "")
					return "00:00";
				return Player.controls.currentPositionString;
			}
		}

		public string MaxPositionString
		{
			get
			{
				if (Player?.currentMedia?.durationString is null)
					return "00:00";
				return Player.currentMedia.durationString;
			}
		}

		public string FolderPath
		{
			get { return FileManager.FolderPath; }
			set
			{
				FileManager.FolderPath = value;
			}
		}

		public void PickFolder()
		{
			CommonOpenFileDialog dlg = new();
			dlg.IsFolderPicker = true;

			if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
				if (dlg.FileName != string.Empty)
					FolderPath = dlg.FileName;

			FolderSelected();
		}

		void InvalidPath()
		{
			MessageBox.Show("Folder path is not valid, select a folder with files.", "Invalid Path", MessageBoxButton.OK, MessageBoxImage.Error);
			PickFolder();
		}

		void MissingFile()
		{
			MessageBox.Show("File was deleted or moved. Pick another folder", "Invalid Path", MessageBoxButton.OK, MessageBoxImage.Error);
			PickFolder();
		}

		void FolderSelected()
		{
			ButtonIsEnabled = true;
			SetButtonPlay();
			UpdateProperties();
			OnPropertyChanged("FolderPath");
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
	}
}