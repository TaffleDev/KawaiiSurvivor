using System;
using UnityEngine;



public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioSource menuMusicSource;
    [SerializeField] private AudioSource gameMusicSource;
    public bool isSFXOn { get; private set; }
    public bool isMusicOn { get; private set; }
    
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        SettingsManager.OnMusicStateChanged += MusicStateChangedCallback;
        SettingsManager.OnSFXStateChanged += SFXStateChangedCallback;

    }

    private void OnDestroy()
    {
        SettingsManager.OnMusicStateChanged -= MusicStateChangedCallback;
        SettingsManager.OnSFXStateChanged -= SFXStateChangedCallback;
    }
    public void PlayMenuMusic()
    {
        gameMusicSource.Stop();
        if (!menuMusicSource.isPlaying) menuMusicSource.Play();
    }

    public void PlayGameMusic()
    {
        menuMusicSource.Stop();
        if (!gameMusicSource.isPlaying) gameMusicSource.Play();
    }
    
    
    public void MusicStateChangedCallback(bool musicState)
    {
        isMusicOn = musicState;
        
        menuMusicSource.mute = !isMusicOn;
        gameMusicSource.mute = !isMusicOn;
    }

    private void SFXStateChangedCallback(bool sfxState)
    {
        isSFXOn = sfxState;
    }
}
