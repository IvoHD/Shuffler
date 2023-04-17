using WMPLib;
using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.ComponentModel;
using System;
using System.IO;
using System.Windows.Threading;

namespace Shuffler
{
	class ShufflerUI : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler? PropertyChanged;

		FileManager FileManager { get; set; }
		PlayerControls PlayerControls { get; set; }
		DiscordManager DiscordManager { get; set; }
		const string PlayIcon = "▶";
		const string PauseIcon = "⏸";

		DispatcherTimer UpdateTimer { get; set; } = new();
		public ShufflerUI()
		{
			WindowsMediaPlayer Player = new();
			FileManager = new();
			PlayerControls = new(FileManager);
			DiscordManager = new(this);

			FileManager.DirectorySelected += () => OnPropertyChanged("FileName");
			FileManager.InvalidPath += InvalidPath;
			FileManager.MissingFile += MissingFile;
			PlayerControls.StartedPlaying += OnPlay;

			//UpdateCurrPositionTimer updates CurrPosition every 100 milliseconds
			UpdateTimer.Interval = new(0, 0, 0, 0, 100);
			UpdateTimer.Tick += UpdatePlayerPositions;
			UpdateTimer.Start();

		}

		void UpdatePlayerPositions(object sender, EventArgs e)
		{
			if (!DragStarted)
				UpdateSlider();
			else
				UpdateSliderExcludingCurrPositionPercent();
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
				if (PlayerControls.Player?.controls?.currentPosition is null || PlayerControls.Player?.currentMedia?.duration is null)
					return 0;
				return PlayerControls.Player.controls.currentPosition / PlayerControls.Player.currentMedia.duration * 100;
			}
			set
			{
				PlayerControls.Player.controls.currentPosition = PlayerControls.Player.currentMedia.duration * value / 100;
			}
		}

		public bool DragStarted { get; set; }

		public string CurrPositionString
		{
			get { return PlayerControls.Player?.controls?.currentPositionString is "" ? "00:00" : PlayerControls.Player.controls.currentPositionString; }
		}

		public string MaxPositionString
		{
			get { return PlayerControls.Player?.currentMedia?.durationString ?? "00:00"; }
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
			get { return Path.GetFileNameWithoutExtension(PlayerControls.Player.URL); }
		}

		protected void OnPropertyChanged(String info)
		{
			if (PropertyChanged is not null)
				PropertyChanged(this, new PropertyChangedEventArgs(info));
		}

		public void PickDirectory()
		{
			CommonOpenFileDialog dlg = new();
			dlg.IsFolderPicker = true;

			if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
			{
				if (dlg.FileName != string.Empty)
				{
					if (FileManager.DirectoryPath == dlg.FileName)
						return;
					DirectoryPath = dlg.FileName;
				}
			}
			else
				return;
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

		void UpdateSlider()
		{
			if (PlayerControls.Player.playState == WMPPlayState.wmppsPlaying)
				UpdateProperties();
		}

		void UpdateSliderExcludingCurrPositionPercent()
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

		void OnPlay()
		{
			PlayBackSliderIsEnabled = true;
			OnPropertyChanged("FileName");
		}

		void SetButtonPlay()
		{
			ButtonSymbol = PlayIcon;
			ButtonStateIsPlay = false;
		}

		void SetButtonPause()
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

		public void Exit()
		{
			PlayerControls.Stop();
			DiscordManager.Dispose();
		}
	}
}