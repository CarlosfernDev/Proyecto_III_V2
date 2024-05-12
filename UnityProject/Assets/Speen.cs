using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speen : MonoBehaviour
{
    [SerializeField] private float spinSpeed = 0.5f;
    void Start()
    {
        
    }

    void Update()
    {
        transform.RotateAround(transform.position, Vector3.up, spinSpeed * Time.deltaTime);
    }
}
