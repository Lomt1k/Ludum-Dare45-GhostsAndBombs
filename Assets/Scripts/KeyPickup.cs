using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public AudioClip keyPickupSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //обновляем подсказку о количестве собранных ключиков у двери на выходе из уровня
        if (FindObjectsOfType<KeyPickup>().Length == 2)
        {
            GameObject.Find("firstKeyIndicator").GetComponent<SpriteRenderer>().sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        }
        else
        {
            GameObject.Find("secondKeyIndicator").GetComponent<SpriteRenderer>().sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        }

        AudioSource.PlayClipAtPoint(keyPickupSound, Camera.main.transform.position);
        Destroy(gameObject);
    }
}
