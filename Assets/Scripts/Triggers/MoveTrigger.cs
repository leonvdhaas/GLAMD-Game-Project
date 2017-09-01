using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Controllers;
using Assets.Scripts.Enumerations;
using Assets.Scripts.Extensions;

namespace Assets.Scripts.Triggers
{
    class MoveTrigger : MonoBehaviour
    {
        private PlayerController con;
        private GameObject car;
        private bool triggert;
        private float currentspeed, speed;
        private float maxspeed = 14.5f;
        private float acceleration = 0.5f;
        private Orientation orientation;

        private void Update()
        {
            if (triggert)
            {
                StartMove();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerController>() != null)
            {
                currentspeed = other.GetComponent<PlayerController>().CurrentSpeed * 0.7f; // movementspeed accordig to character speed (die manier werkt slomo ook mee)
                orientation = other.GetComponent<PlayerController>().Orientation;
                orientation = orientation.GetOppositeOrientation();
                if (gameObject.transform.parent.gameObject != null)
                {
                    car = gameObject.transform.parent.gameObject;
                }
                triggert = true;
            }
        }
        private void StartMove()
        {
            if (car != null)
            {
                speed = Mathf.MoveTowards(currentspeed, maxspeed, acceleration * Time.deltaTime);
                car.transform.position += orientation.GetDirectionVector3() * speed * Time.deltaTime;
            }
            else
            {
                gameObject.SetActive(false);
                Destroy(gameObject);
                enabled = false;
            }
        }
    }
}
