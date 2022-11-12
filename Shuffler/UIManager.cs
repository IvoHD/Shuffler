using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shuffler
{
	class UIManager : NotifyPropertyChanged
	{
		const string play = "▶";
		const string pause = "⏸";

		string _buttonSymbol = play;
		public string ButtonSymbol
		{
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

		public void EnablePlayButton()
		{
			ButtonIsEnabled = true;
		}

		public void SetButtonPlay()
		{
			ShufflerUI.UIManager.ButtonSymbol = play;
			ShufflerUI.UIManager.ButtonStateIsPlay = false;
		}

		public void SetButtonPause()
		{
			ShufflerUI.UIManager.ButtonSymbol = pause;
			ShufflerUI.UIManager.ButtonStateIsPlay = true;
		}
	}
}
