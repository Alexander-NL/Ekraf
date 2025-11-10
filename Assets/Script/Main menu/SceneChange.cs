using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneChange : MonoBehaviour
{
    [Header("Normal Fade Settings")]
    public GameObject TransitionImage;
    public Image fadeImage;
    public float fadeDuration = 1f;
    public bool FadeIn = true;

    public void Start()
    {
        if (FadeIn)
        {
            TransitionImage.SetActive(true);
            StartCoroutine(FadeInTransition());
        }
        else
        {
            TransitionImage.SetActive(false);
        }
    }


    /// <summary>
    /// Exit game fade out
    /// </summary>
    public void ExitGame()
    {
        TransitionImage.SetActive(true);
        Time.timeScale = 1f;
        StartCoroutine(FadeAndExit());
    }


    /// <summary>
    /// Used for normal fade out transition
    /// </summary>
    /// <param name="sceneName"></param>
    public void ChangeSceneWithFade(string sceneName)
    {
        Debug.Log("IT WORKS?");
        TransitionImage.SetActive(true);
        Time.timeScale = 1f;
        StartCoroutine(FadeAndLoadScene(sceneName));
    }


    /// <summary>
    /// Same with ChangeSceneWithFade but this time its white BG
    /// </summary>
    /// <param name="sceneName"></param>
    public void EndSceneWithFade(string sceneName)
    {
        TransitionImage.SetActive(true);
        Time.timeScale = 1f;
        StartCoroutine(FadeToWhiteLoadScene(sceneName));
    }

    IEnumerator FadeToWhiteLoadScene(string sceneName)
    {
        // Fade to black
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Clamp01(timer / fadeDuration);
            fadeImage.color = new Color(1f, 1f, 1f, alpha);
            yield return null;
        }

        // Load the new scene
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator FadeInTransition()
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = 1 - Mathf.Clamp01(timer / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        TransitionImage.SetActive(false);
    }

    IEnumerator FadeAndLoadScene(string sceneName)
    {
        // Fade to black
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Clamp01(timer / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        // Load the new scene
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator FadeAndExit()
    {
        // Fade to black
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Clamp01(timer / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        Application.Quit();
    }
}