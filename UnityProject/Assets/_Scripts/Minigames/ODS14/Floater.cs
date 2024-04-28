using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    private Rigidbody _rb;
    [SerializeField] private List<GameObject> floatPoints;
    [SerializeField] private float depthBeforeSubmerged = 1f;
    [SerializeField] private float displacementAmount = 3f;
    [SerializeField] private float waterDrag = 0.99f;
    [SerializeField] private float waterAngularDrag = 0.5f;
    [SerializeField] private float floaterCount;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        floaterCount = floatPoints.Count;
    }
    private void FixedUpdate()
    {
        foreach (var point in floatPoints)
        {
            //var pointPos = point.transform.position;
            // float waveHeight = WaveManager.Instance.GetWaveHeight(point.transform.position.x, point.transform.position.z) + WaveManager.Instance.transform.position.y;
            Vector3 wavePosition = WaveManager.Instance.GetWaveDisplacement(point.transform.position);
            float waveHeight = wavePosition.y + WaveManager.Instance.transform.position.y;
            _rb.AddForceAtPosition(Physics.gravity / floatPoints.Count, point.transform.position, ForceMode.Acceleration);

            if (point.transform.position.y < waveHeight)
            {
                float displacementMultiplier = Mathf.Clamp01((waveHeight - point.transform.position.y) / depthBeforeSubmerged) * displacementAmount;
                _rb.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), point.transform.position, ForceMode.Acceleration);
                _rb.AddForce(-_rb.velocity * (displacementMultiplier * waterDrag * Time.fixedDeltaTime), ForceMode.VelocityChange);
                _rb.AddTorque(-_rb.angularVelocity * (displacementMultiplier * waterAngularDrag * Time.fixedDeltaTime), ForceMode.VelocityChange);    
            }
        }
    }
}
