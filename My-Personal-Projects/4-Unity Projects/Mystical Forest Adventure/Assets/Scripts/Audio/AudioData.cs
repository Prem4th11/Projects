using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewAudioData", menuName = "Audio/Audio Data")]
public class AudioData : ScriptableObject
{
    public List<Audios> audios;
}
[System.Serializable]
public class Audios
{
    public string audioName;
    public AudioClip audioClip;
    public bool isBackgroundMusic;
}