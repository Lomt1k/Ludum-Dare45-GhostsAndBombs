using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndTrigger : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (FindObjectOfType<KeyPickup>() != null) return; //если еще есть ключи на уровне - не пропускает
            FindObjectOfType<MapGenerator>().GenerateMap(); //next level
        }        
    }

}
