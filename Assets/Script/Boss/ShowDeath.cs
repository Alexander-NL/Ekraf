using TMPro;
using UnityEngine;

public class ShowDeath : MonoBehaviour
{
    public TextMeshProUGUI Deathtext;

    public void UpdateText()
    {
        Deathtext.text = "HERO'S TOTAL DEATH: " + SaveSystem.Instance.Death;
    }
}
