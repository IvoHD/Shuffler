using System;
using WMPLib;

namespace Shuffler
{
	class PlayerControls
	{
		public delegate void startedPlaying();
		public event startedPlaying StartedPlaying;

		FileManager FileManager { get; set; }
		public WindowsMediaPlayer Player { get; private set; }

		public PlayerControls(FileManager fileManager)
		{
			FileManager = fileManager;
			Player = new();
			FileManager.DirectorySelected += OnNewDirectorySelected;
		}

		int _volume = 50;
		public int Volume
		{
			get { return _volume; }
			set
			{
				_volume = value;
				Player.settings.volume = value;
			}
		}

		void OnNewDirectorySelected()
		{
			Player.controls.stop();
			Player = new();
		}

		void PlayRandomFile()
		{
			string Path = FileManager.GetRandomFile();

			Player.controls.stop();
			Player = new();
			Player.PlayStateChange += PlayerOnPlayStateChangeAutoplay;
			Player.URL = Path;
			Player.settings.volume = Volume;
			Player.controls.play();

			StartedPlaying.Invoke();
		}

		void PlayerOnPlayStateChangeAutoplay(int _)
		{
			if (Player.playState == WMPPlayState.wmppsMediaEnded)
				PlayRandomFile();
		}

		public void Play()
		{
			if (Player.playState == WMPPlayState.wmppsPaused)
				Player.controls.play();
			else
				PlayRandomFile();
		}

		public void Pause()
		{
			Player.controls.pause();
		}
	}
}
