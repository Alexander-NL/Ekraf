using System.Collections;
using UnityEngine;

public class Particledead : MonoBehaviour
{
    public float delay;

    void Start()
    {
        StartCoroutine(deadParticle());
    }

    IEnumerator deadParticle()
    {
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
    }
}
