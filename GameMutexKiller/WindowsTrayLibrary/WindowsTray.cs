using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameMutexKiller.WindowsTrayLibrary
{
	public class WindowsTray : IWindowsTray
	{
		private readonly ILogging _logger;
		private readonly Action _closeApplicationFn;
		private readonly NotifyIcon _tray;
		private readonly WindowsTrayRef _windowsTrayRef;
		private const int AppMinimiseTimeInSeconds = 5;

		private IntPtr ConsoleWindow =>  _windowsTrayRef.GetConsoleWindowRef();
		private IntPtr SystemMenu => _windowsTrayRef.GetSystemMenuRef(ConsoleWindow, false);

		public WindowsTray(string appName, ILogging logger, Action closeApplicationFn)
		{
			_logger = logger;
			_closeApplicationFn = closeApplicationFn;
			_windowsTrayRef = new WindowsTrayRef();

			var mExit = new MenuItem("Exit", OnExit);
			var menu = new ContextMenu(new[] { mExit });

			var icon = Properties.Resources.icon64;
			_tray = new NotifyIcon()
			{
				Icon = icon,
				Visible = true,
				Text = appName,
				ContextMenu = menu
			};
			_tray.DoubleClick += RestoreWindow;
			_tray.Click += RestoreWindow;
		}

		private void DisableCloseButton()
		{
			_windowsTrayRef.EnableMenuItemRef(SystemMenu, SystemMenuStates.SC_CLOSE, SystemMenuStates.MF_ENABLED | SystemMenuStates.MF_DISABLED);
		}

		private void RestoreWindow(object sender = null, EventArgs e = null)
		{
			_windowsTrayRef.ShowWindowRef(ConsoleWindow, (int)ShowWindowCommands.Restore);
		}

		private void ConsolePlacementRef(ref WindowPlacement wPlacement)
		{
			_windowsTrayRef.GetWindowPlacementRef(ConsoleWindow, ref wPlacement);
		}

		private void HideConsole()
		{
			_windowsTrayRef.ShowWindowRef(ConsoleWindow, (int)ShowWindowCommands.Hide);
		}

		private void SetupMinimize()
		{
			Task.Factory.StartNew(() =>
			{
				while (true)
				{
					var wPlacement = new WindowPlacement();
					ConsolePlacementRef(ref wPlacement);
					if (wPlacement.showCmd == (int)ShowWindowCommands.ShowMinimized)
						HideConsole();
					Wait(1);
				}
			}, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
		}

		private void Wait(int timeout)
		{
			using (var autoResetEvent = new AutoResetEvent(false))
				autoResetEvent.WaitOne(timeout, true);
		}

		private void OnExit(object sender, EventArgs e)
		{
			_tray.Dispose();
			Application.Exit();

			_logger.Info("Closing application...");
			ShowApplication();
			Task.Run(() =>
			{
				Thread.Sleep(1500);
				_closeApplicationFn();
			});
		}

		public void RunConsoleInBackground(Action appFn)
		{
			DisableCloseButton();
			SetupMinimize();

			_logger.Info($"Application will minimise to tray in {AppMinimiseTimeInSeconds} seconds...");
			_logger.Info("To exit application please right click the tray icon and exit...");

			Task.Run(() =>
			{
				appFn();
				Thread.Sleep(TimeSpan.FromSeconds(AppMinimiseTimeInSeconds));
				HideConsole();
			});

			Application.Run();
		}

		public void ShowApplication()
		{
			RestoreWindow();
		}
	}
}