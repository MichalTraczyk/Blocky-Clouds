using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    bool actualState;
    Interactable interact;
    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        interact = GetComponent<Interactable>();
    }
    public void PullLever()
    {
        actualState = !actualState;
        Interact(actualState);
        animator.SetBool("Down", actualState);
    }
    void Interact(bool state)
    {
        if (state)
            interact.OnEnterEvent.Invoke();
        else
            interact.OnExitEvent.Invoke();
    }
}
