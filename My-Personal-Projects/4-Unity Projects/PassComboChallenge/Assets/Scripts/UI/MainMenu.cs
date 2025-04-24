using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    #region fields | variables
    [SerializeField] private Button easyButton;
    [SerializeField] private Button mediumButton;
    [SerializeField] private Button hardButton;
    [SerializeField] private Button quit;
    [SerializeField] private Button settings;
    public TMPro.TextMeshProUGUI bestScoreText;
    #endregion
    #region Mono and methods
    private void Start()
    {
        int bestScore = PlayerPrefs.GetInt("BestScore", 0);
        bestScoreText.text = "Best Score: " + bestScore;
        easyButton.onClick.AddListener(() => StartGameWithDifficulty("Easy"));
        mediumButton.onClick.AddListener(() => StartGameWithDifficulty("Medium"));
        hardButton.onClick.AddListener(() => StartGameWithDifficulty("Hard"));
        quit.onClick.AddListener(QuitGame);
        settings.onClick.AddListener(OpenSettings);
    }

    private void StartGameWithDifficulty(string difficulty)
    {
        AudioManager.Instance.PlayOnClick();
        PlayerPrefs.SetString("Difficulty", difficulty);
        SceneManager.LoadScene("GameScene");
    }

    public void OpenSettings()
    {
        AudioManager.Instance.PlayOnClick();
        Debug.Log("Todo : Settings Menu Coming Soon!");
    }

    public void QuitGame()
    {
        AudioManager.Instance.PlayOnClick();
        Application.Quit();
        Debug.Log("Game Quit!");
    }
    #endregion
}