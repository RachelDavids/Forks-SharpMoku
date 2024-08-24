using System;
using System.Text;

namespace SharpMoku.Logging
{
	public class StringBuilderLog
		: ILog
	{
		private StringBuilder _builder = new();
		public void ClearLog()
		{
			_builder = new();
		}
		public void Log(string message)
		{

			_builder.Append(message).Append(Environment.NewLine);
		}
		public string GetLogMessage()
		{
			return _builder.ToString();
		}
	}
}
