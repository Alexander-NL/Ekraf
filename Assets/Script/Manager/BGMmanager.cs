using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMmanager : MonoBehaviour
{
    private static BGMmanager _instance;
    public static BGMmanager Instance => _instance;

    [Header("Audio Source")]
    public AudioSource BgmAudio;

    public AudioSource SfxAudio;
    public AudioSource EnemySFX;

    [Header("BGM related")]
    public float transitionTime;
    public AudioClip BossBgm;
    public AudioClip MainMenuBgm;
    public AudioClip GameplayBgm;

    [Header("Audio Level")]
    [Range(0f, 1f)]
    public float BgmVolume;
    [Range(0f, 1f)]
    public float SfxVolume;
    [Range(0f, 1f)]
    public float EnemyVolume;

    void OnEnable()
    {
        SceneManager.activeSceneChanged += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnSceneLoaded;
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        BgmAudio.volume = BgmVolume;
        EnemySFX.volume = EnemyVolume;
        SfxAudio.volume = SfxVolume;
    }

    public void ChangeToGameplayBgm()
    {
        StartCoroutine(MixSources(GameplayBgm));
    }

    public void ChangeToBossBgm()
    {
        StartCoroutine(MixSources(BossBgm));
    }

    public void ChangeToMenuBgm()
    {
        StartCoroutine(MixSources(MainMenuBgm));
    }


    IEnumerator MixSources(AudioClip target)
    {
        float originalVolume = BgmAudio.volume;

        for (float t = 0; t < transitionTime; t += Time.deltaTime)
        {
            BgmAudio.volume = Mathf.Lerp(originalVolume, 0, t / transitionTime);
            yield return null;
        }

        BgmAudio.volume = 0;
        BgmAudio.clip = target;
        BgmAudio.Play();

        for (float t = 0; t < transitionTime; t += Time.deltaTime)
        {
            BgmAudio.volume = Mathf.Lerp(0, originalVolume, t / transitionTime);
            yield return null;
        }

        BgmAudio.volume = originalVolume;
    }


    public void BossBattleBGM()
    {
        BgmAudio.resource = BossBgm;
        BgmAudio.Play();
    }

    void OnSceneLoaded(Scene oldScene, Scene newScene)
    {
        if (newScene.name == "Alex - Menu Test")
        {
            BgmAudio.resource = MainMenuBgm;
            BgmAudio.Play();
        }
        else
        {
            BgmAudio.resource = GameplayBgm;
            BgmAudio.Play();
        }
    }
}
