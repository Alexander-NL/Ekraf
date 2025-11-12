using UnityEngine;
using UnityEngine.UI;

public class OptionsButton : MonoBehaviour
{
    public GameObject OptionCanvas;
    public GameObject PauseCanvas;
    [SerializeField] Slider BGMSlider;
    [SerializeField] Slider SFXSlider;

    public PlayerMiscScript PMS;

    public void Start()
    {
        BGMSlider.value = BGMmanager.Instance.BgmVolume;
        SFXSlider.value = BGMmanager.Instance.SfxVolume;
    }

    private void Update()
    {
        BGMmanager.Instance.BgmVolume = BGMSlider.value;
        BGMmanager.Instance.SfxVolume = SFXSlider.value;
    }

    public void backButton()
    {
        PMS.isOption = false;
        BGMmanager.Instance.SfxOnclick();
        PauseCanvas.SetActive(true);
        OptionCanvas.SetActive(false);
    }
}
