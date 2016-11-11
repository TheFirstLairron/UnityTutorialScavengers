using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public float levelStartDelay = 2f;
    public float turnDelay = 0.1f;
    public static GameManager instance = null;
    public BoardManager board;
    public int level = 1;
    public int basePlayerFood = 100;
    public int playerFoodPoints = 100;

    [HideInInspector]
    public bool playersTurn = true;

    private Text levelText;
    private GameObject levelImage;
    private List<Enemy> enemies;
    private bool enemiesMoving;
    private bool doingSetup;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        enemies = new List<Enemy>();
        board = GetComponent<BoardManager>();
    }

    private void Update()
    {
        // Only move if not already moving
        if(!playersTurn && !enemiesMoving && !doingSetup)
        {
            StartCoroutine(MoveEnemies());
        }
    }

    private void InitGame()
    {
        doingSetup = true;

        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Day " + level;
        levelImage.SetActive(true);
        Invoke("HideLevelImage", levelStartDelay);

        enemies.Clear();
        board.SetupScene(level);
    }

    private void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }

    public void GameOver()
    {
        levelText.text = "After " + level + " days, you have starved.";
        levelImage.SetActive(true);
        level = 0;
    }

    public void AddEnemyToList(Enemy script)
    {
        enemies.Add(script);
    }

    private IEnumerator MoveEnemies()
    {
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);

        if (enemies.Count == 0)
        {
            yield return new WaitForSeconds(turnDelay);
        }

        foreach(Enemy enemy in enemies)
        {
            enemy.MoveEnemy();
            yield return new WaitForSeconds(enemy.moveTime);
        }

        playersTurn = true;
        enemiesMoving = false;
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        level++;
        InitGame();
    }

    private void OnEnable()
    {
        // Setup the OnLevelFinishedLoading function to listen for a scene loaded event
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnDisable()
    {
        // Stop the event from firing the OnLevelFinishedLoading function
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }
}
