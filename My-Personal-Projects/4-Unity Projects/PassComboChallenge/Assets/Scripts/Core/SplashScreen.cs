using UnityEngine;
using UnityEngine.SceneManagement;
public class SplashScreen : MonoBehaviour
{
    #region fields
    public float splashDuration = 2f;
    #endregion
    #region Mono and methods
    void Start()
    {
        Invoke("LoadMainMenu", splashDuration);
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); 
    }
    #endregion
}
