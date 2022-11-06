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

namespace Shuffler
{
	class ShufflerUI : INotifyPropertyChanged
	{
		public delegate void invalidPath();
		public event invalidPath InvalidPath;		
		public delegate void missingFile();
		public event missingFile MissingFile;

		WindowsMediaPlayer Player { get; set; }
		Random Random { get; set; } = new();

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

		public event PropertyChangedEventHandler? PropertyChanged;
		void OnPropertyChanged(String info)
		{
			if (PropertyChanged is not null)
				PropertyChanged(this, new PropertyChangedEventArgs(info));
		}

		void GetFiles()
		{
			FilePaths = Directory.GetFiles(FolderPath);
		}

		public void PlayRandomFile()
		{
			if (FilePaths.Length == 0)
			{
				InvalidPath.Invoke();
				return;
			}

			string Path = FilePaths[Random.Next(FilePaths.Length)];
			if (!File.Exists(FilePaths[Random.Next(FilePaths.Length)]))
			{
				MissingFile.Invoke();
				return;
			}

			Player = new();
			Player.URL = Path;
			Player.PlayStateChange += player_PlayStateChange;
			Player.controls.play();
		}

		private void player_PlayStateChange(int _)
		{
			if(Player.playState == WMPLib.WMPPlayState.wmppsMediaEnded)
				PlayRandomFile();
		}
	}
}