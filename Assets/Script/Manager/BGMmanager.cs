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
    public AudioSource SfxAudioExtra;

    public AudioSource PlayerSfxAudio;
    public AudioSource EnemySFX;
    public AudioSource EnemySFXShoot;

    [Header("BGM related")]
    public float transitionTime;
    public AudioClip BossBgm;
    public AudioClip MainMenuBgm;
    public AudioClip GameplayBgm;
    public AudioClip Ambience;

    [Header("UI related")]
    public AudioClip ClickSound;

    [Header("Hero related")]
    public AudioClip HeroDead;
    public AudioClip HeroWalk;
    public AudioClip HeroRun;
    public AudioClip HeroSlapped;
    public AudioClip HeroJump;
    public AudioClip HeroAlert;

    [Header("Turret")]
    public AudioClip Shoot;
    public AudioClip HitWall;
    public AudioClip TurretExplosion;

    [Header("Boss")]
    public AudioClip Explosion;
    public AudioClip FinalExplosion;

    [Header("Player related")]
    public AudioClip parry;
    public AudioClip slap;

    [Header("Audio Level")]
    [Range(0f, 1f)]
    public float BgmVolume;
    [Range(0f, 1f)]
    public float SfxVolume;

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

        EnemySFX.volume = SfxVolume;
        SfxAudio.volume = SfxVolume;
        PlayerSfxAudio.volume = SfxVolume;
        SfxAudioExtra.volume = SfxVolume;
        EnemySFXShoot.volume = SfxVolume;
    }

    public void SfxOnclick()
    {
        SfxAudio.clip = ClickSound;
        SfxAudio.Play();
    }


    /// <summary>
    /// Either Slap for slapping sound or Parry for parry sound
    /// </summary>
    /// <param name="temp"></param>
    public void PlayerSlap(string temp)
    {
        switch (temp)
        {
            case "Slap":
                SfxAudio.clip = parry;
                SfxAudio.Play();
                break;
            case "Parry":
                SfxAudio.clip = slap;
                SfxAudio.Play();
                break;
        }
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

    public void ArrowHitWall()
    {
        EnemySFX.clip = HitWall;
        EnemySFX.Play();
    }

    public void ArrowShoot()
    {
        EnemySFXShoot.clip = Shoot;
        EnemySFXShoot.Play();
    }

    public void BossExplosion()
    {
        StartCoroutine(BossExplosionDelay());
    }

    IEnumerator BossExplosionDelay()
    {
        SfxAudioExtra.clip = Explosion;
        SfxAudioExtra.Play();

        yield return new WaitForSeconds(Explosion.length);

        EnemySFX.clip = FinalExplosion;
        EnemySFX.Play();
    }

    public void TurretDead()
    {
        EnemySFX.clip = TurretExplosion;
        EnemySFX.Play();
    }

    /// <summary>
    /// Walk, Jump, Dead, Slap, Run (Uses playerSFXaudio and SfxAudioExtra)
    /// </summary>
    /// <param name="temp"></param>
    public void PlayerSfxSet(string temp)
    {
        switch(temp)
        {
            case "Walk":
                PlayerSfxAudio.loop = true;
                PlayerSfxAudio.clip = HeroWalk;
                PlayerSfxAudio.Play();
                break;
            case "Jump":
                PlayerSfxAudio.loop = false;
                PlayerSfxAudio.clip = HeroJump;
                PlayerSfxAudio.Play();
                break;
            case "Dead":
                PlayerSfxAudio.loop = false;
                PlayerSfxAudio.clip = HeroDead;
                PlayerSfxAudio.Play();
                break;
            case "Run":
                PlayerSfxAudio.loop = true;
                PlayerSfxAudio.clip = HeroRun;
                PlayerSfxAudio.Play();
                break;
            case "Slap":
                SfxAudioExtra.loop = false;
                SfxAudioExtra.clip = HeroSlapped;
                SfxAudioExtra.Play();
                break;
            case "Alert":
                SfxAudioExtra.loop = false;
                SfxAudioExtra.clip = HeroAlert;
                SfxAudioExtra.Play();
                break;
        }
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


    public void BossBattleDone()
    {
        BgmAudio.resource = Ambience;
        PlayerSfxAudio.Pause();
        BgmAudio.Play();
    }


    void OnSceneLoaded(Scene oldScene, Scene newScene)
    {
        if (newScene.name == "Tutorial")
        {
            BgmAudio.resource = Ambience;
            BgmAudio.Play();
        }
        else if(newScene.name == "MainMenu")
        {
            BgmAudio.resource = MainMenuBgm;
            BgmAudio.Play();
        }
        else if(newScene.name == "BossRoom")
        {
            BgmAudio.resource = BossBgm;
            BgmAudio.Play();
        }
        else
        {
            BgmAudio.resource = GameplayBgm;
            BgmAudio.Play();
        }
    }
}
