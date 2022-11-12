using System.Windows.Controls;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using WMPLib;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.WindowsAPICodePack.Shell.Interop;
using System.Windows;
using System.Linq;

namespace Shuffler
{
	class ShufflerUI : INotifyPropertyChanged
	{
		public delegate void invalidPath();
		public event invalidPath InvalidPath;		
		public delegate void missingFile();
		public event missingFile MissingFile;

		WindowsMediaPlayer Player { get; set; } = new();
		Random Random { get; set; } = new();
		readonly string[] allowedExtensions = new[] { ".asf", ".wma", ".wmv", ".wm", ".asx", ".wax", ".wvx", ".wmx", ".wpl", ".dvr-ms", ".wmd", ".avi", ".mpg", ".mpeg", ".m1v", ".mp2", ".mp3", ".mpa", ".mpe", ".m3u", ".mid", ".midi", ".rmi", ".aif", ".aifc", ".aiff", ".au", ".snd", ".wav", ".cda", ".ivf", ".wmz", ".wms", ".mov", ".m4a", ".mp4", ".m4v", ".mp4v", ".3g2", ".3gp2", ".3gp", ".3gpp", ".aac", ".adt", ".adts", ".m2ts", ".flac"};

		string _folderPath = "Double click to pick folder...";
		public string FolderPath 
		{ 
			get { return _folderPath; }
			set 
			{
				_folderPath = value;
				GetFiles();
				OnPropertyChanged("FolderPath");
			}
		}

		string[] FilePaths { get; set; } = { };

		string _buttonSymbol = ButtonState.play;
		public string ButtonSymbol {
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

		int _volume = 50;
		public int Volume
		{
			get { return _volume; }
			set { 
				_volume = value;
				Player.settings.volume = value;
				OnPropertyChanged("Volume");
			}
		}


		public event PropertyChangedEventHandler? PropertyChanged;
		void OnPropertyChanged(String info)
		{
			if (PropertyChanged is not null)
				PropertyChanged(this, new PropertyChangedEventArgs(info));
		}

		void GetFiles()
		{
			FilePaths = Directory
			.GetFiles(FolderPath)
			.Where(file => allowedExtensions.Any(file.ToLower().EndsWith))
			.ToArray();

			if (FilePaths.Length == 0)
			{
				InvalidPath.Invoke();
				return;
			}
			Player.controls.stop();
			ButtonIsEnabled = true;
			ButtonSymbol = ButtonState.play;
			ButtonStateIsPlay = false;
		}

		void PlayRandomFile()
		{
			string Path = FilePaths[Random.Next(FilePaths.Length)];
			if (!File.Exists(FilePaths[Random.Next(FilePaths.Length)]))
			{
				MissingFile.Invoke();
				return;
			}

			Player = new();
			Player.PlayStateChange += Player_PlayStateChange;
			Player.URL = Path;
			Player.controls.play();
			ButtonSymbol = ButtonState.pause;
		}

		void Player_PlayStateChange(int _)
		{
			if(Player.playState == WMPPlayState.wmppsMediaEnded)
				PlayRandomFile();
		}

		public void Play()
		{
			if (Player.playState == WMPPlayState.wmppsPaused)
			{
				Player.controls.play();
				ButtonSymbol = ButtonState.pause;
			}
			else
				PlayRandomFile();
		}

		public void Pause()
		{
			Player.controls.pause();
			ButtonSymbol = ButtonState.play;
		}
	}

	static class ButtonState
	{
		public static readonly string play = "▶";
		public static readonly string pause = "⏸";
	}
}