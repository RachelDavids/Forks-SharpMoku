using System;
using System.IO;
using System.Text;

namespace SharpMoku
{
	public interface ILog
	{
		void Log(string Message);
		void ClearLog();
		string GetLogMessage();
	}
	public class SimpleLog : ILog
	{
		//Please change it to be the real _logger framework such as log4Net
		private string fileName = "";
		public SimpleLog(string fileName)
		{
			this.fileName = fileName;
		}
		public void ClearLog()
		{
			try
			{

				System.IO.File.Delete(fileName);

			}
			catch (Exception)
			{

			}
		}
		public void Log(string Message)
		{
			// return;

			if (!Global.CurrentSettings.IsWriteLog)
			{
				return;
			}

			try
			{
				// return;
				using (StreamWriter SW = new(fileName, true))
				{
					Message = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ssss ") + Message;
					SW.WriteLine(Message);
				}
			}
			catch (Exception)
			{

			}
		}
		public static void WriteLog(string Message)
		{

		}

		public string GetLogMessage()
		{
			throw new NotImplementedException();
		}
	}
	public class StringBuilderLog : ILog
	{
		private StringBuilder _Builder = new();
		public void ClearLog()
		{
			_Builder = new StringBuilder();
		}
		public void Log(string Message)
		{

			_Builder.Append(Message).Append(Environment.NewLine);
		}
		public string GetLogMessage()
		{
			return _Builder.ToString();
		}
	}
}
