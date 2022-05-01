using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingFloorParent : MonoBehaviour
{
    MovingFloor masterScript;
    private void Awake()
    {
        masterScript = GetComponentInParent<MovingFloor>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Interactable"))
        {
            other.transform.parent = this.transform;
            masterScript.blockOnIt = other.GetComponent<MovableBlock>();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            other.transform.parent = null;
            masterScript.blockOnIt = null;
        }
    }
}
