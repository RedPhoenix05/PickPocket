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

    [SerializeField] GameObject live1;
    [SerializeField] GameObject live2;
    [SerializeField] GameObject live3;

    [SerializeField] Text levelTimer;
    [SerializeField] GameObject timerObject;

    [SerializeField] Text scoreText;
    [SerializeField] Text timeText;

    //private PlayerStats playerStats;
    //private LBoard lboard;

    private bool isPaused = false;
    private bool gameOverFirst = true;
    public float elapsedTime;

    void Start()
    {
        GameObject player = GameObject.Find("Player");
        //playerStats = player.GetComponent<PlayerStats>();
        //GameObject leaderboard = GameObject.Find("LeaderBoardManager");
        //lboard = leaderboard.GetComponent<LBoard>();
        //Debug.Log("Player Dead Status" + playerStats.getIsDead());
        live1.SetActive(true);
        live2.SetActive(true);
        live3.SetActive(true);
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

    public void TimerUpdate()
    {
        elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        int milliseconds = Mathf.FloorToInt((elapsedTime * 100) % 100);

        levelTimer.text = string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, milliseconds);
    }

    void Update() 
    {
        //Debug.Log("Current Game Over Status:" + playerStats.getIsDead());
        //Debug.Log("beans is cool");
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Debug.Log("Escape");
            PauseOrPlay();
        }
        /*if (playerStats.getIsDead() && gameOverFirst)
        {
            Debug.Log("Game over first");
            GameOverScreen();
            gameOverFirst = false;
        }*/
        if ((isPaused || gameOverMenu || levelCompleteMenu) && Input.GetKeyDown(KeyCode.R))
        {
            OnRestartButton();
        }

        //UpdateLives();
        //TimerUpdate();
        
    }

    /*private void UpdateLives()
    {
        switch (playerStats.getLives())
        {
            case 1:
                live2.SetActive(false);
                break;

            case 2:
                live2.SetActive(true);
                live3.SetActive(false);
                break;
            case 3:
                live2.SetActive(true);
                live3.SetActive(true);
                break;
        }
    }*/

    private void UpdateTimer()
    {

    }

    public void OnPauseButton () 
    {
        //Debug.Log("Escape");
        PauseOrPlay();
    }
    public void OnHomeButton () 
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }
    public void OnResumeButton ()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }
    public void OnRestartButton ()
    {
        Destroy(gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }
    /*public void OnTempTrigger ()
    {
        LevelCompleteScreen();
    }*/
    /*public void OnNextLevelButton()
    {
        //load next level
        string currentScene = SceneManager.GetActiveScene().name;
        int levelNumber;
        if (int.TryParse(currentScene.Replace("level", ""), out levelNumber))
        {
            string nextSceneName = "level" + (levelNumber + 1); // Construct the next level name
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
    }*/

    /*public void GameOverScreen()
    {
        Time.timeScale = 0;
        gameOverMenu.SetActive(true);
        levelCompleteMenu.SetActive(false);
        pauseActive.SetActive(false);
    }*/

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
