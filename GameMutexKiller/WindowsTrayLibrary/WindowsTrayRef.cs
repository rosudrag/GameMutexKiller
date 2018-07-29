using System;
using System.Runtime.InteropServices;

namespace GameMutexKiller.WindowsTrayLibrary
{
	public class WindowsTrayRef : IWindowsTrayRef
	{
		[DllImport("user32.dll")]
		private static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);

		[DllImport("user32.dll")]
		private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool GetWindowPlacement(IntPtr hWnd, ref WindowPlacement lpwndpl);

		[DllImport("kernel32.dll")]
		private static extern IntPtr GetConsoleWindow();

		[DllImport("user32.dll")]
		private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		public bool ShowWindowRef(IntPtr hWnd, int nCmdShow) => ShowWindow(hWnd, nCmdShow);

		public IntPtr GetConsoleWindowRef() => GetConsoleWindow();

		public bool EnableMenuItemRef(IntPtr hMenu, uint uIDEnableItem, uint uEnable) => EnableMenuItem(hMenu, uIDEnableItem, uEnable);

		public IntPtr GetSystemMenuRef(IntPtr hWnd, bool bRevert) => GetSystemMenu(hWnd, bRevert);

		public bool GetWindowPlacementRef(IntPtr hWnd, ref WindowPlacement lpwndpl) => GetWindowPlacement(hWnd, ref lpwndpl);
	}
}