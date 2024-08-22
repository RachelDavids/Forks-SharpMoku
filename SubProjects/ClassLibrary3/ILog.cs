namespace SharpMoku.Logging
{
	public interface ILog
	{
		void Log(string message);
		void ClearLog();
		string GetLogMessage();
	}
}
