using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SharpMoku.Utility
{
	public static class SerializeUtility
	{

		public static void Serialize<T>(T obj, string filename)
		{
			Stream ms = File.OpenWrite(filename);
			//Format the object as Binary
			BinaryFormatter formatter = new();
			formatter.Serialize(ms, obj);
			ms.Flush();
			ms.Close();
			ms.Dispose();
		}

		public static T Deserialize<T>(string filename)
		{
			//Format the object as Binary
			BinaryFormatter formatter = new();

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
