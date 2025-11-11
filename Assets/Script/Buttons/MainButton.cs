using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainButton : MonoBehaviour
{
    public void PlayButtonClick()
    {
        // click sfx
        // change to gameplay bgm
        SceneManager.LoadScene("GamePlay");
    }

    public void OptionsButtonClick()
    {
        // click sfx
        // same bgm as current scene
        SceneManager.LoadScene("OptionsMenu", LoadSceneMode.Additive);
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
