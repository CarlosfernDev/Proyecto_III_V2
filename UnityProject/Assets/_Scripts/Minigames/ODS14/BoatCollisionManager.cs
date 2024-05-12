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
            ODS14Manager.Instance.garbageHit.Invoke();
            Destroy(other.transform.root.gameObject);
        }
        else if (other.gameObject.CompareTag("fish"))
        {
            ODS14Manager.Instance.AnimalHit();
            Destroy(other.transform.root.gameObject);
        }
    }
}
