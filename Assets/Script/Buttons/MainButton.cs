using UnityEngine;
using UnityEngine.SceneManagement;

public class MainButton : MonoBehaviour
{
    [SerializeField] GameObject optionsCanvas;

    public void Start()
    {
        Cursor.visible = true;
    }
    public void PlayButtonClick()
    {
        // click sfx
        // change to gameplay bgm
        SceneManager.LoadScene("Tutorial");
    }

    public void OptionsButtonClick()
    {
        BGMmanager.Instance.SfxOnclick();
        optionsCanvas.SetActive(true);
    }

    public void ButtonBack()
    {
        BGMmanager.Instance.SfxOnclick();
        optionsCanvas.SetActive(false);
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
