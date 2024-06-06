using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatCollisionManager : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.transform.parent.TryGetComponent(out FloatingGarbage garbage))
        {
            if(AudioManager.Instance != null )AudioManager.Instance.Play("BobFish");
            ODS14Manager.Instance.GarbageHit(garbage);
            Destroy(other.transform.root.gameObject);
        }
        else if (other.gameObject.CompareTag("fish"))
        {
            if (AudioManager.Instance != null) AudioManager.Instance.Play("Plonk");
            ODS14Manager.Instance.AnimalHit();
            Destroy(other.transform.root.gameObject);
        }
    }
}
