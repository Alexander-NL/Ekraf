using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainButton : MonoBehaviour
{
    [SerializeField] GameObject optionsCanvas;
    public void PlayButtonClick()
    {
        // click sfx
        // change to gameplay bgm
        SceneManager.LoadScene("GamePlay");
    }

    public void OptionsButtonClick()
    {
        BGMmanager.Instance.SfxOnclick();
        optionsCanvas.SetActive(true);
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
