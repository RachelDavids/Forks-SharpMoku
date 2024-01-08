using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SharpMoku.Utility
{
	public static class SerializeUtility
	{
		public static void CreateNewSettings(string filename)
		{
			Serialize(new SharpMokuSettings(), filename);
		}
		public static void SerializeSettings(SharpMokuSettings setting, string filename)
		{
			Serialize(setting, filename);
		}
		public static SharpMokuSettings DeserializeSettings(string filename)
		{
			return Deserialize<SharpMokuSettings>(filename);
		}

		public static void SerializeBoard(Board board, string filename)
		{
			Serialize(board, filename);
		}
		public static Board DeserializeBoard(string filename)
		{
			return Deserialize<Board>(filename);
		}
		private static void Serialize<T>(T obj, string filename)
		{
			Stream ms = File.OpenWrite(filename);
			//Format the object as Binary
#pragma warning disable SYSLIB0011 // Type or member is obsolete
			BinaryFormatter formatter = new();
#pragma warning restore SYSLIB0011 // Type or member is obsolete
			formatter.Serialize(ms, obj);
			ms.Flush();
			ms.Close();
			ms.Dispose();
		}

		private static T Deserialize<T>(string filename)
		{
			//Format the object as Binary
#pragma warning disable SYSLIB0011 // Type or member is obsolete
			BinaryFormatter formatter = new();
#pragma warning restore SYSLIB0011 // Type or member is obsolete

			//Reading the file from the server
			FileStream fs = File.Open(filename, FileMode.Open);

			T obj = (T)formatter.Deserialize(fs);
			fs.Flush();
			fs.Close();
			fs.Dispose();
			return obj;

		}
	}
}
