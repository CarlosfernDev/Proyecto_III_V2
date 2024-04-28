using System.Collections;
using System.Collections.Generic;
using Unity.XR.Oculus.Input;
using UnityEngine;

public class TroncoParent : MonoBehaviour
{
    public GameObject RefPlayer = null;
    public float speed;
    public Vector3 dir;
    public Rigidbody rb;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Player")
        {
            other.gameObject.transform.parent = this.transform.parent.transform;
            RefPlayer = other.gameObject;
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.name == "Player")
        {
            if (other.gameObject.transform.parent.gameObject == this.transform.parent.gameObject)
            {
                other.gameObject.transform.parent = null;
                RefPlayer = null;
            }
           
        }
    }
}
