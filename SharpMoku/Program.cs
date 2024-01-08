using System;
using System.Windows.Forms;

namespace SharpMoku
{
	internal static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main()
		{
			ApplicationConfiguration.Initialize();
			//    Global.CurrentSettings.BoardSize = 15;
			//   Global.SaveSettings();
			using (FormSharpMoku formSharpMoku = new())
			{
				Application.Run(formSharpMoku);
			}
		}
	}
}
