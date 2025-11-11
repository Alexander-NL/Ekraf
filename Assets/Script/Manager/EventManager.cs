using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public UnityEvent BossBattle;
    public UnityEvent CutsceneStart;
    public UnityEvent GameplayStart;

    public void Start()
    {
        BossBattle.AddListener(BGMmanager.Instance.ChangeToBossBgm);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
