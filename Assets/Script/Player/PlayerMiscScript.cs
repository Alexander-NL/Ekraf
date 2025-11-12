using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMiscScript : MonoBehaviour
{
    [Header("Input Action Reference")]
    public InputActionReference pauseAction;

    [Header("Pause")]
    public GameObject pauseObject;
    public bool paused;
    public bool isOption;

    private void OnEnable()
    {
        pauseAction.action.Enable();
        pauseAction.action.performed += OnPause;
    }

    private void OnDisable()
    {
        pauseAction.action.performed -= OnPause;
        pauseAction.action.Disable();
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (isOption) return;
        paused = !paused;

        if (paused)
        {
            Debug.Log("Paused");
            BGMmanager.Instance.PlayerSfxAudio.Pause();
            Cursor.visible = true;
            pauseObject.SetActive(true);
            Time.timeScale = 0f;
        }
        else if (!paused)
        {
            Debug.Log("Unpaused");
            BGMmanager.Instance.PlayerSfxAudio.UnPause();
            Cursor.visible = false;
            pauseObject.SetActive(false);
            Time.timeScale = 1.0f;
        }
    }
}
