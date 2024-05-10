using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class BoatMovementManager : MonoBehaviour
{
    [SerializeField] private TMP_Text velocityDebug;
    
    [SerializeField] private float topSpeed;
    [SerializeField] private float forceMult;
    [SerializeField] private float accel;
    [SerializeField] private float turnSpd;
    [SerializeField] private GameObject turnPoint;
    
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
        _rb.AddRelativeTorque(_inputManager.TurnInput * turnSpd, ForceMode.Acceleration);
        SpeedUp();
        _rb.AddRelativeForce(_inputManager.MovementInput * accel, ForceMode.Acceleration);
    }

    private void SpeedUp()
    {
        if (_inputManager.MovementInput == Vector3.zero) return;
        Vector3 targetVelocity = _inputManager.MovementInput * topSpeed;
        Vector3 force = (targetVelocity - _rb.velocity) * forceMult;

        if (force.magnitude > forceMult)
        {
            force = force.normalized * forceMult;
        }
        _rb.AddRelativeForce(force, ForceMode.Acceleration);
    }
}
