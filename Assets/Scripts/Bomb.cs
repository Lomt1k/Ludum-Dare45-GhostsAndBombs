using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public AudioClip drawBombSound;
    public AudioClip explosionSound;
    public GameObject explosionPrefab;

    private void Start()
    {
        AudioSource.PlayClipAtPoint(drawBombSound, Camera.main.transform.position);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponent<CircleCollider2D>().isTrigger = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) return; //игнорирует игрока
        Explode();
    }

    public void Explode()
    {
        AudioSource.PlayClipAtPoint(explosionSound, Camera.main.transform.position);
        GameObject exp = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(exp, 0.8f); //исчезновение взрыва
        Destroy(gameObject);
    }
}
