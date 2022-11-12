using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using WMPLib;

namespace Shuffler
{
	class PlayerControls : NotifyPropertyChanged
	{
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

		void PlayRandomFile()
		{
			string Path = ShufflerUI.FileManager.GetRandomFile();

			ShufflerUI.Player = new();
			ShufflerUI.Player.PlayStateChange += PlayerOnPlayStateChangeAutoplay;
			ShufflerUI.Player.URL = Path;
			ShufflerUI.Player.settings.volume = Volume;
			ShufflerUI.Player.controls.play();

			ShufflerUI.UIManager.SetButtonPause();
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

	}
}
