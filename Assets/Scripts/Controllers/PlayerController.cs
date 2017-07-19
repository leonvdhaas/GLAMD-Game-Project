using Assets.Scripts.Enumerations;
using Assets.Scripts.Extensions;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
	public class PlayerController : MonoBehaviour
	{
		[SerializeField]
		private int speed;

		private Orientation orientation = Orientation.North;

		private void Update()
		{
			// Movement
			if (Input.GetKey(KeyCode.W)) {
				Vector3 movement = Vector3.zero;
				switch (orientation) {
					case Orientation.North:
						movement = Vector3.forward;
						break;
					case Orientation.East:
						movement = Vector3.right;
						break;
					case Orientation.South:
						movement = Vector3.back;
						break;
					case Orientation.West:
						movement = Vector3.left;
						break;
				}

				transform.position += movement * speed * Time.deltaTime;
			}

			// Rotation
			if (Input.GetKeyDown(KeyCode.A)) {
				orientation = orientation.GetLeftOrientation();
			}
			else if (Input.GetKeyDown(KeyCode.D)) {
				orientation = orientation.GetRightOrientation();
			}

			transform.rotation = Quaternion.Euler(0, (int)orientation * 90, 0);
		}
	}
}
