using UnityEngine;

namespace Assets.Scripts.Controllers
{
	public class CameraController : MonoBehaviour
	{
		private const float PLAYER_MIN_SPEED = 12.5f;
		private const float PLAYER_MAX_SPEED = 20.0f;
		private const float PLAYER_SPEED_BONUS = 10.0f;
		private const float PLAYER_ABSOLUTE_MAX_SPEED = PLAYER_MAX_SPEED + PLAYER_SPEED_BONUS;

		[SerializeField]
		private float bobbingSpeed;

		[SerializeField]
		private float bobbingAmount;

		private PlayerController player;
		private float timer;
		private float origin;

		private void Start()
		{
			player = GetComponentInParent<PlayerController>();
			origin = transform.localPosition.y;
		}

		void FixedUpdate()
		{
			float waveslice = 0;
			float vertical = (player.CurrentSpeed - PLAYER_MIN_SPEED) / (PLAYER_ABSOLUTE_MAX_SPEED - PLAYER_MIN_SPEED);

			if (player.Frozen || player.CurrentSpeed < PLAYER_MIN_SPEED + 0.1f)
			{
				timer = 0.0f;
			}
			else
			{
				waveslice = Mathf.Sin(timer);
				timer = timer + bobbingSpeed;
				if (timer > Mathf.PI * 2)
				{
					timer = timer - (Mathf.PI * 2);
				}
			}

			Vector3 target = transform.localPosition;
			if (waveslice != 0)
			{
				float translateChange = waveslice * bobbingAmount;
				translateChange *= vertical;
				target.y = origin + translateChange;
			}
			else
			{
				target.y = origin;
			}

			transform.localPosition = target;
		}
	}
}
