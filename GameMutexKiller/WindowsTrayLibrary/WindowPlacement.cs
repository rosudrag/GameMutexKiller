using System.Drawing;

namespace GameMutexKiller.WindowsTrayLibrary
{
	public struct WindowPlacement
	{
		public int length;
		public int flags;
		public int showCmd;
		public Point ptMinPosition;
		public Point ptMaxPosition;
		public Rectangle rcNormalPosition;
	}
}