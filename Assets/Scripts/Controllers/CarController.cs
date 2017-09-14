using Assets.Scripts.Enumerations;
using Assets.Scripts.Extensions;
using Assets.Scripts.Managers;
using UnityEngine;


namespace Assets.Scripts.Controllers
{
	public class CarController
		: MonoBehaviour
	{
		[SerializeField]
		private float currentSpeed;

		[SerializeField]
		private float maxSpeed;

		[SerializeField]
		private float acceleration;

		private bool started;
		private Orientation orientation;

		public void StartDriving(Orientation orientation)
		{
			this.orientation = orientation;
			started = true;
		}

		private void Update()
		{
			if (GameManager.Instance.CurrentGame.GameOver)
			{
				Destroy(gameObject);
			}
			else if (started)
			{
				currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.deltaTime);
				transform.position += orientation.GetDirectionVector3() * currentSpeed * Time.deltaTime;
			}
		}
	}
}
