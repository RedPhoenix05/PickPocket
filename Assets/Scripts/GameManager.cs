using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] List<AIController> allNPC = new();
    [SerializeField] Player player;
    [SerializeField] GameObject failScreen;
    [SerializeField] GameObject successScreen;
    [SerializeField] List<PickpocketMinigame> tasks = new();

    private void Awake()
    {
        failScreen.SetActive(false);
        successScreen.SetActive(false);
    }

    public void GameSuccess()
    {
        failScreen.SetActive(true);
        player.playerController.disableMovement = true;
        Time.timeScale = 0f;
        foreach (var task in tasks)
        {
            task.gameObject.SetActive(false);
        }
    }

    public void GameFail()
    {
        failScreen.SetActive(true);
        player.playerController.disableMovement = true;
        Time.timeScale = 0f;
        foreach (var task in tasks)
        {
            task.gameObject.SetActive(false);
        }
    }

    public void WarnAll(float value)
    {
        foreach (AIController npc in allNPC)
        {
            npc.Warn(value);
        }
    }

    public void CheckGameOver()
    {
        Debug.Log("Task completed");
        bool gameover = true;
        foreach (PickpocketMinigame task in tasks)
        {
            gameover &= task.success;
        }

        // end game if all tasks done
        if (gameover)
        {
            GameSuccess();
        }
    }
}
