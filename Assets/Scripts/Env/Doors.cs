using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class Doors : MonoBehaviour
{
    public bool opened;
    Animator animator;
    [SerializeField]
    private int openedRequests = 0;
    CinemachineImpulseSource impulseSoure;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        impulseSoure = GetComponent<CinemachineImpulseSource>();
    }
    private void Start()
    {
        if (openedRequests > 0)
            ChangeDoorsState(true);
        else
            ChangeDoorsState(false);
    }
    private void Update()
    {
        if(openedRequests > 0&& !opened)
        {
            ChangeDoorsState(true);
        }
        else if(openedRequests == 0 && opened)
        {
            ChangeDoorsState(false);
        }
    }
    //Open and close doors from scripts
    public void ChangeDoorsState(bool state)
    {
        if (opened == state)
            return;
        impulseSoure.GenerateImpulse();
        opened = state;
        animator.SetBool("Opened", opened);
    }
    public void RequestOpen()
    {
        openedRequests++;
    }
    public void RequestClose()
    {
        openedRequests--;
    }
}
