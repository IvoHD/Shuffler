using System.Windows.Controls;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using WMPLib;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.WindowsAPICodePack.Shell.Interop;
using System.Windows;
using System.Linq;

namespace Shuffler
{
	class ShufflerUI 
	{
		public static WindowsMediaPlayer Player { get; set; } = new();
		public static FileManager FileManager { get; set; } = new();
		public static UIManager UIManager { get; set; } = new();
		public static PlayerControls PlayerControls { get; set; } = new();
	}
}