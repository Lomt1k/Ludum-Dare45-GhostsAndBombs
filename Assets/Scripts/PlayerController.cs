using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public Texture2D cursorTexture;
    public AudioClip startLevelSound;

    public Sprite torsoUp;
    public Sprite torsoDown;
    public Sprite torsoLeft;
    public Sprite torsoRight;

    public GameObject handsNoArmed;
    public GameObject rightHandArmed;
    public GameObject leftHandArmed;
    public GameObject bombPrefab;

    SpriteRenderer sr;
    GameObject activeHandsObject;
    int bombs;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
        activeHandsObject = handsNoArmed;

        //возврат бомбы с конца прошлого уровня
        if (FindObjectOfType<MapGenerator>().IsBombSaved) AddBomb(); //возвращаем игроку бомбу, если она осталась у него с прошлого уровня
        AudioSource.PlayClipAtPoint(startLevelSound, Camera.main.transform.position); //звук начала уровня
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        //player movement      
        transform.position += new Vector3(horizontal * moveSpeed * Time.deltaTime, vertical * moveSpeed * Time.deltaTime);


        //player direction
        if (Mathf.Abs(horizontal) > 0.3f)
        {
            if (horizontal >= 0) sr.sprite = torsoRight;
            else sr.sprite = torsoLeft;
        }
        else
        {
            if (vertical > 0) sr.sprite = torsoUp;
            else sr.sprite = torsoDown;
        }

        //hands animation
        activeHandsObject.SetActive(false);
        if (bombs == 0) activeHandsObject = handsNoArmed;
        else
        {
            if (horizontal >= 0f) activeHandsObject = rightHandArmed;
            else activeHandsObject = leftHandArmed;
        }
        activeHandsObject.SetActive(true);

        //draw bomb
        if (Input.GetMouseButtonDown(0) && bombs > 0)
        {
            bombs = 0;
            Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var bomb = Instantiate(bombPrefab, activeHandsObject.transform.position, Quaternion.identity);
            bomb.GetComponent<Rigidbody2D>().AddForce((pz - activeHandsObject.transform.position) * 200f);
        }
    }

    public bool CanTakeBomb()
    {
        return bombs == 0;
    }

    public void AddBomb()
    {
        bombs++;
    }


}
