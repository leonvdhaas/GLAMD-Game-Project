using Assets.Scripts.Enumerations;
using Assets.Scripts.Extensions;
using UnityEngine;
using System.Linq;
using Assets.Scripts.Models;
using System;
using Assets.Scripts.Helpers;
using Assets.Scripts.Managers;

namespace Assets.Scripts.Controllers
{
	public class PlayerController : MonoBehaviour
	{
		private const float GRAVITY = 9.8f;

		[SerializeField]
		private float acceleration;

		[SerializeField]
		private float minSpeed;

		[SerializeField]
		private float maxSpeed;

		[SerializeField]
		private float jumpSpeed;

		[SerializeField]
		private float inhalerSpeedBonus;

		[SerializeField]
		private float inhalerTime;

		[SerializeField]
		private float laneSwapSpeed;

		[SerializeField]
		private float maximumSwipeTime;

		[SerializeField]
		private float minimumSwipeDistance;

		private float currentSpeed;
		private Lane lane = Lane.Middle;
		private Animator animator;
		private CharacterController characterController;
		private float verticalSpeed = 0.0f;
		private Vector3 targetLanePos;
		private Vector2 touchOrigin = -Vector2.one;
		private float fingerStartTime = 0.0f;

		private void Start()
		{
			GameManager.Instance.Player = this;
			CurrentTile = TileManager.Instance.Tiles.First();
		}

		private void Awake()
		{
			characterController = GetComponent<CharacterController>();
			animator = GetComponent<Animator>();
			currentSpeed = (maxSpeed + minSpeed) / 2;
			Orientation = Orientation.North;

			Frozen = true;
			StartCoroutine(CoroutineHelper.Delay(3, () => Frozen = false));
			StartCoroutine(CoroutineHelper.Repeat(1, () =>
			{
				if (!Frozen)
				{
					Points++;
				}
			}, () => Lives > 0));
		}

		private int _lives = 3;
		public int Lives
		{
			get
			{
				return _lives;
			}
			set
			{
				GameManager.Instance.GuiManager.UpdateLives(_lives = value);
			}
		}

		private int _points = 0;
		public int Points
		{
			get
			{
				return _points;
			}
			set
			{
				GameManager.Instance.GuiManager.UpdatePoints(_points = value);
			}
		}

		private int _coins = 0;
		public int Coins
		{
			get
			{
				return _coins;
			}
			set
			{
				GameManager.Instance.GuiManager.UpdateCoins(_coins = value);
			}
		}

		public Orientation Orientation { get; private set; }

		public LanePosition[] TurningPositions { get; set; }

		public bool IsOnLeftCorner { get; set; }

		public bool IsOnRightCorner { get; set; }

		public Tile CurrentTile { get; set; }

		public bool IsDamaged { get; private set; }

		public int Inhalers { get; internal set; }

		public bool IsCoinDoublerActive { get; set; }

		public bool IsInvincible { get; set; }

		public int HorizontalSwipeDirection { get; set; }

		public int VerticalSwipeDirection { get; set; }

		public bool Frozen { get; private set; }

		public bool InhalerUsable
		{
			get
			{
				return Inhalers == PickupController.MAX_NUMBER_OF_INHALERS;
			}
		}

		public bool IsOnCorner
		{
			get
			{
				return (IsOnLeftCorner || IsOnRightCorner) && TurningPositions != null && TurningPositions.Length > 0;
			}
		}

		private void Update()
		{
			ApplyGravity();

			if (IsDamaged || Frozen)
			{
				return;
			}

			MoveToCorrectLane();
			GetTouchInput();

			if ((IsOnLeftCorner && InputHelper.CornerLeft()) ||
				(IsOnRightCorner && InputHelper.CornerRight()))
			{
				TakeCorner();
			}
			else if (!IsOnCorner)
			{
				CheckForLaneSwap();
			}

			Move();
			Jump();
			ActivateInhaler();
		}

		private void Jump()
		{
			if (InputHelper.Jump() && IsAllowedToJump())
			{
				verticalSpeed = jumpSpeed;
			}
		}

		private void ApplyGravity()
		{
			const float gravityMultiplier = 7.5f;

			if (verticalSpeed > 0 || !IsTouchingGround())
			{
				verticalSpeed -= gravityMultiplier * GRAVITY * Time.deltaTime;
				characterController.Move(verticalSpeed * Vector3.up * Time.deltaTime);
			}
		}

		private bool IsAllowedToJump()
		{
			return !IsOnCorner && verticalSpeed <= 0 && IsTouchingGround();
		}

		private bool IsTouchingGround()
		{
			var touching = Physics.Raycast(transform.position + Vector3.up * 0.6f, Vector3.down, 0.2f);
			if (touching)
			{
				verticalSpeed = 0;
			}

			return touching;
		}

		public void TakeFailedCorner()
		{
			if (!IsOnCorner)
			{
				throw new InvalidOperationException("Tried to take invalid corner.");
			}

			IsDamaged = true;
			animator.SetFloat("Speed", 0.0f);
			currentSpeed = minSpeed;

			Lives--;
			if (Lives != ushort.MinValue)
			{
				// Wait until player lands.
				StartCoroutine(CoroutineHelper.WaitUntil(() => IsTouchingGround(), () =>
				{
					animator.Play("Damage");
					StartCoroutine(CoroutineHelper.Delay(0.75f, () => IsDamaged = false));

					TakeCorner();
				}));
			}
		}

		private void TakeCorner()
		{
			// Set position
			var turningPosition = TurningPositions.OrderBy(x => Vector3.Distance(x.Position, transform.position)).First();
			transform.position = Orientation == Orientation.North || Orientation == Orientation.South
				? new Vector3(transform.position.x, transform.position.y, turningPosition.Position.z)
				: new Vector3(turningPosition.Position.x, transform.position.y, transform.position.z);
			lane = turningPosition.Lane;
			targetLanePos = Orientation == Orientation.North || Orientation == Orientation.South
				? Vector3.forward.Multiply(transform.position)
				: Vector3.right.Multiply(transform.position);

			// Set rotation
			Orientation = IsOnLeftCorner ? Orientation.GetLeftOrientation() : Orientation.GetRightOrientation();
			iTween.RotateTo(gameObject, new Vector3(0, (int)Orientation * 90, 0), IsDamaged ? 1.5f : 0.5f);

			// Reset corner variables.
			IsOnLeftCorner = IsOnRightCorner = false;
			TurningPositions = null;
		}

		private void CheckForLaneSwap()
		{
			if (lane != Lane.Left && InputHelper.LaneSwapLeft())
			{
				targetLanePos += Orientation.GetLeftOrientation().GetDirectionVector3() * Tile.LANE_DISTANCE;
				lane--;
			}
			else if (lane != Lane.Right && InputHelper.LaneSwapRight())
			{
				targetLanePos += Orientation.GetRightOrientation().GetDirectionVector3() * Tile.LANE_DISTANCE;
				lane++;
			}
		}

		private void MoveToCorrectLane()
		{
			if (Orientation == Orientation.North || Orientation == Orientation.South)
			{
				transform.position = transform.position.CreateNew(x: Mathf.MoveTowards(transform.position.x, targetLanePos.x, laneSwapSpeed * Time.deltaTime));
			}
			else
			{
				transform.position = transform.position.CreateNew(z: Mathf.MoveTowards(transform.position.z, targetLanePos.z, laneSwapSpeed * Time.deltaTime));
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

		private void ActivateInhaler()
		{
			if (InhalerUsable && InputHelper.ActivateInhaler())
			{
				IsInvincible = true;
				StartCoroutine(CoroutineHelper.Repeat(
					inhalerTime / PickupController.MAX_NUMBER_OF_INHALERS,
					() => Inhalers--,
					() => Inhalers > 0,
					() =>
					{
						maxSpeed -= inhalerSpeedBonus;
						IsInvincible = false;
					}));
			}
		}

		public void TakeObstacleDamage()
		{
			IsDamaged = true;
			Lives--;
			if(Lives != ushort.MinValue)
			{
				animator.Play("Damage");
				StartCoroutine(CoroutineHelper.Delay(0.75f, () => IsDamaged = false));
			}			
			animator.SetFloat("Speed", 0.0f);
			currentSpeed = minSpeed;
		}

		private void GetTouchInput()
		{
			if (Input.touchCount > 0)
			{
				Touch myTouch = Input.GetTouch(0);

				switch (myTouch.phase)
				{
					case TouchPhase.Began:
						touchOrigin = myTouch.position;
						fingerStartTime = Time.time;
						break;
					case TouchPhase.Moved:
					case TouchPhase.Ended:
					case TouchPhase.Canceled:
						SetSwipeDirection(myTouch);
						break;
					default:
						break;
				}
			}
		}

		private void SetSwipeDirection(Touch myTouch)
		{
			if (touchOrigin.x >= 0)
			{
				Vector2 touchEnd = myTouch.position;

				Vector2 direction = touchEnd - touchOrigin;

				float gestureDistance = direction.magnitude;
				float gestureTime = Time.time - fingerStartTime;

				touchOrigin.x = -1;

				if (gestureTime < maximumSwipeTime && gestureDistance > minimumSwipeDistance && direction.x != 0 && direction.y != 0)
				{
					if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
					{
						HorizontalSwipeDirection = direction.x > 0 ? 1 : -1;
					}
					else
					{
						VerticalSwipeDirection = direction.y > 0 ? 1 : -1;
					}
				}

			}
		}
	}
}
