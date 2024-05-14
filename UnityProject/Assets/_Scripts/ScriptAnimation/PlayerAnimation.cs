using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public TestInputs Inputs;
    public Rigidbody rb;
    public Animator _anim;

    public int minimalSpeed;
    public float maximalSpeed;

    private void FixedUpdate()
    {
        _anim.SetFloat("Speed", Mathf.Clamp(rb.velocity.magnitude - minimalSpeed, 0, maximalSpeed) / maximalSpeed);
    }
}
