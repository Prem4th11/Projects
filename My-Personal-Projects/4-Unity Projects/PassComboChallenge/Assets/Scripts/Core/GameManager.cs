using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    #region fields | variables

    public GameObject[] teammates;
    private int currentTarget;
    private int score = 0;
    private float timer = 30f;

    private string difficulty;
    private float highlightDuration;
    private float highlightStartTime;
    private int correctStreak = 0;
    private const float perfectTimingThreshold = 0.3f; // 300 ms

    public TMPro.TextMeshProUGUI scoreText;
    public TMPro.TextMeshProUGUI timerText;
    [SerializeField] private TMPro.TextMeshProUGUI feedbackText;
    [SerializeField] private Button backButton;

    #endregion

    #region Mono and methods

    private void Start()
    {
        backButton.onClick.AddListener(GoToMainMenu);
        difficulty = PlayerPrefs.GetString("Difficulty", "Medium");

        switch (difficulty)
        {
            case "Easy":
                highlightDuration = 1.5f;
                break;
            case "Medium":
                highlightDuration = 1.0f;
                break;
            case "Hard":
                highlightDuration = 0.8f; //Adjusted from 0.6f for fair gameplay
                break;
        }

        HighlightNextTarget();
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        timerText.text = "Time: " + Mathf.Ceil(timer).ToString() + "s";

        if (timer <= 0)
        {
            EndGame();
        }
    }

    private void HighlightNextTarget()
    {
        foreach (GameObject teammate in teammates)
        {
            teammate.GetComponent<HighlightEffect>().RemoveHighlight();
        }

        currentTarget = Random.Range(0, teammates.Length);
        teammates[currentTarget].GetComponent<HighlightEffect>().Highlight();
        highlightStartTime = Time.time;

        Invoke("DeactivateTargetIfMissed", highlightDuration);
    }

    private void DeactivateTargetIfMissed()
    {
        if (!teammates[currentTarget].GetComponent<HighlightEffect>().IsHighlighted()) return;

        teammates[currentTarget].GetComponent<HighlightEffect>().RemoveHighlight();

        switch (difficulty)
        {
            case "Hard":
                if(score > 0)
                {
                    score--;
                    StartCoroutine(ShowFeedback("Oops! -1", Color.red));
                }
                else
                {
                    StartCoroutine(ShowFeedback("Oops!", Color.red));
                }
                break;
            case "Medium"://No point,no penalty
                break;
            case "Easy"://No penalty at all
                break;
        }

        correctStreak = 0;
        scoreText.text = "Score: " + score.ToString();
        HighlightNextTarget();
    }

    public void CheckInput(GameObject selectedTeammate)
    {
        CancelInvoke("DeactivateTargetIfMissed");
        float reactionTime = Time.time - highlightStartTime;
        if (selectedTeammate == teammates[currentTarget])
        {
            int basePoints = 1;
            int bonusPoints = 0;
            correctStreak++;

            //Combo bonus
            if (difficulty == "Medium" && correctStreak >= 3)
            {
                bonusPoints += 2;
                StartCoroutine(ShowFeedback("Combo x3 : +2", Color.black));
                AudioManager.Instance.PlayCombo();
                correctStreak = 0;
            }
            else if (difficulty == "Hard" && correctStreak >= 5)
            {
                bonusPoints += 3;
                StartCoroutine(ShowFeedback("Combo x5 : +3", Color.black));
                AudioManager.Instance.PlayCombo();
                correctStreak = 0;
            }

            //Perfect timing
            if (reactionTime <= perfectTimingThreshold)
            {
                bonusPoints += 1;
                StartCoroutine(ShowFeedback("Perfect Timing! +1", Color.black));
                AudioManager.Instance.PlayPerfect();
            }

            score += basePoints + bonusPoints;
        }
        else
        {
            if (difficulty != "Easy") correctStreak = 0;

            if (difficulty == "Hard")
            {
                if (score > 0)
                {
                    score--;
                    StartCoroutine(ShowFeedback("Oops! -1", Color.red));
                }
                else
                {
                    StartCoroutine(ShowFeedback("Oops!", Color.red));
                }
                AudioManager.Instance.PlayTapWrong();
            }
        }

        scoreText.text = "Score: " + score.ToString();
        HighlightNextTarget();
    }

    private IEnumerator ShowFeedback(string message, Color color)
    {
        feedbackText.text = message;
        feedbackText.color = color;
        feedbackText.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.8f);

        feedbackText.gameObject.SetActive(false);
    }

    private void EndGame()
    {
        PlayerPrefs.SetInt("FinalScore", score);

        int bestScore = PlayerPrefs.GetInt("BestScore", 0);
        if (score > bestScore)
        {
            PlayerPrefs.SetInt("BestScore", score);
            PlayerPrefs.Save(); //Important for persistent saving on some platforms
        }

        AudioManager.Instance.PlayGameOver();
        SceneManager.LoadScene("GameOverScene");
    }

    private void GoToMainMenu()
    {
        AudioManager.Instance.PlayOnClick();
        SceneManager.LoadScene("MainMenu");
    }

    #endregion
}
