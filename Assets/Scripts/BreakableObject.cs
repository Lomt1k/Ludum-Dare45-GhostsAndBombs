using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    public Sprite breakableSprite;

    private void Start()
    {
        if (breakableSprite != null)
        {
            GetComponent<SpriteRenderer>().sprite = breakableSprite;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bomb"))
        {
            Destroy(gameObject);
        }

    }
}
