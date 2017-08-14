using System.Security.Cryptography;
using System.Text;

namespace Assets.Scripts.Utilities
{
	public static class Hasher
	{
		public static string Hash(string input)
		{
			using (var algorithm = SHA512.Create())
			{
				var bytes = Encoding.UTF8.GetBytes(input);
				var hash = algorithm.ComputeHash(bytes);
				return Base64.Encode(hash);
			}
		}
	}
}
