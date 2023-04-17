using Microsoft.VisualBasic;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

		public ShufflerWindow()
		{
			InitializeComponent();
			DataContext = ShufflerUI = new ShufflerUI();
		}


		void PickDirectory_DoubleClick(object sender, RoutedEventArgs e)
		{
			ShufflerUI.PickDirectory();
		}

		void Play_Click(object sender, RoutedEventArgs e)
		{
			ShufflerUI.Play();
		}

		void Pause_Click(object sender, RoutedEventArgs e)
		{
			ShufflerUI.Pause();
		}

		void PlaybackSlider_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
		{
			ShufflerUI.DragStarted = true;
		}

		void PlaybackSlider_DragComplete(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
		{
			ShufflerUI.DragStarted = false;
			ShufflerUI.CurrPositionPercent = (sender as Slider).Value;
		}

		void Skip_Click(object sender, EventArgs e)
		{
			ShufflerUI.Play();
		}

		void OnExit(object sender, CancelEventArgs e)
		{
			ShufflerUI.Exit();
		}
	}
}
