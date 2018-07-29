using Serilog;
using Serilog.Core;

namespace GameMutexKiller
{
	public class Logging : ILogging
	{
		private static Logging _instance;
		private readonly Logger _logger;

		public Logging()
		{
			_logger = new LoggerConfiguration()
				.WriteTo.Console()
				.CreateLogger();
		}

		public void Info(string message)
		{
			_logger.Information(message);
		}

		public static Logging Instance => _instance ?? (_instance = new Logging());
	}

	public interface ILogging
	{
		void Info(string message);
	}
}