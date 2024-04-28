using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class WaterSplashScript : MonoBehaviour
{
    [SerializeField] private VisualEffect _waterSplashVFX;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        { 
            _waterSplashVFX.Play();
        }
    }
}
