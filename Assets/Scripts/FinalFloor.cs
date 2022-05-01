using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalFloor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            GameManager.Instance.FinishGame();
        }
    }
}
