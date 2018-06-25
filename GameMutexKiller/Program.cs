using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace GameMutexKiller
{
	public class Program
	{
		public static void Main(string[] args)
		{
			AppDomain.CurrentDomain.UnhandledException += (o, e) => HandleException(e);
			Application.ThreadException += (o, e) => HandleException(e);

			var result1 = HandleWrapper.KillLock("Life is Feudal").ToList();
			var result2 = HandleWrapper.KillLock("lif client instance").ToList();
			result1.ForEach(i => Console.Write("{0}\t", i));
			result2.ForEach(i => Console.Write("{0}\t", i));

			Console.WriteLine("Press any key to close.");
			Console.ReadLine();
		}

		private static void HandleException(EventArgs unhandledExceptionEventArgs)
		{
			Console.WriteLine("Ups! Something went wrong. Please try again.");
			Console.WriteLine(unhandledExceptionEventArgs.ToString());
			Console.WriteLine("Press any key to close.");
			Console.ReadLine();
			Environment.Exit(0);
		}
	}
}