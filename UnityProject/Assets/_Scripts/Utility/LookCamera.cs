using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookCamera : MonoBehaviour
{
    public Transform _camera;

    private void Start()
    {
        if (_camera == null)
            _camera = Camera.main.transform;
    }

    private void Update()
    {
        transform.LookAt(transform.position + _camera.forward);
    }
}
