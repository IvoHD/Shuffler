using System.Windows.Controls;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using WMPLib;
using System.Diagnostics;

namespace Shuffler
{
	class ShufflerUI : INotifyPropertyChanged
	{
		WindowsMediaPlayer player { get; set; } = new();
		Random random { get; set; } = new();

		string _FolderPath = "Double click to pick folder...";
		public string FolderPath 
		{ 
			get { return _FolderPath; }
			set 
			{
				_FolderPath = value;
				GetFiles();
				OnPropertyChanged("FolderPath");
			}
		}


		string[] FilePaths { get; set; } 

		void GetFiles()
		{
			FilePaths = Directory.GetFiles(FolderPath);
		}

		public event PropertyChangedEventHandler? PropertyChanged;
		void OnPropertyChanged(String info)
		{
			if (PropertyChanged is not null)
				PropertyChanged(this, new PropertyChangedEventArgs(info));
		}

	}
}