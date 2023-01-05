using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Shuffler
{
	class FileManager
	{
		public delegate void invalidPath();
		public event invalidPath InvalidPath;
		public delegate void missingFile();
		public event missingFile MissingFile;
		public delegate void directorySelected();
		public event directorySelected DirectorySelected;

		Random Random { get; set; } = new();

		readonly string[] AllowedExtensions = new[] { ".asf", ".wma", ".wmv", ".wm", ".asx", ".wax", ".wvx", ".wmx", ".wpl", ".dvr-ms", ".wmd", ".avi", ".mpg", ".mpeg", ".m1v", ".mp2", ".mp3", ".mpa", ".mpe", ".m3u", ".mid", ".midi", ".rmi", ".aif", ".aifc", ".aiff", ".au", ".snd", ".wav", ".cda", ".ivf", ".wmz", ".wms", ".mov", ".m4a", ".mp4", ".m4v", ".mp4v", ".3g2", ".3gp2", ".3gp", ".3gpp", ".aac", ".adt", ".adts", ".m2ts", ".flac" };
		string[] FilePaths { get; set; } = { };

		string _directoryPath = "Double click to pick directory...";
		public string DirectoryPath
		{
			get { return _directoryPath; }
			set
			{
				_directoryPath = value;
				GetFiles();
			}
		}

		void GetFiles()
		{
			FilePaths = Directory
			.GetFiles(DirectoryPath)
			.Where(file => AllowedExtensions.Any(file.ToLower().EndsWith))
			.ToArray();

			if (FilePaths.Length == 0)
			{
				InvalidPath.Invoke();
				return;
			}

			DirectorySelected.Invoke();
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
