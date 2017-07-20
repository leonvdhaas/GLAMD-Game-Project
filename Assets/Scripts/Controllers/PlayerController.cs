using System;
using Assets.Scripts.Enumerations;
using Assets.Scripts.Extensions;
using UnityEngine;

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
		private Orientation orientation = Orientation.North;
		private Animator animator;

		private void Awake()
		{
			animator = GetComponent<Animator>();
			currentSpeed = (maxSpeed + minSpeed) / 2;
		}

		private void Update()
		{
			Move();
			LaneSwap();
			Rotate();
		}

		private void Rotate()
		{
			var previousOrientation = orientation;
			if (Input.GetKeyDown(KeyCode.A))
			{
				orientation = orientation.GetLeftOrientation();
			}
			else if (Input.GetKeyDown(KeyCode.D))
			{
				orientation = orientation.GetRightOrientation();
			}
			
			if (previousOrientation != orientation)
			{
				transform.rotation = Quaternion.Euler(0, (int)orientation * 90, 0);
			}
		}

		private void LaneSwap()
		{
			if (lane != Lane.Left && Input.GetKeyDown(KeyCode.LeftArrow))
			{
				transform.position += orientation.GetLeftOrientation().GetDirectionVector3() * LANE_DISTANCE;
				lane--;
			}
			else if (lane != Lane.Right && Input.GetKeyDown(KeyCode.RightArrow))
			{
				transform.position += orientation.GetRightOrientation().GetDirectionVector3() * LANE_DISTANCE;
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
			
			transform.position += orientation.GetDirectionVector3() * currentSpeed * Time.deltaTime;
		}
	}
}
