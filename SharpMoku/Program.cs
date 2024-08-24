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
			using (Form formSharpMoku = new())
			{
				Application.Run(formSharpMoku);
			}
		}
	}
}
