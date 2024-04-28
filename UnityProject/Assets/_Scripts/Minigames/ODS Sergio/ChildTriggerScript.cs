using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildTriggerScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        transform.parent.gameObject.GetComponent<SpawnSystem>().TriggerOnChildEnterer(this.gameObject);
    }
}
