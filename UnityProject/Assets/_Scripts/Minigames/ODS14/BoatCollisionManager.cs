using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatCollisionManager : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("hit");
        if (other.gameObject.CompareTag("fish"))
        {
            ODS14Manager.Instance.AnimalHit();
            Destroy(other.gameObject);
        }
    }
}
