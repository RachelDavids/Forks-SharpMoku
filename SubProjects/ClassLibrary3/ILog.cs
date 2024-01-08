using System;
using System.IO;
using System.Text;

namespace SharpMoku
{
	public interface ILogSettings
	{
		bool IsWriteLog { get; set; }
	}

	public interface ILog
	{
		void Log(string message);
		void ClearLog();
		string GetLogMessage();
	}
	public sealed class SimpleLog
		: ILog
	{
		//Please change it to be the real _logger framework such as log4Net
		private string fileName = "";
		private readonly ILogSettings _logSettings;

		public SimpleLog(string fileName, ILogSettings logSettings)
		{
			this.fileName = fileName;
			_logSettings = logSettings;
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
		public void Log(string message)
		{
			// return;

			if (!_logSettings.IsWriteLog)
			{
				return;
			}

			try
			{
				// return;
				using (StreamWriter SW = new(fileName, true))
				{
					message = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ssss ") + message;
					SW.WriteLine(message);
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
