using UnityEngine;

namespace Assets.Scripts.Helpers
{
	public class InputHelper
	{
		public static bool Jump()
		{
			return Input.GetKeyDown(KeyCode.Space);
		}

		public static bool LaneSwapLeft()
		{
			return Input.GetKeyDown(KeyCode.A);
		}

		public static bool LaneSwapRight()
		{
			return Input.GetKeyDown(KeyCode.D);
		}

		public static bool CornerLeft()
		{
			return Input.GetKeyDown(KeyCode.A);
		}

		public static bool CornerRight()
		{
			return Input.GetKeyDown(KeyCode.D);
		}

		public static bool ActivateInhaler()
		{
			return Input.GetKeyDown(KeyCode.Return);
		}
	}
}
