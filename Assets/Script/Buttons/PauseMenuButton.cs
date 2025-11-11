using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuButton : MonoBehaviour
{
    [SerializeField] GameObject pauseCanvas;
    [SerializeField] GameObject optionsCanvas;

    public HeroRespawn HR;
    public PlayerMiscScript PMS;

    public void ResumeButtonClick()
    {
        BGMmanager.Instance.SfxOnclick();
        pauseCanvas.SetActive(false);
        PMS.paused = false;
        Time.timeScale = 1.0f;
    }

    public void RetryButtonClick()
    {
        BGMmanager.Instance.SfxOnclick();
        Time.timeScale = 1.0f;
        HR.Retry();
        PMS.paused = false;
        pauseCanvas.SetActive(false);
    }

    public void OptionsButtonClick()
    {
        BGMmanager.Instance.SfxOnclick();
        optionsCanvas.SetActive(true);
        pauseCanvas.SetActive(false);
    }

    public void QuitButtonClick()
    {
        BGMmanager.Instance.SfxOnclick();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}
