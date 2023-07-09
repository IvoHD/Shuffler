using System;
using System.Threading;
using WMPLib;

namespace Shuffler
{
	class PlayerControls
	{
		public delegate void startedPlaying();
		public event startedPlaying StartedPlaying;

		FileManager FileManager { get; set; }

		WindowsMediaPlayer _player = new();
		public WindowsMediaPlayer Player 
		{
			get { return _player; }
			private set 
			{
				_player.controls.stop();
				_player = value;
				_player.settings.volume = Volume;
				_player.PlayStateChange += PlayerOnPlayStateChangeAutoplay;
			}
		}

		public PlayerControls(FileManager fileManager)
		{
			FileManager = fileManager;
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
		}

		void PlayRandomFile()
		{
			PlayFile(FileManager.GetRandomFile());
		}

		public void PlayFileByIndex(int index)
		{
			PlayFile(FileManager.GetFileByIndex(index));
		}

		void PlayFile(string path)
		{
			Player = new();
			Player.URL = path;
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

		public void Stop()
		{
			Player.controls.stop();
		}
	}
}
