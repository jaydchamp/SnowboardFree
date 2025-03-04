using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CurrentGameState { MainMenu, Paused, Playing, DEAD}

public class GameManager : MonoBehaviour
{
    CurrentGameState gameState;
    public static GameManager Instance
    {
        get;
        private set;
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (Instance != null)
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Start()
    {
        UIManager.Instance.onGameBegin += () =>
        {
            Debug.Log("Game Begun Event");
            gameState = CurrentGameState.Playing;
        };

        UIManager.Instance.onPlayerDied += () =>
        {
            Debug.Log("Game Ended Event");
            gameState = CurrentGameState.DEAD;
        };

        UIManager.Instance.onOpenPauseMenu += () =>
        {
            Debug.Log("Pause Menu Opened Event");
            gameState = CurrentGameState.Paused;
        };

        UIManager.Instance.onClosePauseMenu += () =>
        {
            Debug.Log("Pause Menu Closed Event");
            gameState = CurrentGameState.Playing;
        };

        UIManager.Instance.onMainMenu += () => 
        {
            Debug.Log("Main Menu Opened Event");
            gameState = CurrentGameState.MainMenu;
        };

        //UIManager.Instance.offMainMenu += () => 
        //{
           /// gameState = CurrentGameState.Playing
        //};

    }

    void Update()
    {
        if (gameState == CurrentGameState.MainMenu)
        {
            UIManager.Instance.handleMainMenuUpdate();
        }
        else if (gameState == CurrentGameState.Paused)
        {
            UIManager.Instance.handlePausedUpdate();
        }
        else if (gameState == CurrentGameState.Playing)
        {
            UIManager.Instance.handlePlayingUpdate();
        }
        else if(gameState == CurrentGameState.DEAD)
        {
            UIManager.Instance.handleDeadUpdate();
        }
    }
}
