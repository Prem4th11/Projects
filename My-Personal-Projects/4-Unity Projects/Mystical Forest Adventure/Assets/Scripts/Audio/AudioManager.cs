using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    #region fields
    public static AudioManager instance;
    [SerializeField] private AudioSource backgroundMusicSource;
    [SerializeField] private AudioSource soundSource;
    [SerializeField] private AudioData audioData;

    private AudioClip currentBgClip; //Track current background music
    #endregion

    #region Mono & Methods
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayBackgroundMusic("basegamebg");
    }

    public void PlayBackgroundMusic(string audioName)
    {
        AudioClip audioClip = audioData.audios.FirstOrDefault(b => b.audioName == audioName && b.isBackgroundMusic)?.audioClip;
        if (audioClip != null)
        {
            if (currentBgClip == audioClip) return; // Avoid restarting the same music

            currentBgClip = audioClip;
            backgroundMusicSource.clip = currentBgClip;
            backgroundMusicSource.loop = true;
            backgroundMusicSource.Play();
        }
        else
        {
            Debug.LogWarning($"Background Music {audioName} not found!");
        }
    }

    public void StopBackgroundMusic()
    {
        Debug.Log("called stop");
        backgroundMusicSource.Stop();
    }

    public void Play(string audioName)
    {
        AudioClip clip = audioData.audios.FirstOrDefault(a => a.audioName == audioName)?.audioClip;
        if (clip != null)
        {
            soundSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"Audio Clip {audioName} not found!");
        }
    }
    #endregion 
}
