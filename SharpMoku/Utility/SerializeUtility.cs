using System.IO;
#if XNETFRAMEWORK
using System.Runtime.Serialization.Formatters.Binary;
#else
using System.Text.Json;
#endif

namespace SharpMoku.Utility
{
	public static class SerializeUtility
	{
		private static readonly JsonSerializerOptions s_options = new(JsonSerializerDefaults.Web) {
			WriteIndented = true,
		};
#if XNETFRAMEWORK
		public static void Serialize<T>(T obj, string filename)
		{
			using (Stream ms = File.OpenWrite(filename))
			{
				BinaryFormatter formatter = new();
				formatter.Serialize(ms, obj);
				ms.Flush();
			}
		}

		public static T Deserialize<T>(string filename)
		{
			//Format the object as Binary
			BinaryFormatter formatter = new();
			//Reading the file from the server
			T obj;
			using (FileStream fs = File.Open(filename, FileMode.Open, FileAccess.Read))
			{
				obj = (T)formatter.Deserialize(fs);
				fs.Flush();
			}
			return obj;
		}
#else
		public static void Serialize<T>(T obj, string filename)
		{
			using (Stream ms = File.OpenWrite(filename))
			{
				JsonSerializer.Serialize(ms, obj, s_options);
				ms.Flush();
			}
		}

		public static T Deserialize<T>(string filename)
		{
			using (FileStream fs = File.Open(filename, FileMode.Open, FileAccess.Read))
			{
				return JsonSerializer.Deserialize<T>(fs, s_options);
			}
		}
#endif
	}
}
