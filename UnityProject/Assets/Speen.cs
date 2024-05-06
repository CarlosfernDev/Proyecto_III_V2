using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speen : MonoBehaviour
{
    [SerializeField] private float spinSpeed = 0.5f;
    public Transform target;
    void Start()
    {
        
    }

    void Update()
    {
        Vector3 relativePos = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos);

        Quaternion current = transform.localRotation;

        transform.localRotation = Quaternion.Slerp(current, current, Time.deltaTime);
        transform.Translate(0, 0, 3 * Time.deltaTime);
    }
}
