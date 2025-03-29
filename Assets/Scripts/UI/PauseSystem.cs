// Drew Phelps
// This script handles entire level UI
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class PauseSystem : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject gameOverMenu;
    [SerializeField] GameObject levelCompleteMenu;
    [SerializeField] GameObject pauseActive;

    //[SerializeField] GameObject live1;
    //[SerializeField] GameObject live2;
    //[SerializeField] GameObject live3;

    //[SerializeField] Text levelTimer;
    //[SerializeField] GameObject timerObject;

    //[SerializeField] Text scoreText;
    //[SerializeField] Text timeText;

    //private PlayerStats playerStats;
    //private LBoard lboard;

    private bool isPaused = false;
    private bool gameOverFirst = true;
    public float elapsedTime;

    public GameObject failureObject;
    public GameObject successObject;

    public AudioManager am;

    void Start()
    {
        GameObject player = GameObject.Find("Player");
        //failureObject = GameObject.Find("Cube");
        //successObject = GameObject.Find("Cube (1)");
    }

    public void PauseOrPlay()
    {
        if (isPaused)
        {
            pauseMenu.SetActive(false); //continue
            Time.timeScale = 1;
            isPaused = false;

        }
        else if (!isPaused)
        {
            pauseMenu.SetActive(true); //pause
            Time.timeScale = 0;
            isPaused = true;
        }
    }

    void Update() 
    {
        //Debug.Log("Current Game Over Status:" + playerStats.getIsDead());
        //Debug.Log("beans is cool");
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Debug.Log("Escape");
            am.onMouseClick();
            PauseOrPlay();
        }
        if (failureObject.activeInHierarchy)
        {
            //gameFailed = true;
            GameOverScreen();
        }

        if (successObject.activeInHierarchy)
        {
            //gameSuccess = true;
            LevelCompleteScreen();
        }

    }

    public void OnPauseButton () 
    {
        //Debug.Log("Escape");
        am.onMouseClick();
        PauseOrPlay();
    }
    public void OnHomeButton () 
    {
        am.onMouseClick();
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }
    public void OnResumeButton ()
    {
        am.onMouseClick();
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }
    public void OnRestartButton ()
    {
        am.onMouseClick();
        Destroy(gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }
    /*public void OnTempTrigger ()
    {
        LevelCompleteScreen();
    }*/
    public void OnNextLevelButton()
    {
        //load next level
        string currentScene = SceneManager.GetActiveScene().name;
        int levelNumber;
        if (int.TryParse(currentScene.Replace("Level", ""), out levelNumber))
        {
            string nextSceneName = "Level" + (levelNumber + 1); // Construct the next level name
            Debug.Log(nextSceneName);
            // Check if next scene exists before loading
            if (Application.CanStreamedLevelBeLoaded(nextSceneName))
            {
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                Debug.Log("Next level does not exist: " + nextSceneName);
            }
        }
        else
        {
            Debug.LogError("Current scene name format is incorrect: " + currentScene);
        }


        Time.timeScale = 1;
    }

    public void GameOverScreen()
    {
        Time.timeScale = 0;
        gameOverMenu.SetActive(true);
        levelCompleteMenu.SetActive(false);
        pauseActive.SetActive(false);
    }

    public void LevelCompleteScreen()
    {
        Time.timeScale = 0;
        gameOverMenu.SetActive(false);
        levelCompleteMenu.SetActive(true);
        pauseActive.SetActive(false);
    }
    /*public void LevelCompleteScreen()
    {
        GameObject[] textOverlays = GameObject.FindGameObjectsWithTag("TextOverlay");
        foreach (GameObject obj in textOverlays)
        {
            obj.SetActive(false);
        }

        string sceneName = SceneManager.GetActiveScene().name;
        int levelNumber = 0;
        int.TryParse(System.Text.RegularExpressions.Regex.Match(sceneName, @"\d+").Value, out levelNumber);

        timerObject.SetActive(false);

        int finalScore = playerStats.getPoints();
        float finalTime = elapsedTime;

        scoreText.text = "Score: " + (playerStats.getPoints());
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        int milliseconds = Mathf.FloorToInt((elapsedTime * 100) % 100);
        timeText.text = string.Format("Time: " + "{0:00}:{1:00}.{2:00}", minutes, seconds, milliseconds);

        //FindFirstObjectByType<LeaderBoardManager>().CheckAndUpdateHighScore(2, 5000);
        //leaderBoardManager.CheckAndUpdateHighScore(levelNumber, playerStats.getPoints());
        int level = 2;//LevelManager.GetCurrentLevelNumber();
        LBoard.Instance.SaveScoreAndTime(level, finalScore, finalTime);
        

        Time.timeScale = 0;
        gameOverMenu.SetActive(false);
        levelCompleteMenu.SetActive(true);
        pauseActive.SetActive(false);
    }*/
}
