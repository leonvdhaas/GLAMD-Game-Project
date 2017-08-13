using System;

namespace Assets.Scripts.Utilities
{
	public static class Base64
	{
		public static string Encode(byte[] data)
		{
			return Convert.ToBase64String(data);
		}

		public static byte[] Decode(string data)
		{
			return Convert.FromBase64String(data);
		}
	}
}
