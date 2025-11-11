using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsButton : MonoBehaviour
{
    [SerializeField] Slider BGMSlider;
    [SerializeField] Slider SFXSlider;

    public void loadVolumetoSlider()
    {
        // load each volume to slider
    }

    public void loadSlidertoVolume()
    {
        // load slide value to volume
    }

    public void backButton()
    {
        SceneManager.UnloadSceneAsync("OptionsMenu");
    }
}
