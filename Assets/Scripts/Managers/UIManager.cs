using System;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour, IGameStateListener
{

    [Header(" Panels")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject weaponSelectionPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject gameoverPanel;
    [SerializeField] private GameObject stageCompletePanel;
    [SerializeField] private GameObject waveTransitionPanel;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject pausePanel;
    
    [SerializeField] private GameObject restartConfirmationPanel;
    [SerializeField] private GameObject characterSelectionPanel;
    [SerializeField] private GameObject settingsPanel;
    
    private List<GameObject> panels = new List<GameObject>();

    [Header("Actions")] 
    public static Action<Panel> OnPanelShown;

    private void Awake()
    {
        panels.AddRange(new GameObject[]
        {
            menuPanel,
            weaponSelectionPanel,
            gamePanel,
            gameoverPanel,
            stageCompletePanel,
            waveTransitionPanel,
            shopPanel,

        });

        GameManager.onGamePaused  += GamePausedCallBack;
        GameManager.onGameResumed += GameResumedCallback;
        
        pausePanel.SetActive(false);
        HideRestartConfirmationPanel();
        HideCharacterSelectionPanel();
        HideSettings();
    }

    private void OnDestroy()
    {
        GameManager.onGamePaused  -= GamePausedCallBack;
        GameManager.onGameResumed -= GameResumedCallback;
    }

    public void GameStateChangedCallBack(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.MENU:
                ShowPanel(menuPanel);
                AudioManager.instance.PlayMenuMusic();
                break;

            case GameState.WEAPONSELECTION:
                ShowPanel(weaponSelectionPanel);
                break;

            case GameState.GAME:
                ShowPanel(gamePanel);
                AudioManager.instance.PlayGameMusic();
                break;

            case GameState.GAMEOVER:
                ShowPanel(gameoverPanel);
                break;

            case GameState.STAGECOMPLETE:
                ShowPanel(stageCompletePanel);
                break;

            case GameState.WAVETRANSITION:
                ShowPanel(waveTransitionPanel);
                break;

            case GameState.SHOP:
                ShowPanel(shopPanel);
                break;

            
        }
    }

    private void ShowPanel(GameObject panel)
    {
        foreach ( GameObject p in panels)
        {
            //This logic is the same as below
            p.SetActive(p == panel);

            if (p == panel)
            {
                TryTriggerPanelShownAction(p);
            }
            
            /*
            if (p == panel)
                p.SetActive(true);
            else
                p.SetActive(false);
            */
        }
    }
    
    
    private void GamePausedCallBack()
    {
        pausePanel.SetActive(true);
        
        TryTriggerPanelShownAction(pausePanel);

        SetPanelInteractAbility(gamePanel, false);
    }

    private void GameResumedCallback()
    { 
        pausePanel.SetActive(false);
        SetPanelInteractAbility(gamePanel, true);

    }
    public void ShowRestartConfirmationPanel()
    {
        restartConfirmationPanel.SetActive(true);
        TryTriggerPanelShownAction(restartConfirmationPanel);
        SetPanelInteractAbility(gamePanel, false);
    }

    public void HideRestartConfirmationPanel()
    {
        restartConfirmationPanel.SetActive(false);
        
        TryTriggerPanelShownAction(pausePanel);
        SetPanelInteractAbility(gamePanel, true);

    }

    public void ShowCharacterSelectionPanel()
    {
        characterSelectionPanel.SetActive(true);
        TryTriggerPanelShownAction(characterSelectionPanel);
        menuPanel.SetActive(false);

    } 
    public void HideCharacterSelectionPanel()
    {
        characterSelectionPanel.SetActive(false);
        menuPanel.SetActive(true);
        TryTriggerPanelShownAction(menuPanel);
    }

    public void ShowSettings()
    {
        settingsPanel.SetActive(true);
        TryTriggerPanelShownAction(settingsPanel);
        menuPanel.SetActive(false);
    }

    public void HideSettings()
    {
        settingsPanel.SetActive(false);
        menuPanel.SetActive(true);
        TryTriggerPanelShownAction(menuPanel);
    }


    private void TryTriggerPanelShownAction(GameObject panelObject)
    {
        if (panelObject.TryGetComponent(out Panel panelComponent))
            OnPanelShown?.Invoke(panelComponent);
    }

    public static void SetPanelInteractAbility(GameObject panelObject, bool interactable)
    {
        if (panelObject.TryGetComponent(out CanvasGroup cg))
            cg.interactable = interactable;
    }
}
