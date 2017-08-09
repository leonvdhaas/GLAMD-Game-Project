using Assets.Scripts.Controllers;
using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.Helpers
{
	public static class InputHelper
	{
		public static bool Jump()
		{
			bool jumped = Input.GetKeyDown(KeyCode.Space) || GameManager.Instance.Player.VerticalSwipeDirection == 1;
			GameManager.Instance.Player.VerticalSwipeDirection = 0;
			return jumped;
		}

		public static bool LaneSwapLeft()
		{
			bool swappedLeft = Input.GetKeyDown(KeyCode.A) || GameManager.Instance.Player.HorizontalSwipeDirection == -1;
			GameManager.Instance.Player.HorizontalSwipeDirection = 0;
			return swappedLeft;
		}

		public static bool LaneSwapRight()
		{
			bool swappedRight = Input.GetKeyDown(KeyCode.D) || GameManager.Instance.Player.HorizontalSwipeDirection == 1;
			GameManager.Instance.Player.HorizontalSwipeDirection = 0;
			return swappedRight;
		}

		public static bool CornerLeft()
		{
			bool tookLeftCorner = Input.GetKeyDown(KeyCode.A) || GameManager.Instance.Player.HorizontalSwipeDirection == -1;
			GameManager.Instance.Player.HorizontalSwipeDirection = 0;
			return tookLeftCorner;
		}

		public static bool CornerRight()
		{
			bool tookRightCorner = Input.GetKeyDown(KeyCode.D) || GameManager.Instance.Player.HorizontalSwipeDirection == 1;
			GameManager.Instance.Player.HorizontalSwipeDirection = 0;
			return tookRightCorner;
		}

		public static bool ActivateInhaler()
		{
			return Input.GetKeyDown(KeyCode.Return);
		}
	}
}
