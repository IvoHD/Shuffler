using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Shuffler
{
	/// <summary>
	/// Interaction logic for ShufflerWindow.xaml
	/// </summary>
	public partial class ShufflerWindow : Window
	{
		ShufflerUI ShufflerUI { get; set; }
		bool DragStarted { get; set; }
		bool HamburgerMenuOpened { get; set; }

		const double OpenCloseaAnimationInterval = 0.2;

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

		void HamburgerButton_Click(object sender, RoutedEventArgs e)
		{
			HamburgerMenuOpened = !HamburgerMenuOpened;

			HamburgerMenu.BeginAnimation(WidthProperty, new DoubleAnimation(HamburgerMenuOpened ? 0 : 120, new(TimeSpan.FromSeconds(OpenCloseaAnimationInterval))));
		}

		void ListView_Click(object sender, RoutedEventArgs e)
		{
			ShufflerUI.PlayFile((sender as ListView).SelectedIndex);
		}
	}
}
