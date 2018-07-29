using System;

namespace GameMutexKiller.WindowsTrayLibrary
{
	public interface IWindowsTray
	{
		void RunConsoleInBackground(Action appFn);
	}
}