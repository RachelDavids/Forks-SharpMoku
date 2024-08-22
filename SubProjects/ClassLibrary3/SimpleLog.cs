using System;
using System.IO;

namespace SharpMoku.Logging
{
	public sealed class SimpleLog(string fileName, ILogSettings logSettings)
		: ILog
	{
		//Please change it to be the real _logger framework such as log4Net

		public void ClearLog()
		{
			try
			{
				File.Delete(fileName);
			}
			catch (Exception)
			{

			}
		}
		public void Log(string message)
		{
			// return;

			if (!logSettings.IsWriteLog)
			{
				return;
			}

			try
			{
				// return;
				using (StreamWriter sw = new(fileName, true))
				{
					message = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ssss ") + message;
					sw.WriteLine(message);
				}
			}
			catch (Exception)
			{

			}
		}
		public string GetLogMessage()
		{
			throw new NotSupportedException($"{nameof(SimpleLog)}.{nameof(GetLogMessage)}");
		}
	}
}
