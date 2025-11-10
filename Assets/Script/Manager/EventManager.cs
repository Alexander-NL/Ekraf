using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public UnityEvent BossBattle;

    public void Start()
    {
        BossBattle.AddListener(BGMmanager.Instance.ChangeToBossBgm);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
