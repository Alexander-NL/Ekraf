using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button gameplayButton;

    void Start()
    {
        gameplayButton.onClick.AddListener(BGMmanager.Instance.ChangeToGameplayBgm);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
