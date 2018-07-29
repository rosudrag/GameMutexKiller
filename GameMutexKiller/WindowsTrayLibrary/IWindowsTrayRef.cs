using System;

namespace GameMutexKiller.WindowsTrayLibrary
{
	public interface IWindowsTrayRef
	{
		bool GetWindowPlacementRef(IntPtr hWnd, ref WindowPlacement lpwndpl);
		bool EnableMenuItemRef(IntPtr hMenu, uint uIDEnableItem, uint uEnable);
		IntPtr GetSystemMenuRef(IntPtr hWnd, bool bRevert);
		bool ShowWindowRef(IntPtr hWnd, int nCmdShow);
		IntPtr GetConsoleWindowRef();
	}
}