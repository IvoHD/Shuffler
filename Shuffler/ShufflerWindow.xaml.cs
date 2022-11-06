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

namespace Shuffler
{
	/// <summary>
	/// Interaction logic for ShufflerWindow.xaml
	/// </summary>
	public partial class ShufflerWindow : Window
	{
		ShufflerUI ShufflerUI { get; set; }
		public ShufflerWindow()
		{
			InitializeComponent();
			DataContext = ShufflerUI = new ShufflerUI();
			ShufflerUI.InvalidPath += InvalidPath;
			ShufflerUI.MissingFile += MissingFile;
		}

		void InvalidPath()
		{
			MessageBox.Show("Folder path is not valid, select a folder with files.", "Invalid Path", MessageBoxButton.OK, MessageBoxImage.Error);
			PickFolder();
		}

		void MissingFile()
		{
			MessageBox.Show("File was deleted or moved. Pick another folder", "Invalid Path", MessageBoxButton.OK, MessageBoxImage.Error);
			PickFolder();
		}

		void PickFolder()
		{
			CommonOpenFileDialog dlg = new();
			dlg.IsFolderPicker = true;

			if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
				if (dlg.FileName != string.Empty)
					ShufflerUI.FolderPath = dlg.FileName;
		}

		void PickFolder_DoubleClick(object sender, RoutedEventArgs e)
		{
			PickFolder();
		}

		void Play_Click(object sender, RoutedEventArgs e)
		{
			ShufflerUI.PlayRandomFile();
		}
	}
}
