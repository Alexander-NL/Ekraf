using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour
{
    [Header("Reference")]
    public HeroRespawn heroRespawn;
    public SceneChange sceneChange;

    [Header("Events")]
    public UnityEvent BossBattle;
    public UnityEvent CutsceneStart;
    public UnityEvent CutsceneEnd;
    public UnityEvent GameplayStart;
    public UnityEvent GameplayEnd;
    public UnityEvent NextLevel;

    public void Start()
    {
        InitializeBossBattle();
        InitializeNextLevel();
    }

    private void InitializeNextLevel()
    {
        
    }

    private void InitializeBossBattle()
    {
        BossBattle.AddListener(BGMmanager.Instance.ChangeToBossBgm);
    }

    public void InvokeNextLevel()
    {
        SaveSystem.Instance.NextScene(heroRespawn.DeadCounter);
        Scene currentScene = SceneManager.GetActiveScene();

        switch (currentScene.name)
        {
            case "Tutorial":
                sceneChange.ChangeSceneWithFade("Chamber1");
                break;
            case "Chamber1":
                sceneChange.ChangeSceneWithFade("Chamber2");
                break;
            case "Chamber2":
                sceneChange.ChangeSceneWithFade("BossRoom");
                break;
        }

        NextLevel.Invoke();
    }

    void Update()
    {
        
    }
}
