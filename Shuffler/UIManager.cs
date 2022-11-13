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
		const string Play = "▶";
		const string Pause = "⏸";

		public UIManager()
		{
			ShufflerUI.FileManager.NewFolderSelected += FileSelected;
		}

		void FileSelected()
		{
			ButtonIsEnabled = true;
			SetButtonPlay();
		}

		string _buttonSymbol = Play;
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

		bool _playBackSliderIsEnabled = false;
		public bool PlayBackSliderIsEnabled
		{
			get { return _playBackSliderIsEnabled; }
			set 
			{
				_playBackSliderIsEnabled = value;
				OnPropertyChanged("PlayBackSliderIsEnabled");
			}
		}

		public void SetButtonPlay()
		{
			ShufflerUI.UIManager.ButtonSymbol = Play;
			ShufflerUI.UIManager.ButtonStateIsPlay = false;
		}

		public void SetButtonPause()
		{
			ShufflerUI.UIManager.ButtonSymbol = Pause;
			ShufflerUI.UIManager.ButtonStateIsPlay = true;
		}
	}
}
