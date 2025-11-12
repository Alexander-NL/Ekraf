using System.Collections;
using UnityEngine;

public class Particledead : MonoBehaviour
{
    public ParticleSystem[] particles;

    void Start()
    {
        foreach (var particle in particles)
        {
            particle.Stop();
            particle.Play();
        }
    }
}
