using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapGenerator : MonoBehaviour
{
    public GameObject LevelPrefab;
    public GameObject playerPrefab;
    public GameObject bombPrefab;
    public GameObject keyPrefab;
    public GameObject ghostPrefab;
    public GameObject finishLevelPrefab;
    public Sprite breakableTexture;
    public Text UI_StatText;
    public Text UI_DeathText;
    public Text UI_BestResult;
    public AudioClip gameOverMusic;

    private GameObject currentLevel;
    private List<GameObject> currentSpawnables;
    private bool bombSaved;
    private int levelNumber; //номер уровня с начала игры

    // Start is called before the first frame update
    void Start()
    {
        currentSpawnables = new List<GameObject>();
        GenerateMap();
    }

    public bool IsBombSaved
    {
        get => bombSaved;
    }

    public void GenerateMap()
    {
        //уровень генерируется не в первый раз
        if (currentLevel != null)
        {
            bombSaved = !GameObject.FindObjectOfType<PlayerController>().CanTakeBomb(); //смотрим - была ли у игрока бомба (сохраняем её)
            //удаляем все объекты с прошлого уровня
            foreach (var obj in currentSpawnables)
            {
                Destroy(obj);
            }
            currentSpawnables.Clear();
            //пересоздаем уровень
            Destroy(currentLevel);
            currentLevel = Instantiate(LevelPrefab);
            //стата
            levelNumber++;
            Time.timeScale += 0.05f;
            UpdateUIStats();

        }
        else currentLevel = GameObject.Find("Level");


        #region Генератор дверей

        //получаем объекты всех дверей
        GameObject[] doors = new GameObject[currentLevel.transform.Find("Doors").childCount];
        for (int i = 0; i < doors.Length; i++)
        {
            doors[i] = currentLevel.transform.Find("Doors").GetChild(i).gameObject;
        }
        

        int rand, newrand;
        //открываем одну из трёх дверей на верхнем уровне (первая горизонтальная линия дверей)
        rand = Random.Range(0, 3);
        doors[rand].SetActive(false);

        //--- дополнительно: решаем - стоит ли сделать одну из оставшихся двух дверей разрушаемой
        if (Random.Range(0, 2) == 1)
        {
            do
            {
                newrand = Random.Range(0, 3);
                doors[newrand].AddComponent<BreakableObject>().breakableSprite = breakableTexture;
            } while (newrand == rand);

        }

        //открываем две двери на нижнем уровне (вторая горизонтальная линия дверей)
        rand = Random.Range(3, 6);
        doors[rand].SetActive(false);

        do
        {
            newrand = Random.Range(3, 6);
        } while (newrand == rand);
        doors[newrand].SetActive(false);

        //среди вертикальных дверей с элементами 6 и 7 одну нужно открыть обязательно, другую: либо открыть, либо сделать разрушаемой!
        //решаем - открыть ли дверь 6
        if (Random.Range(0, 2) == 1)
        {
            //открываем дверь 6, а седьмую - рандом
            doors[6].SetActive(false);
            //дверь 7 можно открывать, а можно сделать разрушаемой - на выбор
            if (Random.Range(0, 2) == 1) doors[7].SetActive(false);
            else doors[7].AddComponent<BreakableObject>().breakableSprite = breakableTexture; //тут обязательно делать разрушаемой!
        }
        else
        {
            //открываем дверь 7, а шестую - рандом
            doors[7].SetActive(false);
            //дверь 6 можно открывать, а можно сделать разрушаемой - на выбор
            if (Random.Range(0, 2) == 1) doors[6].SetActive(false);
            else doors[6].AddComponent<BreakableObject>().breakableSprite = breakableTexture; //тут обязательно делать разрушаемой!
        }

        //--- иногда игрок может заспавниться на уровне в замурованной компате (только с бомбой можно из неё выйти, нужно исключить подобное на первом уровне, т.к. у игрока точно нет бомбы)
        if (levelNumber == 0)
        {
            var breakableObjects = FindObjectsOfType<BreakableObject>();
            foreach (var obj in breakableObjects)
            {
                Destroy(obj.gameObject);
            }
        }
        #endregion

        currentLevel.transform.Rotate(new Vector3(0, 0, 90 * Random.Range(0, 4) ));

        #region Спавны | Заполнение комнат

        //получаем все позиции спавнов
        List<GameObject> spawns = new List<GameObject>();
        for (int i = 0; i < currentLevel.transform.Find("Spawns").childCount; i++)
        {
            spawns.Add( currentLevel.transform.Find("Spawns").GetChild(i).gameObject );
        }

        //спавним выход с уровня
        rand = Random.Range(0, spawns.Count);
        currentSpawnables.Add(Instantiate(finishLevelPrefab, spawns[rand].transform.position, Quaternion.identity));
        spawns.RemoveAt(rand);

        //спавн игрока
        rand = Random.Range(0, spawns.Count);
        currentSpawnables.Add( Instantiate(playerPrefab, spawns[rand].transform.position, Quaternion.identity) );
        spawns.RemoveAt(rand);

        //спавним ключи
        for (int i = 0; i < 2; i++) //всегда 2 ключа
        {
            rand = Random.Range(0, spawns.Count);
            currentSpawnables.Add(Instantiate(keyPrefab, spawns[rand].transform.position, Quaternion.identity));
            spawns.RemoveAt(rand);
        }

        //гарантированный спавн одной бомбы
        rand = Random.Range(0, spawns.Count);
        currentSpawnables.Add(Instantiate(bombPrefab, spawns[rand].transform.position, Quaternion.identity));
        spawns.RemoveAt(rand);

        //гарантированный спавн одного врага
        rand = Random.Range(0, spawns.Count);
        currentSpawnables.Add(Instantiate(ghostPrefab, spawns[rand].transform.position, Quaternion.identity));
        spawns.RemoveAt(rand);

        //--- в последние позиции рандомно спвним врагов и бомб
        for (int i = 0; i < spawns.Count; i++)
        {
            rand = Random.Range(0, spawns.Count);
            if (Random.Range(0, 2) == 1) currentSpawnables.Add(Instantiate(bombPrefab, spawns[rand].transform.position, Quaternion.identity));
            else currentSpawnables.Add(Instantiate(ghostPrefab, spawns[rand].transform.position, Quaternion.identity));
            spawns.RemoveAt(rand);
        }



        #endregion

    }

    public void UpdateUIStats()
    {
        UI_StatText.text = "<b>Level:</b> " + (levelNumber + 1) + $"   <b>Speed</b>: {Time.timeScale:F2}";
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        UI_DeathText.text = $"YOU DIED ON\n <b><color=yellow> LEVEL {levelNumber + 1} </color></b> \nPRESS <color=red> SPACE </color> TO RESTART";
        UI_DeathText.transform.parent.gameObject.SetActive(true);
        //обновляем и отображаем лучший результат
        int bestResult = PlayerPrefs.GetInt("BestResult", 0);
        if (levelNumber + 1 > bestResult) bestResult = levelNumber + 1;
        PlayerPrefs.SetInt("BestResult", bestResult);
        UI_BestResult.text = $"<b><color=yellow>YOUR BEST RESULT:</color> LEVEL {bestResult}</b>";

        //game over music
        AudioSource.PlayClipAtPoint(gameOverMusic, Camera.main.transform.position);
        gameObject.GetComponent<AudioSource>().Stop();
    }

    public int GetLevel()
    {
        return levelNumber;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.timeScale == 0f)
        {
            Time.timeScale = 1.0f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

}
