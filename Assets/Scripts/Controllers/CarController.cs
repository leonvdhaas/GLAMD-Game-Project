using System.Collections;
using UnityEngine;


namespace Assets.Scripts.Triggers
{
	public class CarController : MonoBehaviour
	{
		private Vector3 curPos, lastPos;
		private bool moving = false;

		private void Update()
		{
			StartCoroutine(CheckMoving());
			if (moving)
			{
				StartCoroutine(Destroy());
			}
		}

		private IEnumerator Destroy()
		{
			yield return new WaitForSeconds(1.7f);
			gameObject.SetActive(false);
			Destroy(gameObject);
			enabled = false;
		}

		private IEnumerator CheckMoving()
		{
			Vector3 startPos = transform.position;
			yield return new WaitForSeconds(0.1f);
			Vector3 finalPos = transform.position;
			moving = startPos.x != finalPos.x || startPos.y != finalPos.y || startPos.z != finalPos.z;
		}
	}
}
