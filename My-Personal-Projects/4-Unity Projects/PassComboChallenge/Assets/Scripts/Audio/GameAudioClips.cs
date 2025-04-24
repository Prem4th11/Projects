using UnityEngine;

[CreateAssetMenu(fileName = "GameAudioClips", menuName = "Audio/Game Audio Clips")]
public class GameAudioClips : ScriptableObject
{
    public AudioClip tapCorrect;
    public AudioClip tapWrong;
    public AudioClip comboBonus;
    public AudioClip perfectTiming;
    public AudioClip gameOver;
}
