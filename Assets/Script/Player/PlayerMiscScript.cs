using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMiscScript : MonoBehaviour
{
    [Header("Input Action Reference")]
    public InputActionReference pauseAction;

    [Header("Pause")]
    public GameObject pauseObject;
    public bool paused;

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
        paused = !paused;

        if (paused)
        {
            pauseObject.SetActive(true);
            Time.timeScale = 0f;
        }
        else if (!paused)
        {
            pauseObject.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
