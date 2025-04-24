using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    #region fields | variables
    public TMPro.TextMeshProUGUI finalScoreText;
    public TMPro.TextMeshProUGUI bestScoreText;
    [SerializeField] private Button restart;  
    [SerializeField] private Button goToMainMenu;
    #endregion
    #region Mono and methods
    private void Start()
    {
        int finalScore = PlayerPrefs.GetInt("FinalScore", 0);
        int bestScore = PlayerPrefs.GetInt("BestScore", 0);

        finalScoreText.text = "Final Score: " + finalScore;
        bestScoreText.text = "Best Score: " + bestScore;

        restart.onClick.AddListener(RestartGame);
        goToMainMenu.onClick.AddListener(GoToMainMenu);
    }

    private void RestartGame()
    {
        AudioManager.Instance.PlayOnClick();
        SceneManager.LoadScene("GameScene");
    }

    private void GoToMainMenu()
    {
        AudioManager.Instance.PlayOnClick();
        SceneManager.LoadScene("MainMenu");
    }
    #endregion
}