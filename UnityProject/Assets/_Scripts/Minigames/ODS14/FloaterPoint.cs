// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class FloaterPoint : MonoBehaviour
// {
//     [SerializeField] private int floaterCount;
//     [SerializeField] private float depthBeforeSubmerged = 1f;
//     [SerializeField] private float displacementAmount = 3f;
//     [SerializeField] private float waterDrag = 0.99f;
//     [SerializeField] private float waterAngularDrag = 0.5f;
//     
//     [SerializeField] private Rigidbody _rb;
//     
//     void Start()
//     {
//         _rb = transform.root.GetComponent<Rigidbody>();
//     }
//     
//     void FixedUpdate()
//     {
//         _rb.AddForceAtPosition(Physics.gravity / floaterCount, transform.position, ForceMode.Acceleration);
//         float waveHeight = WaveManager.Instance.GetWaveHeight(transform.position.x, transform.position.z) + WaveManager.Instance.transform.position.y;
//
//         if (transform.position.y < waveHeight)
//         {
//             float displacementMultiplier = Mathf.Clamp01((waveHeight - transform.position.y) / depthBeforeSubmerged) * displacementAmount;
//             _rb.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), transform.position, ForceMode.Acceleration);
//             _rb.AddForce(-_rb.velocity * (displacementMultiplier * waterDrag * Time.fixedDeltaTime), ForceMode.VelocityChange);
//             _rb.AddTorque(-_rb.angularVelocity * (displacementMultiplier * waterAngularDrag * Time.fixedDeltaTime), ForceMode.VelocityChange);    
//         }
//     }
// }
