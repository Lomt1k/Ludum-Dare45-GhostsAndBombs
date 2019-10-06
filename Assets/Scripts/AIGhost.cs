using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGhost : MonoBehaviour
{
    public float movementSpeed; //скорость передвижения
    public float ViewDistance; //расстояние, на котором он увидит игрока

    private bool followingPlayer;

    Transform player;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").transform;
            return;
        }

        if (!followingPlayer)
        {
            if (Vector2.Distance(player.position, transform.position) < ViewDistance)
            {
                followingPlayer = true;
            }
        }
        else
        {
            float newPosX = 0f;
            if (player.position.x - transform.position.x > 0) newPosX = transform.position.x + movementSpeed * Time.fixedDeltaTime;
            else newPosX = transform.position.x - movementSpeed * Time.fixedDeltaTime;

            float newPosY = 0f;
            if (player.position.y - transform.position.y > 0) newPosY = transform.position.y + movementSpeed * Time.fixedDeltaTime;
            else newPosY = transform.position.y - movementSpeed * Time.fixedDeltaTime;

            transform.position = new Vector2(newPosX, newPosY);
            transform.LookAt(player.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bomb"))
        {
            Destroy(gameObject);
            collision.GetComponent<Bomb>().Explode();
        }

        //end game
        if (collision.CompareTag("Player"))
        {
            FindObjectOfType<MapGenerator>().GameOver();
        }
    }
}
