using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour
{
    [Header("Reference")]
    public HeroRespawn heroRespawn;
    public SceneChange sceneChange;
    public CutsceneManager cutscene;
    public GameObject cutsceneCanvas;
    public ShowDeath show;

    [Header("Events")]
    public UnityEvent BossDone;
    public UnityEvent NextLevel;

    public void Start()
    {
        InitializeBossDone();
        InitializeNextLevel();
    }

    private void InitializeNextLevel()
    {
        
    }

    private void InitializeBossDone()
    {
        SaveSystem.Instance.NextScene(heroRespawn.DeadCounter);

        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "BossRoom")
        {
            BossDone.AddListener(BGMmanager.Instance.BossBattleDone);
            BossDone.AddListener(Done);
            BossDone.AddListener(cutscene.StartCutscene);
            BossDone.AddListener(show.UpdateText);
        }
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
                sceneChange.ChangeSceneWithFade("Chamber3");
                break;
            case "Chamber3":
                sceneChange.ChangeSceneWithFade("Chamber4");
                break;
            case "Chamber4":
                sceneChange.ChangeSceneWithFade("BossRoom");
                break;
        }

        NextLevel.Invoke();
    }

    public void Done()
    {
        SaveSystem.Instance.CheckAndSaveDeathTotal(SaveSystem.Instance.Death);
        cutsceneCanvas.SetActive(true);
    }
}
