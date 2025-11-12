using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    [Header("")]
    [SerializeField] CanvasGroup[] CutsceneList;
    [SerializeField] CanvasGroup fadeImage;
    [SerializeField] bool isComic;
    public int temp;
    public float fadeDuration = 1f;

    [Header("Input Action Reference")]
    public InputActionReference NextAction;

    private void OnEnable()
    {
        NextAction.action.Enable();
        NextAction.action.performed += NextPart;
    }

    private void OnDisable()
    {
        NextAction.action.performed -= NextPart;
        NextAction.action.Disable();
    }

    private void Start()
    {
        //  ...
        StartCutscene();  
    }

    public void StartCutscene()
    {
        Time.timeScale = 0f;
        foreach(var cutscene in CutsceneList)
        {
            cutscene.alpha = 0f;
        }
        StartCoroutine(FadeOutTransition(fadeImage));

        StartCoroutine(FadeInTransition(CutsceneList[0]));
    }

    private void NextPart(InputAction.CallbackContext context)
    {
        temp++;
        UpdateCutscene(temp);
    }

    void UpdateCutscene(int temp)
    {
        if (temp >= CutsceneList.Length)
        {
            //End
            if(isComic == false)
            {
                StartCoroutine(FadeOutTransition(CutsceneList[temp - 1]));
                SceneManager.LoadScene("MainMenu");
            }
            StartCoroutine(FadeInTransition(fadeImage));
            Time.timeScale = 1f;
            this.gameObject.SetActive(false);
        }
        else
        {
            StartCoroutine(FadeInTransition(CutsceneList[temp]));
            if(isComic == false)
            {
                StartCoroutine(FadeOutTransition(CutsceneList[temp-1]));
            }
        }
    }

    IEnumerator FadeInTransition(CanvasGroup fadeCanvasGroup)
    {
        if (fadeCanvasGroup == null) yield break;

        //yield return new WaitForSeconds(1f);

        //fadeCanvasGroup.gameObject.SetActive(true);
        fadeCanvasGroup.alpha = 0f;

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            fadeCanvasGroup.alpha = Mathf.Clamp01(timer / fadeDuration);
            yield return null;
        }

        fadeCanvasGroup.alpha = 1f;
        //fadeCanvasGroup.gameObject.SetActive(true);
    }
    IEnumerator FadeOutTransition(CanvasGroup fadeCanvasGroup)
    {
        if (fadeCanvasGroup == null) yield break;

        //fadeCanvasGroup.gameObject.SetActive(true);
        fadeCanvasGroup.alpha = 1f;

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            fadeCanvasGroup.alpha = 1f - Mathf.Clamp01(timer / fadeDuration);
            yield return null;
        }

        fadeCanvasGroup.alpha = 0f;
        //fadeCanvasGroup.gameObject.SetActive(false);
    }

}


