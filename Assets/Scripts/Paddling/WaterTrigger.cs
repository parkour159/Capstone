using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTrigger : MonoBehaviour
{
    private Transform boat;
    private Paddling paddling;

    private void Start()
    {
        boat = GameObject.Find("Final_Boat").transform;
        paddling = GameObject.Find("Final_Boat").GetComponent<Paddling>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Paddle"))
        {
            paddling.isTriggered = true;
            paddling.oarTriggerEnterPos = boat.InverseTransformDirection(other.transform.position);
            paddling._previousBoatPos = boat.InverseTransformDirection(boat.transform.position);
            Debug.Log("Trigger Enter");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Paddle"))
        {
            paddling.isTriggered = false;
            paddling.oarTriggerExitPos = boat.InverseTransformDirection(other.transform.position);
            paddling._currentBoatPos = boat.InverseTransformDirection(boat.transform.position);
        }
    }
}
