using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

//////////////////////////////////////////////////////////////////////////////////////////////////////
//Responsible for managing game mechanics for enemy spawning, pickup collection and game progression//
//////////////////////////////////////////////////////////////////////////////////////////////////////
public class GameController : MonoBehaviour
{
    public GameObject[] spawnerLocations;
    public GameObject[] minorCollectableLocations;
    public Enemy[] enemyPrefabs;
    public Collectable[] relics;
    public Collectable potionPrefab;

    public Spawner spawnerPrefab;

    public GameObject[] relicDisplayLocations;

    public Text playerLivesDisplay;
    public Text messageDisplay;

    public GameObject gradeDisplay;
    public GameObject percentDisplay;

    public GameObject victory;
    public GameObject defeat;

    public Player player;

    public AudioSource gameLoop;
    public AudioSource bossMusic;

    public AudioSource victoryMusicPrefab;
    public AudioSource defeatMusicPrefab;

    public Enemy bossPrefab;
    protected Enemy boss;
    bool bossSpawned = false;

    private string[] spawnerlocales = new string[] { "The Great Hall", "The Library", "The Storage Room", "The Bedroom", "The Study"};

    bool spawning = true;
    bool relicSpawned = false;

    int difficultyLevel = 0;
    int waveSize = 3;

    int maxSpawners = 1;
    int currentSpawners = 0;
    int defeatedSpawners = 0;

    int relicCount = 0;
    int totalRelics = 4;

    bool potionSpawned = false;

    int posI = 0;
    int posI2 = 0;

    int correctSolutions = 0;
    int incorrectSolutions = 0;

    bool musicPlayed = false;

    // Use this for initialization
    void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        victory.SetActive(false);
        defeat.SetActive(false);
        gameLoop.Play();
        gameLoop.loop = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu");
        }

        if (player == null)
        {
            //Game Over
            //Debug.Log("GAME OVER");
            playerLivesDisplay.text = "0";
            defeat.SetActive(true);

            bossMusic.Stop();
            gameLoop.Stop();

            if (!musicPlayed)
            {
                Vector3 pos = GameObject.FindGameObjectWithTag("MainCamera").transform.position;
                AudioSource sound = Instantiate(defeatMusicPrefab, pos, Quaternion.identity);
                sound.Play();
                Destroy(sound, 4.0f);
                musicPlayed = true;   
            }

            StartCoroutine(returnToMenu());
        }
        else
        {
            if (relicCount < totalRelics)
            {
                if (!spawning)
                {
                    if (currentSpawners <= 0 && !relicSpawned)
                    {
                        relicSpawned = true;
                        spawnRelic(posI);
                        defeatedSpawners = 0;
                    }
                }
                else
                {
                    if (!potionSpawned)
                    {
                        spawnPotion(posI2);
                    }

                    if (defeatedSpawners >= 4)
                    {
                        spawning = false;
                        if (maxSpawners < 2)
                            maxSpawners++;
                    }

                   

                    if (currentSpawners < maxSpawners)
                    {
                        spawnSpawner(posI);
                    }
                }
            }
            else if (boss == null && bossSpawned)
            {
                //Win
                //Debug.Log("WIN");
                int percent = determineResultPercent();
                string grade = determineGrade(percent);

                bossMusic.Stop();
                gameLoop.Stop();

                if (!musicPlayed)
                {
                    Vector3 pos = GameObject.FindGameObjectWithTag("MainCamera").transform.position;
                    AudioSource sound = Instantiate(victoryMusicPrefab, pos, Quaternion.identity);
                    sound.Play();
                    Destroy(sound, 4.0f);
                    musicPlayed = true;
                }
                gradeDisplay.GetComponent<TextMeshProUGUI>().text = grade;
                percentDisplay.GetComponent<TextMeshProUGUI>().text = percent + "% Correct";
                victory.SetActive(true);

                StartCoroutine(returnToMenu());
            }
            else if (boss == null)
            {
                gameLoop.Stop();
                bossMusic.Play();
                bossMusic.loop = true;

                boss = Instantiate(bossPrefab, new Vector3(0.0f, 1.05f, 0.0f), Quaternion.identity);
                boss.controller = this;
                bossSpawned = true;

                messageDisplay.text = ("The Boss Has Spawned In The Atrium");
            }
            playerLivesDisplay.text = player.getHealth().ToString();
        }
    }

    private void spawnSpawner(int posIndex)
    {
        if (spawning)
        {
            int waveCount = waveSize;
            Wave[] waves = new Wave[waveCount];

            if (difficultyLevel < enemyPrefabs.Length - 1)
            {
                for (int i = 0; i < waveCount - 1; i++)
                {
                    waves[i] = new Wave(enemyPrefabs[difficultyLevel], i + 1, 2.0f);
                }
                waves[waveCount - 1] = new Wave(enemyPrefabs[difficultyLevel + 1], 1, 2.0f);
            }
            else
            {
                for (int i = 0; i < waveCount; i++)
                {
                    waves[i] = new Wave(enemyPrefabs[difficultyLevel], 1, 3.0f);
                }
            }
            Spawner spawnedSpawner = Instantiate(spawnerPrefab, spawnerLocations[posIndex].transform.position, Quaternion.identity) as Spawner;
            spawnedSpawner.construct(waves, this);
            currentSpawners++;

            messageDisplay.text = ("A Portal Has Opened In " + spawnerlocales[posIndex]);

            posI++;
            if (posI >= spawnerLocations.Length)
                posI = 0;
        }
    }

    private void spawnRelic(int posIndex)
    {
        Collectable relic = Instantiate(relics[relicCount], new Vector3(spawnerLocations[posIndex].transform.position.x, spawnerLocations[posIndex].transform.position.y + 0.5f, spawnerLocations[posIndex].transform.position.z), Quaternion.identity) as Collectable;
        relic.contruct(this);

        messageDisplay.text = ("A Relic Has Appeared In " + spawnerlocales[posIndex]);

        posI++;
        if (posI >= spawnerLocations.Length)
            posI = 0;
    }

    private void spawnPotion(int posIndex)
    {
        Collectable potion = Instantiate(potionPrefab, new Vector3(minorCollectableLocations[posIndex].transform.position.x, minorCollectableLocations[posIndex].transform.position.y + 0.5f, minorCollectableLocations[posIndex].transform.position.z), Quaternion.identity) as Collectable;
        potion.contruct(this);

        //messageDisplay.text = ("A Potion Has Spawned In " + potionlocales[posIndex]);

        posI2++;
        if (posI2 >= minorCollectableLocations.Length)
            posI2 = 0;

        potionSpawned = true;
    }

    public void registerDeadSpawner()
    {
        defeatedSpawners++;
        currentSpawners--;
    }

    public void registerRelicPickup()
    {
        Instantiate(relics[relicCount], relicDisplayLocations[relicCount].transform.position, Quaternion.identity);
        difficultyLevel++;
        relicCount++;
        spawning = true;
        relicSpawned = false;
    }

    public void registerPotionPickup()
    {
        //player.addHealth();
        potionSpawned = false;
    }

    public void incrementCorrectSolutions()
    {
        this.correctSolutions++;
        Debug.Log("Correct Solutions: " + correctSolutions + "\nIncorrect Solutions: " + incorrectSolutions);
    }

    public void incrementIncorrectSolutions()
    {
        this.incorrectSolutions++;
        Debug.Log("Correct Solutions: " + correctSolutions + "\nIncorrect Solutions: " + incorrectSolutions);
    }

    int determineResultPercent()
    {
        int percent;
        float totalAnswers = (correctSolutions + incorrectSolutions);
        percent = (int)((correctSolutions / totalAnswers) * 100);
        Debug.Log(percent);
        return percent;
    }

    string determineGrade(int percent)
    {
        if (percent == 100)
            return "A+";
        else if (percent >= 70)
            return "A";
        else if (percent >= 60)
            return "B";
        else if (percent >= 50)
            return "C";
        else
            return "D";
    }

    IEnumerator returnToMenu()
    {
        Debug.Log("Returning To Menue...");

        yield return new WaitForSeconds(6.0f);

        SceneManager.LoadScene("Menu");

        yield return null;
    }
}
