using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class BoatMovementManager : MonoBehaviour
{
    [SerializeField] private TMP_Text velocityDebug;
    
    [SerializeField] private float topFwdSpeed;
    [SerializeField] private float fwdForceMult;
    [SerializeField] private float topBwdSpeed;
    [SerializeField] private float bwdForceMult;
    [SerializeField] private float turnSpd;
    
    private BoatInputManager _inputManager;
    private Rigidbody _rb;

    private void OnEnable()
    {
        _inputManager = GetComponent<BoatInputManager>();
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //transform.Translate(_inputManager.MovementInput * accel * Time.deltaTime);
        //transform.Rotate(_inputManager.TurnInput * turnSpd * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if ((MySceneManager.Instance != null ? MySceneManager.Instance.isLoading : false) || (GameManager.Instance != null ? GameManager.Instance.isPaused : false)) return;
        if (!ODS14Manager.Instance.canMove) return;
        
        SpeedUp();
        //MoveForwardBackward();
        TurnLeftRight();
    }

    private void SpeedUp()
    {
        if (_inputManager.MovementInput == Vector3.zero) return;
        if (_inputManager.MovementInput.z > 0f)
        {
            Vector3 targetVelocity = _inputManager.MovementInput * topFwdSpeed;
            Vector3 force = new Vector3(0,0,(targetVelocity.z - _rb.velocity.z) * fwdForceMult);

            if (force.magnitude > fwdForceMult)
            {
                force = force.normalized * fwdForceMult;
            }
            _rb.AddRelativeForce(force, ForceMode.Acceleration);
        }
        else
        {
            Vector3 targetVelocity = _inputManager.MovementInput * topBwdSpeed;
            Vector3 force = new Vector3(0,0,(targetVelocity.z - _rb.velocity.z) * bwdForceMult);

            if (force.magnitude > bwdForceMult)
            {
                force = force.normalized * bwdForceMult;
            }
            _rb.AddRelativeForce(force, ForceMode.Acceleration);
        }
        
    }

    private void MoveForwardBackward()
    {
        if (_inputManager.MovementInput.z != 0)
        {
            //_rb.AddRelativeForce(_inputManager.MovementInput * accel, ForceMode.Acceleration);
        }
    }

    private void TurnLeftRight()
    {
        float _fwInput = _inputManager.MovementInput.z;
        Vector3 _LocalVelocity = transform.InverseTransformDirection(_rb.velocity);
        float _zVelocity = _LocalVelocity.z;
        
        if (((_fwInput > 0f) || _zVelocity >= 1f))
        {
            _rb.AddRelativeTorque(_inputManager.TurnInput * turnSpd, ForceMode.Acceleration);
        }
        else if (((_fwInput < 0f) || _zVelocity <= -1f))
        {
            _rb.AddRelativeTorque(_inputManager.TurnInput * turnSpd, ForceMode.Acceleration);
        }
    }
}
