using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Models.Codes
{
	public abstract class Code
		: MonoBehaviour
	{
		[SerializeField]
		private KeyCode[] combination;

		private KeyCode[] usedKeyCodes;
		private Queue<KeyCode> queue;

		private void Start()
		{
			queue = new Queue<KeyCode>(combination);
			usedKeyCodes = combination.Distinct().ToArray();
		}

		private void Update()
		{
			if (combination.Length > 0 && CanActivate())
			{
				Activate();
			}
		}

		private bool CanActivate()
		{
			if (!Input.anyKeyDown)
			{
				return false;
			}

			foreach (var keyCode in usedKeyCodes)
			{
				if (Input.GetKeyDown(keyCode))
				{
					if (queue.Dequeue() == keyCode)
					{
						if (queue.Count == 0)
						{
							queue = new Queue<KeyCode>(combination);
							return true;
						}

						return false;
					}
					else
					{
						queue = new Queue<KeyCode>(combination);
						if (queue.Peek() == keyCode)
						{
							queue.Dequeue();
						}

						return queue.Count == 0;
					}
				}
			}

			return false;
		}

		protected abstract void Activate();
	}
}
