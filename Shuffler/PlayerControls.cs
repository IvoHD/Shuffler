using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using WMPLib;

namespace Shuffler
{
	class PlayerControls : NotifyPropertyChanged
	{
		public PlayerControls()
		{
			ShufflerUI.FileManager.NewFolderSelected += ResetPositions;
		}

		int _volume = 50;
		public int Volume
		{
			get { return _volume; }
			set
			{
				_volume = value;
				ShufflerUI.Player.settings.volume = value;
				OnPropertyChanged("Volume");
			}
		}

		public double CurrPositionPercent { 
			get
			{
				if ((double?)ShufflerUI.Player?.controls?.currentPosition is null || (double?)ShufflerUI.Player?.currentMedia?.duration is null)
					return 0;
				return ShufflerUI.Player.controls.currentPosition / ShufflerUI.Player.currentMedia.duration * 100;
			}
			set 
			{
				ShufflerUI.Player.controls.currentPosition = ShufflerUI.Player.currentMedia.duration * value / 100;
			}
		}

		public string CurrPositionString
		{
			get {
				if (ShufflerUI.Player?.controls?.currentPositionString is "")
					return "00:00";
				return ShufflerUI.Player.controls.currentPositionString; 
			}
		}

		public string MaxPositionString
		{
			get
			{
				if (ShufflerUI.Player?.currentMedia?.durationString is null)
					return "00:00";
				return ShufflerUI.Player.currentMedia.durationString;
			}
		}

		void PlayRandomFile()
		{
			string Path = ShufflerUI.FileManager.GetRandomFile();

			ShufflerUI.Player = new();
			ShufflerUI.Player.PlayStateChange += PlayerOnPlayStateChangeAutoplay;
			ShufflerUI.Player.URL = Path;
			ShufflerUI.Player.settings.volume = Volume;
			ShufflerUI.Player.controls.play();

			ShufflerUI.UIManager.SetButtonPause();
			ShufflerUI.UIManager.PlayBackSliderIsEnabled = true;
		}

		void PlayerOnPlayStateChangeAutoplay(int _)
		{
			if (ShufflerUI.Player.playState == WMPPlayState.wmppsMediaEnded)
				PlayRandomFile();
		}

		public void Play()
		{
			if (ShufflerUI.Player.playState == WMPPlayState.wmppsPaused)
			{
				ShufflerUI.Player.controls.play();
				ShufflerUI.UIManager.SetButtonPause();
			}
			else
				PlayRandomFile();
		}

		public void Pause()
		{
			ShufflerUI.Player.controls.pause();
			ShufflerUI.UIManager.SetButtonPlay();
		}

		public void UpdatePositions()
		{
			if(ShufflerUI.Player.playState == WMPPlayState.wmppsPlaying)
			{
				OnPropertyChanged("CurrPositionString");
				OnPropertyChanged("CurrPositionPercent");
				OnPropertyChanged("MaxPositionString");
			}
		}

		void ResetPositions()
		{
			ShufflerUI.Player = new();
			OnPropertyChanged("CurrPositionString");
			OnPropertyChanged("CurrPositionPercent");
			OnPropertyChanged("MaxPositionString");
		}

		internal void UpdatePositionsExcludingCurrPositionPercent()
		{
			OnPropertyChanged("CurrPositionString");
			OnPropertyChanged("MaxPositionString");
		}
	}
}
