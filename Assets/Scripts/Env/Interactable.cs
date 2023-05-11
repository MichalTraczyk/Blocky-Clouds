using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[SelectionBase]
public class Interactable : MonoBehaviour
{

    public UnityEvent OnEnterEvent;
    public UnityEvent OnExitEvent;
    private void Start()
    {
        if (OnEnterEvent == null)
            OnEnterEvent = new UnityEvent();
        if (OnExitEvent == null)
            OnExitEvent = new UnityEvent();
    }
}
