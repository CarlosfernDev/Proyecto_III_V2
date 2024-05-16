using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Yarn.Unity;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class CloudAI : MonoBehaviour
{
    
    [SerializeField] private float _range = 10f;
    [SerializeField] private float _speed = 3f;
    [SerializeField] private float _returnSpeed = 6f;
    
    private NavMeshAgent _agent;

    private Vector3 destination;
    
    public CloudSpawner targetCloudSpawner;
    public bool isReturningToPowerplant;
    public bool isCaptured;


    void Start()
    {
        InitNavigation();
    }

    void Update()
    {
        if (_agent == null) return;
        if (isCaptured) return;
        if (!isReturningToPowerplant)
        {
            Movement();
        }
    }

    private void InitNavigation()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    public void CloudCaptured()
    {
        isCaptured = true;
        _agent.isStopped = true;
        _agent.ResetPath();
    }
    
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {

        Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in a sphere 
        if (NavMesh.SamplePosition(randomPoint, out var hit, 1.0f, NavMesh.AllAreas)) //documentation: https://docs.unity3d.com/ScriptReference/AI.NavMesh.SamplePosition.html
        { 
            //the 1.0f is the max distance from the random point to a point on the navmesh, might want to increase if range is big
            //or add a for loop like in the documentation
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    private void Movement()
    {
        if(_agent.remainingDistance <= _agent.stoppingDistance) //done with path
        {
            SetNewDestination();
        }
    }

    private void SetNewDestination()
    {
        Vector3 point;
        if (RandomPoint(transform.position, _range, out point)) //pass in our centre point and radius of area
        {
            Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); //so you can see with gizmos
            _agent.SetDestination(point);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (_agent == null) return;
        if (_agent.remainingDistance > 0 && !isReturningToPowerplant)
        {
            _agent.isStopped = true;
            _agent.ResetPath();
            _agent.isStopped = false;
            SetNewDestination();
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (_agent == null) return;
        if (_agent.remainingDistance > 0 && !isReturningToPowerplant)
        {
            _agent.isStopped = true;
            _agent.ResetPath();
            _agent.isStopped = false;
            SetNewDestination();
        }
    }

    public void ReturnToBase(Transform targetPowerplant)
    {
        isReturningToPowerplant = true;
        _agent.speed = _returnSpeed;
        _agent.SetDestination(targetPowerplant.position);
    }

    public void CancelReturn()
    {
        isReturningToPowerplant = false;
        isCaptured = false;
        targetCloudSpawner = null;
        _agent.speed = _speed;
        _agent.ResetPath();
    }

    public void ResetMovement()
    {
        _agent.isStopped = true;
        _agent.ResetPath();
    }
}
