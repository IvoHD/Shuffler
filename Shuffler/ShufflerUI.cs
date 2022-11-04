using System.Windows.Controls;
using System;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Shuffler
{
	class ShufflerUI : INotifyPropertyChanged
	{
		string _FolderPath = "Double click to pick folder...";
		public string FolderPath 
			{ get { return _FolderPath; }
			set 
			{
				_FolderPath = value;
				OnPropertyChanged("FolderPath");
			}
		}

		public event PropertyChangedEventHandler? PropertyChanged;
		void OnPropertyChanged(String info)
		{
			if (PropertyChanged is not null)
				PropertyChanged(this, new PropertyChangedEventArgs(info));
		}

	}
}