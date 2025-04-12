using System.Collections;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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
}

public interface IGameStateListener
{
    void GameStateChangedCallBack(GameState gameState);
}
