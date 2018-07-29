using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GameMutexKiller
{
	public class MutexKiller
	{
		private readonly IHandleWrapper _handleWrapper;
		private readonly List<string> _mutexList;
		private readonly ILogging _logger;
		private const int KillTimeoutInMs = 10000;
		private bool _keepRunning;

		public MutexKiller(ILogging logger)
		{
			_logger = logger;
			_handleWrapper = new HandleWrapper();
			_mutexList = new List<string>()
			{
				"Life is Feudal",
				"lif client instance"
			};
		}
		public void KillMutex()
		{
			foreach (var mutex in _mutexList)
			{
				var lockKillResult = _handleWrapper.KillLock(mutex);
				if (lockKillResult.Any() && lockKillResult.All(l => l))
				{
					_logger.Info($"Killed lock for {mutex}");
				}
			}
		}

		public void StartKilling()
		{
			_keepRunning = true;
			Task.Factory.StartNew(LoopKillMutex);
		}

		private void LoopKillMutex()
		{
			KillMutex();
			Thread.Sleep(KillTimeoutInMs);
			if (_keepRunning)
			{
				LoopKillMutex();
			}
		}

		public void StopKilling()
		{
			_keepRunning = false;
		}
	}
}