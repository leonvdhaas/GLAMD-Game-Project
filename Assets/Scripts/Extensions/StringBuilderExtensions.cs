using System.Text;

namespace Assets.Scripts.Extensions
{
	public static class StringBuilderExtensions
	{
		public static StringBuilder Join(this StringBuilder sb, string seperator, params string[] values)
		{
			int i = 0;
			while (true)
			{
				sb.Append(values[i]);
				if (++i < values.Length)
				{
					sb.Append(seperator);
				}
				else
				{
					break;
				}
			}

			return sb;
		}
	}
}
