using System.IO;
using System.IO.Compression;
using System.Text;

namespace Assets.Scripts.Utility
{
	public static class Compressor
	{
		public static byte[] Zip(string input)
		{
			var bytes = Encoding.UTF8.GetBytes(input);

			using (var msIn = new MemoryStream(bytes))
			using (var msOut = new MemoryStream())
			{
				using (var gs = new GZipStream(msOut, CompressionMode.Compress))
				{
					CopyTo(msIn, gs);
				}

				return msOut.ToArray();
			}
		}

		public static string Unzip(byte[] bytes)
		{
			using (var msIn = new MemoryStream(bytes))
			using (var msOut = new MemoryStream())
			{
				using (var gs = new GZipStream(msIn, CompressionMode.Decompress))
				{
					CopyTo(gs, msOut);
				}

				return Encoding.UTF8.GetString(msOut.ToArray());
			}
		}

		private static void CopyTo(Stream source, Stream destination)
		{
			byte[] bytes = new byte[4096];

			int count;
			while ((count = source.Read(bytes, 0, bytes.Length)) != 0)
			{
				destination.Write(bytes, 0, count);
			}
		}
	}
}
