using TMPro;
using UnityEngine;

public class ShowDeath : MonoBehaviour
{
    public TextMeshProUGUI Deathtext;
    public TextMeshProUGUI BestScore;

    public void UpdateText()
    {
        Deathtext.text = "HERO'S TOTAL DEATH: " + SaveSystem.Instance.Death;
        int bestScore = SaveSystem.Instance.GetDeathTotal();
        BestScore.text = "FEWEST DEATH: " + bestScore;
    }
}
