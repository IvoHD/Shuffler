using Microsoft.VisualBasic;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Shuffler
{
	/// <summary>
	/// Interaction logic for ShufflerWindow.xaml
	/// </summary>
	public partial class ShufflerWindow : Window
	{
		ShufflerUI ShufflerUI { get; set; }
		bool DragStarted { get; set; }

		DispatcherTimer UpdateCurrPositionTimer = new();

		public ShufflerWindow()
		{
			InitializeComponent();
			DataContext = ShufflerUI = new ShufflerUI();
		
			//UpdateCurrPositionTimer updates CurrPosition every 100 milliseconds
			UpdateCurrPositionTimer.Interval = new(0, 0, 0, 0, 100);
			UpdateCurrPositionTimer.Tick += UpdatePlayerPositions;
			UpdateCurrPositionTimer.Start();
		}


		void PickFolder_DoubleClick(object sender, RoutedEventArgs e)
		{
			ShufflerUI.PickFolder();
		}

		void Play_Click(object sender, RoutedEventArgs e)
		{
			ShufflerUI.Play();
		}

		void Pause_Click(object sender, RoutedEventArgs e)
		{
			ShufflerUI.Pause();
		}

		void UpdatePlayerPositions(object sender, EventArgs e)
		{
			if (!DragStarted)
				ShufflerUI.UpdateSlider();
			else
				ShufflerUI.UpdateSliderExcludingCurrPositionPercent();
		}

		void PlaybackSlider_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
		{
			DragStarted = true;
		}

		void PlaybackSlider_DragComplete(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
		{
			DragStarted = false;
			Slider Slider = sender as Slider;
			ShufflerUI.CurrPositionPercent = Slider.Value;
		}
	}
}
