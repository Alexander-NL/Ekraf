using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretManager : MonoBehaviour
{
    public static TurretManager Instance { get; private set; }


    public List<TurretBehaviour> turretList;
    public List<GameObject> arrowStash;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void RefreshTurret()
    {
        foreach (var t in turretList)
        {
            t.gameObject.SetActive(true);
            t.isDead = false;
        }

        foreach (var t in arrowStash)
        {
            Destroy(t.gameObject);
        }
        arrowStash.Clear();
    }
}
