using System;
using System.Runtime.InteropServices;

namespace SharpMoku
{
	public class Clicking
	{
		[DllImport("user32.dll")]
		private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);
		private const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
		private const uint MOUSEEVENTF_LEFTUP = 0x0004;
		[DllImport("user32.dll")]
		private static extern void mouse_event(
			   uint dwFlags, // motion and click options
			   uint dx, // horizontal position or change
			   uint dy, // vertical position or change
			   uint dwData, // wheel movement
			   IntPtr dwExtraInfo // application-defined information
		);

		public static void SendClick()
		{

			mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, new System.IntPtr());
			mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, new System.IntPtr());
		}

	}
}
