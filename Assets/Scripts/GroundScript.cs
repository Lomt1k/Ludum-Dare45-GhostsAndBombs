using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundScript : MonoBehaviour
{
    public Sprite[] groundSprites;
    public AudioClip[] groundSounds;

    // Start is called before the first frame update
    void Start()
    {
        //--- установка сеттинга
        int level = FindObjectOfType<MapGenerator>().GetLevel();        
        int groundIndex = level / 5; //меняется каждые 5 уровней
        int backgroundIndex = 4; //bg по умолчанию - ground

        //если нет нужного спрайта для уровня (закончились) - идём по второму кругу
        while (groundIndex >= groundSprites.Length)
        {
            groundIndex -= groundSprites.Length;
            //высчитываем нужный задний фон
            backgroundIndex++;
            if (backgroundIndex >= groundSprites.Length) backgroundIndex = 0;
        }
        gameObject.GetComponent<SpriteRenderer>().sprite = groundSprites[groundIndex];

        //музыка (если нужно изменить - меняем
        AudioSource audio = GameObject.Find("MapGenerator").GetComponent<AudioSource>();
        if (audio.clip != groundSounds[groundIndex])
        {
            audio.clip = groundSounds[groundIndex];
            audio.Play();
        }
        

        transform.Find("backGround").GetComponent<SpriteRenderer>().sprite = groundSprites[backgroundIndex];

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
