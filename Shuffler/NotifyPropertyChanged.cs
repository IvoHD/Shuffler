using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shuffler
{
	class NotifyPropertyChanged : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler? PropertyChanged;
		protected void OnPropertyChanged(String info)
		{
			if (PropertyChanged is not null)
				PropertyChanged(this, new PropertyChangedEventArgs(info));
		}
	}
}
