using Discord;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using WMPLib;
using Activity = Discord.Activity;

namespace Shuffler
{
	class DiscordManager
	{
		Discord.Discord RPC { get; set; }
		ActivityManager ActivityManager { get; set; }
		Activity Activity { get; set; }
		ShufflerUI ShufflerUI { get; set; }

		Thread DiscordUpdateThread { get; set; }
		bool Disposed { get; set; } 

		public DiscordManager(ShufflerUI shufflerUI)
		{
			try
			{
				RPC = new(-1, (UInt64)Discord.CreateFlags.NoRequireDiscord);
			}
			catch (Exception e)
			{
				RPC = null;
			}

			if(RPC is not null)
			{
				ShufflerUI = shufflerUI;
				ActivityManager = RPC.GetActivityManager();
				DiscordUpdateThread = new(Update);
				DiscordUpdateThread.Start();
			}
		}

		public void UpdateActivity()
		{
			try
			{
				if(PlayerControls.Player.playState == WMPPlayState.wmppsPlaying)
					Activity = new()
					{
						State = $"Listening to {ShufflerUI.FileName}",
						Details = $"{PlayerControls.Player.controls.currentPositionString} - {PlayerControls.Player.currentMedia.durationString}"
					};
				else if(PlayerControls.Player.playState == WMPPlayState.wmppsPaused)
					Activity = new()
					{
						State = $"Currently paused {ShufflerUI.FileName}",
						Details = $"{PlayerControls.Player.controls.currentPositionString} - {PlayerControls.Player.currentMedia.durationString}"
					};
				else
					Activity = new()
					{
						State = $"Idling",
					};

				ActivityManager.UpdateActivity(Activity, _ => { });
				RPC.RunCallbacks();

				Trace.WriteLine("Activity set", Activity.State);
			}
			catch (System.Runtime.InteropServices.COMException e)
			{  }
		}

		public void Update()
		{
			while (!Disposed)
			{
				UpdateActivity();
				if (RPC != null)
					RPC.RunCallbacks();
			}
		}

		public void Dispose()
		{
			if(RPC is not null)
			{
				Disposed = true;
				DiscordUpdateThread.Join();
				RPC.Dispose();
			}
		}
	}
}
