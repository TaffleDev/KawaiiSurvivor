using System.Collections;
using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Action")]
    public static Action onGamePaused;
    public static Action onGameResumed;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Application.targetFrameRate = 60;
        SetGameState(GameState.MENU);
    }

    public void StartGame()            => SetGameState(GameState.GAME);
    public void StartWeaponSelection() => SetGameState(GameState.WEAPONSELECTION);
    public void StartShop()            => SetGameState(GameState.SHOP);

    public void SetGameState(GameState gameState)
    {
        IEnumerable<IGameStateListener> gameStateListeners = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IGameStateListener>();

        foreach ( IGameStateListener gameStateListener in gameStateListeners)
            gameStateListener.GameStateChangedCallBack(gameState);


        
    }

    public void WaveCompletedCallback()
    {
        if (Player.instance.HasLeveledUp() || WaveTransitionManager.instance.HasCollectedChests())
        {
            SetGameState(GameState.WAVETRANSITION);
        }
        else
        {
            SetGameState(GameState.SHOP);
        }
    }

    public void ManageGameover()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PauseButtonCallBack()
    {
        Time.timeScale = 0;
        onGamePaused?.Invoke();
    }

    public void ResumeButtonCallBack()
    {
        Time.timeScale = 1;
        onGameResumed?.Invoke();
    }

    public void RestartFromPause()
    {
        Time.timeScale = 1;
        ManageGameover();
    }
    
}

public interface IGameStateListener
{
    void GameStateChangedCallBack(GameState gameState);
}
