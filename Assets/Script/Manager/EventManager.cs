using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public UnityEvent BossBattle;
    public UnityEvent CutsceneStart;
    public UnityEvent CutsceneEnd;
    public UnityEvent GameplayStart;
    public UnityEvent GameplayEnd;

    public void Start()
    {
        InitializeBossBattle();
    }

    private void InitializeBossBattle()
    {
        BossBattle.AddListener(BGMmanager.Instance.ChangeToBossBgm);
    }

    void Update()
    {
        
    }
}
