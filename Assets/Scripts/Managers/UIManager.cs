using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    private bool menuState = false;

    public Transform pauseMenuUI;
    public Transform speedUI;
    public Transform healthUI;
    public Transform timeUI;
    public Transform endingScoreUI;

    public event Action onGameBegin;
    public event Action onPlayerDied;
    public event Action onMainMenu;
    public event Action onOpenPauseMenu;
    public event Action onClosePauseMenu;

    private Button restartButton;
    private Button quitGameButton;
    private Button startGameButton;

    //temp holders
    private int MainMenu = 0;
    private int GameplayScene = 1;
    private int GameOverScene = 2;

    private TMP_Text endingScoreVal;
    private TMP_Text timeVariable;
    private float timer;

    public static UIManager Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        //delete duplicate instances on restarting
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

        timer = 0;
    }

    public void handleMainMenuUpdate()
    {
        if (startGameButton == null)
        {
            startGameButton = GameObject.Find("StartGame").GetComponent<Button>();
            startGameButton.onClick.AddListener(() => OnButtonClick("StartGame"));
        }
    }
    public void handlePlayingUpdate()
    { 
        if (pauseMenuUI == null)
        {
            pauseMenuUI = GameObject.FindWithTag("PauseMenu").transform;
            //Debug.Log("No Pause Menu");
        }
        if (speedUI == null)
        {
            speedUI = GameObject.FindWithTag("Speed").transform;
            //Debug.Log("No Speed UI");
        }
        if (healthUI == null)
        {
            healthUI = GameObject.FindWithTag("Health").transform;
            //Debug.Log("No health UI");
        }
        if(timeVariable == null)
        {
            timeUI = GameObject.Find("time").transform;
            timeVariable = timeUI.GetComponent<TMP_Text>();
            //Debug.Log("No Timer UI");
        }

        if (!(ScrollingBackground.Instance.getScrollSpeed() <= 0.0f) || !menuState)
        {
            timer += Time.deltaTime;
        }

        timeVariable.text = timer.ToString("F2");
    }

    public void handlePausedUpdate()
    {
        if (restartButton == null)
        {
            restartButton = GameObject.Find("restartButton").GetComponent<Button>();
            restartButton.onClick.AddListener(() => OnButtonClick("restartButton"));
        }
        if (quitGameButton == null)
        {
            quitGameButton = GameObject.Find("quitGameButton").GetComponent<Button>();
            quitGameButton.onClick.AddListener(() => OnButtonClick("quitGameButton"));
        }
    }

    public void handleDeadUpdate()
    {
        if (restartButton == null)
        {
            restartButton = GameObject.Find("restartButton").GetComponent<Button>();
            restartButton.onClick.AddListener(() => OnButtonClick("restartButton"));
        }
        if (endingScoreUI == null)
        {
            endingScoreUI = GameObject.FindWithTag("EndScore").transform;
            endingScoreVal = endingScoreUI.GetComponent<TMP_Text>();
        }

        endingScoreVal.text = timer.ToString("F2");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(MainMenu);
        onMainMenu?.Invoke();
    }
    
    public void RestartANDOpenGame()
    {

    }

    public void BeginGame()
    {
        //Debug.Log("Game Beings");
        SceneManager.LoadScene(GameplayScene);
        onGameBegin?.Invoke();
    }

    public void EndGame()
    {
        //onGameEnd?.Invoke();
        Debug.Log("QUIT GAME CALLED");
        Application.Quit();
    }

    public void PlayerDied()
    {
        SceneManager.LoadScene(GameOverScene);
        onPlayerDied?.Invoke();
    }

    public bool GetMenuState()
    {
        return menuState;
    }

    public void OpenPauseMenu()
    {
        //Debug.Log("Pause Menu Opened");
        menuState = true;
        ObstacleManager.Instance.SetSpawningStatus(false);
        
        speedUI.gameObject.SetActive(false);
        healthUI.gameObject.SetActive(false);
        pauseMenuUI.gameObject.SetActive(true);
        timeUI.gameObject.SetActive(false);
        onOpenPauseMenu?.Invoke();
    }

    public void ClosePauseMenu()
    {
        //Debug.Log("Pause Menu Closed");
        menuState = false;
        ObstacleManager.Instance.SetSpawningStatus(true);

        speedUI.gameObject.SetActive(true);
        healthUI.gameObject.SetActive(true);
        pauseMenuUI.gameObject.SetActive(false);
        timeUI.gameObject.SetActive(true);
        onClosePauseMenu?.Invoke();
    }

    public void Init()
    {
        speedUI = GameObject.Find("Speed").transform;
        healthUI = GameObject.Find("Health").transform;
        pauseMenuUI = GameObject.Find("PauseMenu").transform;
        pauseMenuUI.gameObject.SetActive(false);
    }

    void OnButtonClick(string buttonName)
    {
        switch (buttonName)
        {
            case "restartButton":
                {
                    LoadMainMenu();
                    break;
                }

            case "quitGameButton":
                {
                    EndGame();
                    break;
                }

            case "StartGame":
                {
                    BeginGame();
                    break;
                }
        }
    }
}
