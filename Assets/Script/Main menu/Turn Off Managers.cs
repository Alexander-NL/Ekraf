using UnityEngine;

public class TurnOffManagers : MonoBehaviour
{
    public GameObject[] turnoff;
    public GameObject[] turnon;

    void Start()
    {
        foreach (var turn in turnoff)
        {
            turn.gameObject.SetActive(false);
        }


        if (turnon.Length > 0) return;
        foreach (var turn in turnon)
        {
            turn.gameObject.SetActive(true);
        }
    }
}
