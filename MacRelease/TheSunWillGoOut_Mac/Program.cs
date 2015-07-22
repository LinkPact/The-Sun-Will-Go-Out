using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;

namespace SpaceProject_Mac
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		public static void Main (string[] args)
		{		
			MonoMac.AppKit.NSApplication.Init ();

			using (var p = new MonoMac.Foundation.NSAutoreleasePool ()) {
				MonoMac.AppKit.NSApplication.SharedApplication.Delegate = new AppDelegate ();
				MonoMac.AppKit.NSApplication.Main (args);
			}
		}
	}

	class AppDelegate : MonoMac.AppKit.NSApplicationDelegate
	{
		Game1 game;
		public override void FinishedLaunching (MonoMac.Foundation.NSObject notification)
		{
			game = new Game1 ();
			game.Run ();
		}
			
		public override bool ApplicationShouldTerminateAfterLastWindowClosed (MonoMac.AppKit.NSApplication sender)
		{			
			if (Game1.GameRestarted) {
				StartNewProcess ();
			}
			return true;
		}

		private void StartNewProcess()
		{
			ProcessStartInfo startInfo = new ProcessStartInfo ();
			startInfo.UseShellExecute = false;
			startInfo.FileName = GetApplicationPath ();
			startInfo.RedirectStandardOutput = true;

			Process.Start (startInfo);
		}

		private string GetApplicationPath()
		{
			StringBuilder path = new StringBuilder(Path.GetDirectoryName (Assembly.GetEntryAssembly ().Location));
			path.Replace ("MonoBundle", "MacOS/");
			string name = "TheSunWillGoOut_Mac";
			path.Append (name);

			return path.ToString ();
		}
	}
}

