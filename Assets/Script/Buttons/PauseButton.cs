using UnityEngine;

public class PauseButton : MonoBehaviour
{
    // this is the pause button it self (not pause menu)
    [SerializeField] GameObject pauseCanvas;

    void Start()
    {
        pauseCanvas.SetActive(false);
        Time.timeScale = 1f;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseButtonClick(); 
        }
    }

    public void pauseButtonClick()
    {
        BGMmanager.Instance.PlayerSfxAudio.Pause();
        Cursor.visible = true;
        pauseCanvas.SetActive(true);
        Time.timeScale = 0f;
    }
}
