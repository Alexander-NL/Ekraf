using System.Collections;
using UnityEngine;

public class TurretManager : MonoBehaviour
{
    public GameObject[] TurretHead;

    public void RefreshTurret()
    {
        StartCoroutine(refreshDelay());
    }

    IEnumerator refreshDelay()
    {
        yield return new WaitForSeconds(2f);

        foreach (var t in TurretHead)
        {
            t.SetActive(true);
        }
    }
}
