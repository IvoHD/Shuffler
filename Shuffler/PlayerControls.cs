using WMPLib;

namespace Shuffler
{
	class PlayerControls
	{
		public delegate void startedPlaying();
		public event startedPlaying StartedPlaying;

		FileManager FileManager { get; set; }

		public PlayerControls(FileManager fileManager)
		{
			FileManager = fileManager;
		}

		int _volume = 50;
		public int Volume
		{
			get { return _volume; }
			set
			{
				_volume = value;
				ShufflerUI.Player.settings.volume = value;
			}
		}

		void PlayRandomFile()
		{
			string Path = FileManager.GetRandomFile();

			ShufflerUI.Player = new();
			ShufflerUI.Player.PlayStateChange += PlayerOnPlayStateChangeAutoplay;
			ShufflerUI.Player.URL = Path;
			ShufflerUI.Player.settings.volume = Volume;
			ShufflerUI.Player.controls.play();

			StartedPlaying.Invoke();
		}

		void PlayerOnPlayStateChangeAutoplay(int _)
		{
			if (ShufflerUI.Player.playState == WMPPlayState.wmppsMediaEnded)
				PlayRandomFile();
		}

		public void Play()
		{
			if (ShufflerUI.Player.playState == WMPPlayState.wmppsPaused)
				ShufflerUI.Player.controls.play();
			else
				PlayRandomFile();
		}

		public void Pause()
		{
			ShufflerUI.Player.controls.pause();
		}
	}
}
