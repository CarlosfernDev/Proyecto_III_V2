using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerAnimation : MonoBehaviour
{
    [SerializeField] private float RotationValue;
    [SerializeField] private Animator Animator;
    bool Rotate = false;

    public void StartAnimation()
    {
        Animator.gameObject.SetActive(true);
        Animator.SetTrigger("Start");
    }

    public void RotateAnimation()
    {
        if (Rotate) { 
            transform.localEulerAngles = Vector3.forward * -RotationValue;
            Rotate = false;
        }
        else
        {
            transform.localEulerAngles = Vector3.forward * RotationValue;
            Rotate = true;
        }
    }
}
