using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Tabsil.Sijil;
using UnityEngine.Networking;

public class SettingsManager : MonoBehaviour, IWantToBeSaved
{
    [Header("Elements")]
    [SerializeField] private Button sfxButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button privacyButton;
    [SerializeField] private Button askButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private GameObject creditsPanel;
    
    
    [Header("Settings")]
    [SerializeField] private Color onColor;
    [SerializeField] private Color offColor;

    [Header("Data")]
    private bool sfxState;
    private bool musicState;

    [Header("Actions")]
    public static Action<bool> OnSFXStateChanged;
    public static Action<bool> OnMusicStateChanged;
    private void Awake()
    {
        sfxButton.onClick.RemoveAllListeners();
        sfxButton.onClick.AddListener(SfxButtonCallback);
        
        musicButton.onClick.RemoveAllListeners();
        musicButton.onClick.AddListener(MusicButtonCallback);
        
        privacyButton.onClick.RemoveAllListeners();
        privacyButton.onClick.AddListener(PrivacyPolicyButtonCallback);
        
        askButton.onClick.RemoveAllListeners();
        askButton.onClick.AddListener(AskButtonCallback);
        
        creditsButton.onClick.RemoveAllListeners();
        creditsButton.onClick.AddListener(CreditsButtonCallback);


        OnMusicStateChanged += AudioManager.instance.MusicStateChangedCallback;
    }

    private void OnDestroy()
    {
        OnMusicStateChanged -= AudioManager.instance.MusicStateChangedCallback;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HideCreditsPanel();
        OnSFXStateChanged?.Invoke(sfxState);
        OnMusicStateChanged?.Invoke(musicState);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateSfxVisuals()
    {
        if (sfxState)
        {
            sfxButton.image.color = onColor;
            sfxButton.GetComponentInChildren<TextMeshProUGUI>().text = "ON";
        }
        else
        {
            sfxButton.image.color = offColor;
            sfxButton.GetComponentInChildren<TextMeshProUGUI>().text = "OFF";
        }
    }

    private void UpdateMusicVisuals()
    {
        if (musicState)
        {
            musicButton.image.color = onColor;
            musicButton.GetComponentInChildren<TextMeshProUGUI>().text = "ON";
        }
        else
        {
            musicButton.image.color = offColor;
            musicButton.GetComponentInChildren<TextMeshProUGUI>().text = "OFF";
        }
    }

    private void PrivacyPolicyButtonCallback()
    {
        Debug.Log("You got the privacy policy");
        Application.OpenURL("https://www.youtube.com/watch?v=dQw4w9WgXcQ");
    }
    
    private void SfxButtonCallback()
    {
        sfxState = !sfxState;

        UpdateSfxVisuals();
        Save();
        // Trigger an Action with the new sfx state
        OnSFXStateChanged?.Invoke(sfxState);
        
    }

    private void MusicButtonCallback()
    {
        musicState = !musicState;

        UpdateMusicVisuals();
        Save();
        
        // Trigger an Action with the new audioManager state
        OnMusicStateChanged?.Invoke(musicState);
    }
    
    private void CreditsButtonCallback()
    {
        creditsPanel.SetActive(true);
    }

    public void HideCreditsPanel()
    {
        creditsPanel.SetActive(false);
    }

    private void AskButtonCallback()
    {
        string email = "Seidelin2@hotmail.com";
        string subject = MyEscapeURL("Help");
        string body = MyEscapeURL("Hey! I need help with this ...");
        
        Application.OpenURL("mailto:" + email + "?subject=" + subject +  "&body=" + body);
    }

    private string MyEscapeURL(string s)
    {
        //It doesn't like to have spaces in URL so this replaces it.
        // %20 is hex number for space
        return UnityWebRequest.EscapeURL(s).Replace("+", "%20");
    }

    public void Load()
    {

        sfxState = true;
        musicState = true;

        if (Sijil.TryLoad(this, "sfx", out object sfxStateObject))
            sfxState = (bool)sfxStateObject;
        
        if (Sijil.TryLoad(this, "music", out object musicStateObject))
            musicState = (bool)musicStateObject;
        
        UpdateSfxVisuals();
        UpdateMusicVisuals();
    }

    public void Save()
    {
        Sijil.Save(this, "sfx", sfxState);
        Sijil.Save(this, "music", musicState);
        
    }
}
