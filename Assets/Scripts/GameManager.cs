using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] List<AIController> allNPC = new();
    [SerializeField] Player player;
    [SerializeField] GameObject failScreen;
    [SerializeField] GameObject successScreen;
    [SerializeField] List<PickpocketMinigame> tasks = new();
    [SerializeField] Interactable exitInteractable;
    [SerializeField] int cash = 0;
    [SerializeField] TextMeshPro cashDisplay;

    private void Awake()
    {
        failScreen.SetActive(false);
        successScreen.SetActive(false);
        exitInteractable.interactEvent.AddListener(Exit);
        exitInteractable.canInteract = false;
        Time.timeScale = 1;
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
        Debug.Log("Game Over: Failure");
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
        bool tasksDone = true;
        cash = 0;
        foreach (PickpocketMinigame task in tasks)
        {
            tasksDone &= task.success;
            if (task.success) cash += task.cashValue;
        }

        cashDisplay.text = "$" + cash.ToString();

        // end game if all tasks done
        if (tasksDone)
        {
            EnableExit();
        }
    }

    public void EnableExit()
    {
        exitInteractable.canInteract = true;
    }

    public void Exit(bool none)
    {
        GameSuccess();
    }
}
