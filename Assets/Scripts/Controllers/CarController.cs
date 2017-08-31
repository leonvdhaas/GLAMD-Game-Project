using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Assets.Scripts.Helpers;

namespace Assets.Scripts.Triggers
{
    public class CarController : MonoBehaviour
    {
        Vector3 curPos, lastPos;
        public bool Moving = false;

        private void Update()
        {
            StartCoroutine(CheckMoving());
            if (Moving)
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
            if (startPos.x != finalPos.x || startPos.y != finalPos.y
                || startPos.z != finalPos.z)
                Moving = true;
        }
    }
}
