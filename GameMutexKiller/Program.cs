using System;
using System.Windows.Forms;
using GameMutexKiller.WindowsTrayLibrary;

namespace GameMutexKiller
{
	public class Program
	{
		private const string AppNAme = "Game Mutex Lock Killer";
		private static MutexKiller _mutexKiller;
		private static readonly ILogging Logger = Logging.Instance;
		private static WindowsTray _windowsTray;

		public static void Main(string[] args)
		{
			SetupApplicationExceptionHandling();

			Logger.Info($"Activating {AppNAme} ...");
			_windowsTray = new WindowsTray("Game Mutex Killer", Logger, CloseApplication);
			_windowsTray.RunConsoleInBackground(() =>
			{
				Logger.Info($"{AppNAme} activated");
				_mutexKiller = new MutexKiller(Logger);
				_mutexKiller.StartKilling();
			});

			

			Console.ReadLine();
			CloseApplication();
		}

		private static void SetupApplicationExceptionHandling()
		{
			AppDomain.CurrentDomain.UnhandledException += (o, e) => HandleException(e);
			Application.ThreadException += (o, e) => HandleException(e);
		}

		private static void HandleException(EventArgs unhandledExceptionEventArgs)
		{
			Logger.Info("Ups! Something went wrong. Please try again.");
			Logger.Info(unhandledExceptionEventArgs.ToString());
			Logger.Info("Press any key to close.");
			Console.ReadLine();
			CloseApplication();
		}

		private static void CloseApplication()
		{
			_mutexKiller.StopKilling();
			Environment.Exit(0);
		}
	}
}