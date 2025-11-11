using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuButton : MonoBehaviour
{
    // not include pausing button

    [SerializeField] GameObject pauseCanvas;

    public void ResumeButtonClick()
    {
        // click sound
        pauseCanvas.SetActive(false);
        Time.timeScale = 1.0f;

    }

    public void RetryButtonClick()
    {
        SceneManager.LoadScene("GamePlay");
    }

    public void OptionsButtonClick()
    {
        // click sfx
        // same bgm as current scene
        SceneManager.LoadScene("OptionsMenu", LoadSceneMode.Additive);
    }

    public void MainMenuButtonClick()
    {
        // click sound
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitButtonClick()
    {
        // click sfx
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}
