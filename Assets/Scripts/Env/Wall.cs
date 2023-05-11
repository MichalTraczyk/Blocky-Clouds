using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;

public class Wall : MonoBehaviour
{
    [SerializeField]
    int destroyHits = 3;
    public TextMeshProUGUI text;
    public GameObject particles;
    public Transform particlesPosition;
    public void Hit()
    {
        destroyHits--;
        text.text = destroyHits.ToString();
        if (destroyHits <= 0)
            Die();
    }
    void Die()
    {
        SoundManager.Instance.PlaySound("BoxDestroy");
        Instantiate(particles, particlesPosition.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
