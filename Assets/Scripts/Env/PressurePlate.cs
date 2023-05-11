using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    Interactable interactable;
    private void Awake()
    {
        interactable = GetComponent<Interactable>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Interactable")
        {
            interactable.OnEnterEvent.Invoke();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Interactable")
        {
            interactable.OnExitEvent.Invoke();
        }
    }
}
