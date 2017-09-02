using UnityEngine;
using Assets.Scripts.Controllers;
using Assets.Scripts.Enumerations;
using Assets.Scripts.Extensions;

namespace Assets.Scripts.Triggers
{
	class MoveTrigger : MonoBehaviour
	{
		private PlayerController con;
		private GameObject car;
		private bool triggered;
		private float currentspeed, speed;
		private float maxspeed = 14.5f;
		private float acceleration = 0.5f;
		private Orientation orientation;

		private void Update()
		{
			if (triggered)
			{
				Move();
			}
		}

		private void Move()
		{
			if (car != null)
			{
				speed = Mathf.MoveTowards(currentspeed, maxspeed, acceleration * Time.deltaTime);
				car.transform.position += orientation.GetDirectionVector3() * speed * Time.deltaTime;
			}
			else
			{
				Destroy(gameObject);
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			if (triggered)
			{
				return;
			}

			var player = other.GetComponent<PlayerController>();
			if (player != null)
			{
				currentspeed = player.CurrentSpeed * 0.7f;
				orientation = player.Orientation.GetOppositeOrientation();

				if (gameObject.transform.parent != null)
				{
					car = gameObject.transform.parent.gameObject;
				}

				triggered = true;
			}
		}
	}
}
