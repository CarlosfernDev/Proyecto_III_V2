using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionAnimation : MonoBehaviour
{
    [Header("Scale")]
    public Transform TransformReference;
    public float SpeedRotation;

    public void Update()
    {
        TransformReference.localEulerAngles = TransformReference.localEulerAngles + Vector3.forward * SpeedRotation * Time.deltaTime;
    }
}
