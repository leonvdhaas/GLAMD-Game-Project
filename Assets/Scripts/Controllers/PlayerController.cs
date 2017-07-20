using Assets.Scripts.Enumerations;
using Assets.Scripts.Extensions;
using UnityEngine;
using System.Linq;
using Assets.Scripts.Models;

namespace Assets.Scripts.Controllers
{
	public class PlayerController : MonoBehaviour
	{
		private const int LANE_DISTANCE = 2;

		[SerializeField]
		private float acceleration;

		[SerializeField]
		private float minSpeed;

		[SerializeField]
		private float maxSpeed;

		private float currentSpeed;
		private Lane lane = Lane.Middle;
		private Animator animator;

		private void Awake()
		{
			animator = GetComponent<Animator>();
			currentSpeed = (maxSpeed + minSpeed) / 2;

			Orientation = Orientation.North;
		}

		public Orientation Orientation { get; private set; }

		public bool IsOnLeftCorner { get; set; }

		public bool IsOnRightCorner { get; set; }

		public LanePosition[] TurningPositions { get; set; }

		private void Update()
		{
			if ((IsOnLeftCorner && Input.GetKeyDown(KeyCode.A)) ||
				(IsOnRightCorner && Input.GetKeyDown(KeyCode.D)))
			{
				TakeCorner();
			}
			else
			{
				LaneSwapping();
			}

			Move();
		}

		public void TakeFailedCorner()
		{
			// Failed corner behavior.

			TakeCorner();
		}

		private void TakeCorner()
		{
			// Set position
			var turningPosition = TurningPositions.OrderBy(x => Vector3.Distance(x.Position, transform.position)).First();
			transform.position = Orientation == Orientation.North || Orientation == Orientation.South
				? new Vector3(transform.position.x, transform.position.y, turningPosition.Position.z)
				: new Vector3(turningPosition.Position.x, transform.position.y, transform.position.z);
			lane = turningPosition.Lane;

			// Set rotation
			Orientation = IsOnLeftCorner ? Orientation.GetLeftOrientation() : Orientation.GetRightOrientation();
			transform.rotation = Quaternion.Euler(0, (int)Orientation * 90, 0);

			// Reset corner variables.
			IsOnLeftCorner = IsOnRightCorner = false;
			TurningPositions = null;
		}

		private void LaneSwapping()
		{
			if (lane != Lane.Left && Input.GetKeyDown(KeyCode.LeftArrow))
			{
				transform.position += Orientation.GetLeftOrientation().GetDirectionVector3() * LANE_DISTANCE;
				lane--;
			}
			else if (lane != Lane.Right && Input.GetKeyDown(KeyCode.RightArrow))
			{
				transform.position += Orientation.GetRightOrientation().GetDirectionVector3() * LANE_DISTANCE;
				lane++;
			}
		}

		private void Move()
		{
			if (currentSpeed < maxSpeed)
			{
				currentSpeed += acceleration;
				animator.SetFloat("Speed", currentSpeed);
			}

			transform.position += Orientation.GetDirectionVector3() * currentSpeed * Time.deltaTime;
		}
	}
}
