using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static Shuffler.ShufflerUI;

namespace Shuffler
{
	class FileManager : NotifyPropertyChanged
	{
		public delegate void invalidPath();
		public event invalidPath InvalidPath;
		public delegate void missingFile();
		public event missingFile MissingFile;

		Random Random { get; set; } = new();

		readonly string[] allowedExtensions = new[] { ".asf", ".wma", ".wmv", ".wm", ".asx", ".wax", ".wvx", ".wmx", ".wpl", ".dvr-ms", ".wmd", ".avi", ".mpg", ".mpeg", ".m1v", ".mp2", ".mp3", ".mpa", ".mpe", ".m3u", ".mid", ".midi", ".rmi", ".aif", ".aifc", ".aiff", ".au", ".snd", ".wav", ".cda", ".ivf", ".wmz", ".wms", ".mov", ".m4a", ".mp4", ".m4v", ".mp4v", ".3g2", ".3gp2", ".3gp", ".3gpp", ".aac", ".adt", ".adts", ".m2ts", ".flac" };
		string[] FilePaths { get; set; } = { };

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
			ShufflerUI.UIManager.EnablePlayButton();
			ShufflerUI.UIManager.SetButtonPlay();
		}

		public string GetRandomFile()
		{
			string Path = FilePaths[Random.Next(FilePaths.Length)];
			if (!File.Exists(FilePaths[Random.Next(FilePaths.Length)]))
			{
				MissingFile.Invoke();
				return "";
			}

			return Path;
		}
	}
}
