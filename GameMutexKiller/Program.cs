using System;
using System.Windows.Forms;
using GameMutexKiller.WindowsTrayLibrary;

namespace GameMutexKiller
{
	public class Program
	{
		private static MutexKiller _mutexKiller;
		private static readonly ILogging Logger = Logging.Instance;

		public static void Main(string[] args)
		{
			AppDomain.CurrentDomain.UnhandledException += (o, e) => HandleException(e);
			Application.ThreadException += (o, e) => HandleException(e);

			Logger.Info("Game Mutex Lock Killer Activated.");

			var windowsTray = new WindowsTray("Game Mutex Killer", Logger, CloseApplication);
			windowsTray.RunConsoleInBackground();

			_mutexKiller = new MutexKiller();
			_mutexKiller.StartKilling();

			Console.ReadLine();
			CloseApplication();
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