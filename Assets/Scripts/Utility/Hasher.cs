using System.Security.Cryptography;
using System.Text;

namespace Assets.Scripts.Utility
{
	public static class Hasher
	{
		public static string Hash(string input)
		{
			using (SHA512 algorithm = SHA512.Create())
			{
				var bytes = Encoding.UTF8.GetBytes(input);
				var hash = algorithm.ComputeHash(bytes);
				return Encoding.UTF8.GetString(hash);
			}
		}
	}
}
