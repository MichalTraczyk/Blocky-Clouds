using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public GameObject particles;
    //public float rotationSpeed;

    private void OnTriggerEnter(Collider other)
    {
        //GameManager.Instance.CollectCoin();
        SoundManager.Instance.PlaySound("CollectCoin");
        Instantiate(particles, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
