using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Controllers;

namespace Assets.Scripts.Triggers
{
    class MoveTrigger : MonoBehaviour
    {
        private PlayerController con;
        private GameObject Car;
        private bool hasMoved;
        private float speed = 5;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                hasMoved = true;
                Car = transform.Find("CarGoed(Clone)").gameObject;
                StartMove();
            }
            if (other.gameObject.tag == "Car(Clone)")
            {
                Destroy(this.Car);
            }
        }
        private void StartMove()
        {
            float xDelta = transform.position.x - Car.transform.position.x;
            float zDelta = transform.position.z - Car.transform.position.z;
            Vector3 distance = Math.Abs(zDelta) > Math.Abs(xDelta) ? new Vector3(0, 0, zDelta) : new Vector3(xDelta, 0, 0);
            iTween.MoveTo(Car, iTween.Hash("position", Car.transform.position + distance, "easetype", iTween.EaseType.linear, "time", 1.25f));
        }
    }
}
