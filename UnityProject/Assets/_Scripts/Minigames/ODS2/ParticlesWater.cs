using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesWater : MonoBehaviour
{
    public ParticleSystem particles;

    public void StartParticles()
    {
        particles.Play();
    }
}
