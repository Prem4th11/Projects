using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region fields
    public static AudioManager Instance;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameAudioClips audioClips;
    #endregion
    #region Mono and methods
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayOnClick() => PlayClip(audioClips.tapCorrect);
    public void PlayTapCorrect() => PlayClip(audioClips.tapCorrect);
    public void PlayTapWrong() => PlayClip(audioClips.tapWrong);
    public void PlayCombo() => PlayClip(audioClips.comboBonus);
    public void PlayPerfect() => PlayClip(audioClips.perfectTiming);
    public void PlayGameOver() => PlayClip(audioClips.gameOver);

    private void PlayClip(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
    #endregion
}
