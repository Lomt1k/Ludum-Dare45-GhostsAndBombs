using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombPickup : MonoBehaviour
{
    public AudioClip bombPickupSound; //звук подбора бомбы

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var pc = other.GetComponent<PlayerController>();
            if (pc.CanTakeBomb())
            {
                pc.AddBomb();
                AudioSource.PlayClipAtPoint(bombPickupSound, Camera.main.transform.position);
                Destroy(gameObject);
            }
        }
    }
}
