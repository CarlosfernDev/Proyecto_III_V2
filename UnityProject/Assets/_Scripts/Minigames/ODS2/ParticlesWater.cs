using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesWater : MonoBehaviour
{
    public ParticleSystem particles;

    public AudioSource waterAudio;

    public void StartParticles()
    {
        particles.Play();
    }

    public void WaterAudioPlay()
    {
        waterAudio.Play();
    }
}
