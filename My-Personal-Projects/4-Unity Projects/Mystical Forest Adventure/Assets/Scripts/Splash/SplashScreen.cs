using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashScreen : MonoBehaviour
{
    public UnityEngine.UI.Slider loadingBar;
    private const string GAMESCENE = "GameScene";
    public GameObject fillArea;

    private void Start()
    {
        StartCoroutine(LoadGame());
    }

    private IEnumerator LoadGame()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(GAMESCENE);

        while (!operation.isDone)
        {
            fillArea.SetActive(true);
            loadingBar.value = operation.progress;
            yield return null;
        }
    }

}
