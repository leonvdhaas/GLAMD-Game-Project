using System;
using UnityEngine;

public class SwipeControl : MonoBehaviour
{

	public enum SWIPE_DIRECTION
	{
		SD_UP
		, SD_DOWN
		, SD_LEFT
		, SD_RIGHT
		, SD_UP_RIGHT
		, SD_DOWN_RIGHT
		, SD_DOWN_LEFT
		, SD_UP_LEFT
		, SD_TOUCH
	}

	public enum SWIPE_TYPE
	{
		ST_8_DIRECTION
		, ST_4_DIRECTION
	}

	/* Callbacks */
	public delegate void Callback(SWIPE_DIRECTION iDirection);
	private Callback m_cCallback;
	private Callback m_cCurrentCallback;

	/* Public parameters */
	public bool m_bMobileTouchBased = false;
	public bool m_bActiveVisualFeedback = true;
	public bool m_bLeftClick = true;
	public bool m_bInstantSwipe = false;
	public SWIPE_TYPE m_stSwipeType = SWIPE_TYPE.ST_8_DIRECTION;

	[Range(0.0f, 10.0f)]
	public float SwipeDistance = 0.2f;

	/* Private parameters */
	private bool m_bSwipePressed = false;
	private Vector3 m_vStarSwipePos;
	private Vector3 m_vEndSwipePos;
	private Vector3 m_vSwipeDirection;
	private LineRenderer m_lLineRenderer;
	private int m_iNumberOfPoints = 0;
	private int m_iInputMouse = 0;

	private float m_fHeightSensibility = 0.2f;
	private float m_fWidthSensibility = 0.2f;

	// Custom Implementations
	private float fingerStartTime;
	private Vector2 fingerStartPos;

	[SerializeField]
	private float maxSwipeTime;

	[SerializeField]
	private float minSwipeDist;

	[SerializeField]
	[Range(0.0f, 10.0f)]
	private float instantSwipeDistance;

	/* This method allow you to set the callback. 
	 * When a Swipe gesture is done, the input callback
	 * will called. */
	public void SetMethodToCall(Callback iCallback)
	{
		if (m_cCurrentCallback != null)
		{
			m_cCallback -= m_cCurrentCallback;
		}

		m_cCallback += iCallback;
		m_cCurrentCallback = m_cCallback;

		m_lLineRenderer = this.GetComponent<LineRenderer>();
	}

	private void UpdateDebugLineRenderer()
	{
		if (Input.GetKey(KeyCode.Mouse0))
		{
			m_iNumberOfPoints++;
			m_lLineRenderer.SetVertexCount(m_iNumberOfPoints);

			Vector3 mousePos = new Vector3(0.0f, 0.0f, 0.0f);
			mousePos = Input.mousePosition;
			mousePos.z = 1.0f;
			Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

			m_lLineRenderer.SetPosition(m_iNumberOfPoints - 1, worldPos);
		}
		else
		{
			m_iNumberOfPoints = 0;
			m_lLineRenderer.SetVertexCount(0);
		}
	}

	private void CheckAndComputeDirection4Direction()
	{

		if (m_vSwipeDirection.normalized.x > 0.0f
				&& m_vSwipeDirection.normalized.x > m_fWidthSensibility)
		{

			if (m_vSwipeDirection.x > 0.0f)
			{
				if (m_vSwipeDirection.normalized.y >= 0.5f
					&& m_vSwipeDirection.normalized.y > m_fHeightSensibility)
				{
					m_cCallback(SWIPE_DIRECTION.SD_UP);
				}
				else if (m_vSwipeDirection.normalized.y <= -0.5f
					&& m_vSwipeDirection.normalized.y < -m_fHeightSensibility)
				{
					m_cCallback(SWIPE_DIRECTION.SD_DOWN);
				}
				else
				{
					m_cCallback(SWIPE_DIRECTION.SD_RIGHT);
				}
			}
		}
		else if (m_vSwipeDirection.normalized.x < 0.0f
					&& m_vSwipeDirection.normalized.x < -m_fWidthSensibility)
		{

			if (m_vSwipeDirection.normalized.y >= 0.5f
					&& m_vSwipeDirection.normalized.y > m_fHeightSensibility)
			{
				m_cCallback(SWIPE_DIRECTION.SD_UP);
			}
			else if (m_vSwipeDirection.normalized.y <= -0.5f
						&& m_vSwipeDirection.normalized.y < -m_fHeightSensibility)
			{
				m_cCallback(SWIPE_DIRECTION.SD_DOWN);
			}
			else
			{
				m_cCallback(SWIPE_DIRECTION.SD_LEFT);
			}
		}
		else if (m_vSwipeDirection.normalized.y >= 0.0f)
		{
			m_cCallback(SWIPE_DIRECTION.SD_UP);
		}
		else
		{
			m_cCallback(SWIPE_DIRECTION.SD_DOWN);
		}
	}

	private void CheckAndComputeDirection()
	{

		if (m_vSwipeDirection.normalized.x > 0.0f
				&& m_vSwipeDirection.normalized.x > m_fWidthSensibility)
		{

			if (m_vSwipeDirection.x > 0.0f)
			{
				if (m_vSwipeDirection.normalized.y > 0.0f
						&& m_vSwipeDirection.normalized.y > m_fHeightSensibility)
				{
					m_cCallback(SWIPE_DIRECTION.SD_UP_RIGHT);
				}
				else if (m_vSwipeDirection.normalized.y < 0.0f
							&& m_vSwipeDirection.normalized.y < -m_fHeightSensibility)
				{
					m_cCallback(SWIPE_DIRECTION.SD_DOWN_RIGHT);
				}
				else
				{
					m_cCallback(SWIPE_DIRECTION.SD_RIGHT);
				}
			}
		}
		else if (m_vSwipeDirection.normalized.x < 0.0f
					&& m_vSwipeDirection.normalized.x < -m_fWidthSensibility)
		{

			if (m_vSwipeDirection.normalized.y > 0.0f
					&& m_vSwipeDirection.normalized.y > m_fHeightSensibility)
			{
				m_cCallback(SWIPE_DIRECTION.SD_UP_LEFT);
			}
			else if (m_vSwipeDirection.normalized.y < 0.0f
						&& m_vSwipeDirection.normalized.y < -m_fHeightSensibility)
			{
				m_cCallback(SWIPE_DIRECTION.SD_DOWN_LEFT);
			}
			else
			{
				m_cCallback(SWIPE_DIRECTION.SD_LEFT);
			}
		}
		else
		{

			if (m_vSwipeDirection.normalized.y >= 0.0f)
			{
				m_cCallback(SWIPE_DIRECTION.SD_UP);
			}
			else
			{
				m_cCallback(SWIPE_DIRECTION.SD_DOWN);
			}
		}
	}

	private void UpdateMouseBasedControl()
	{

		if (m_bLeftClick)
		{
			m_iInputMouse = 0;
		}
		else
		{
			m_iInputMouse = 1;
		}

		if (!m_bSwipePressed && Input.GetMouseButtonDown(m_iInputMouse))
		{
			m_vStarSwipePos = Input.mousePosition;
			m_bSwipePressed = true;
		}

		if (m_bSwipePressed)
		{

			if (!m_bInstantSwipe && Input.GetMouseButtonUp(m_iInputMouse))
			{
				/* If is a Normal swipe, we have to compute the swipe
				 * only after the release of the touch. */

				m_vEndSwipePos = Input.mousePosition;
				m_bSwipePressed = false;

				m_vSwipeDirection = m_vEndSwipePos - m_vStarSwipePos;

				if (m_vSwipeDirection.sqrMagnitude < SwipeDistance * 1000.0f)
				{
					m_cCallback(SWIPE_DIRECTION.SD_TOUCH);
					return;
				}
				else
				{
					if (m_stSwipeType == SWIPE_TYPE.ST_4_DIRECTION)
					{
						CheckAndComputeDirection4Direction();
					}
					else
					{
						CheckAndComputeDirection();
					}
				}
			}
			else if (m_bInstantSwipe)
			{
				/* Is an Instant Swipe, so we have to compute the swipe
				 * immidiatly after a change of direction */

				m_vEndSwipePos = Input.mousePosition;
				m_vSwipeDirection = m_vEndSwipePos - m_vStarSwipePos;

				if (m_vSwipeDirection.sqrMagnitude > instantSwipeDistance * 1000.0f)
				{
					m_bSwipePressed = false;

					if (m_stSwipeType == SWIPE_TYPE.ST_4_DIRECTION)
					{
						CheckAndComputeDirection4Direction();
					}
					else
					{
						CheckAndComputeDirection();
					}
				}
			}
		}
	}

	private void CallCallback(SWIPE_DIRECTION direction, Touch touch)
	{
		fingerStartTime = Time.time;
		fingerStartPos = touch.position;
		m_cCallback(direction);
	}

	private void UpdateMobileTouchBasedControlThatActuallyWorks()
	{
		if (Input.touchCount > 0)
		{
			foreach (Touch touch in Input.touches)
			{
				switch (touch.phase)
				{
					case TouchPhase.Began:
						fingerStartTime = Time.time;
						fingerStartPos = touch.position;
						break;
					case TouchPhase.Ended:
					case TouchPhase.Moved:
						float gestureTime = Time.time - fingerStartTime;
						float gestureDist = (touch.position - fingerStartPos).magnitude;
						if (gestureTime < maxSwipeTime && gestureDist > minSwipeDist)
						{
							Vector2 direction = touch.position - fingerStartPos;
							Vector2 swipeType = Vector2.zero;

							if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
							{
								//horizontal swipe
								swipeType = Vector2.right * Mathf.Sign(direction.x);
							}
							else
							{
								swipeType = Vector2.up * Mathf.Sign(direction.y);
							}

							if (swipeType.x != 0.0f)
							{
								if (swipeType.x > 0.0f)
								{
									CallCallback(SWIPE_DIRECTION.SD_RIGHT, touch);
								}
								else
								{
									CallCallback(SWIPE_DIRECTION.SD_LEFT, touch);
								}
							}

							if (swipeType.y != 0.0f)
							{
								if (swipeType.y > 0.0f)
								{
									CallCallback(SWIPE_DIRECTION.SD_UP, touch);
								}
								else
								{
									CallCallback(SWIPE_DIRECTION.SD_DOWN,  touch);
								}
							}
						}

						break;
				}
			}
		}
	}

	private void UpdateMobileTouchBasedControl()
	{
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
		{

			m_vSwipeDirection = Input.GetTouch(0).deltaPosition;

			if (m_stSwipeType == SWIPE_TYPE.ST_4_DIRECTION)
			{
				CheckAndComputeDirection4Direction();
			}
			else
			{
				CheckAndComputeDirection();
			}
		}
		else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Stationary)
		{
			m_cCallback(SWIPE_DIRECTION.SD_TOUCH);
		}
	}

	private void Update()
	{

		if (m_bActiveVisualFeedback)
		{
			UpdateDebugLineRenderer();
		}

		if (m_bMobileTouchBased)
		{
			UpdateMobileTouchBasedControlThatActuallyWorks();
		}
		else
		{
			UpdateMouseBasedControl();
		}
	}

	public void CancelCurrentSwipe()
	{
		m_vStarSwipePos = Vector3.zero;
		m_bSwipePressed = false;
	}
}
