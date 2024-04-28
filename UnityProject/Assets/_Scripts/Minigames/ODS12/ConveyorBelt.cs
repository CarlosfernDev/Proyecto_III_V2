using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConveyorBelt : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private bool isCorner;
    [SerializeField] private Vector3 perpDirection;

    [SerializeField] private List<Rigidbody> _rbOnBelt;
    private Material _material;

    private void Start()
    {
        ODS12Singleton.Instance.OnGarbageDelivered.AddListener(CheckRBAvailability);
        _rbOnBelt = new List<Rigidbody>();
        _material = GetComponent<MeshRenderer>().material;
    }

    private void FixedUpdate()
    {
        if (_rbOnBelt == null) return;
        foreach (Rigidbody beltRb in _rbOnBelt)
        {
            if (beltRb == null)
            {
                _rbOnBelt.Remove(beltRb);
                return;
            }
            beltRb.TryGetComponent(out GarbageScript garbage);
            if (!garbage)
            {
                MoveOnBelt(beltRb);
                continue;
            }

            if (!isCorner)
            {
                MoveOnBelt(beltRb);
                continue;
            }

            if (!garbage.isCentered)
            {
                Vector3 iniMovement = speed * perpDirection * Time.deltaTime;
                beltRb.MovePosition(beltRb.transform.position + iniMovement);

                if (DistanceInXZ(beltRb.transform.position, this.transform.position) > 0.05f) return;
                garbage.isCentered = true;
                continue;
            }
            MoveOnBelt(beltRb);
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.TryGetComponent(out Rigidbody rb)) return;
        _rbOnBelt.Add(rb);
    }

    public void OnCollisionExit(Collision other)
    {
        if (!other.gameObject.TryGetComponent(out Rigidbody rb)) return;
        if (rb.TryGetComponent(out GarbageScript garbage))
        {
            garbage.isCentered = false;
        }
        _rbOnBelt.Remove(rb);
    }

    private float DistanceInXZ(Vector3 initial, Vector3 final)
    {
        Vector3 distanceVec = new Vector3(0, 0, 0);

        var distX = (initial.x - final.x);
        var distZ = (initial.z - final.z);

        distanceVec.Set(distX, 0, distZ);

        return distanceVec.magnitude;
    }

    private void MoveOnBelt(Rigidbody rb)
    {
        Vector3 movement = speed * transform.forward * Time.deltaTime;
        rb.MovePosition(rb.gameObject.transform.position + movement);
    }

    private void CheckRBAvailability()
    {
        foreach (var rbCol in _rbOnBelt)
        {
            if (rbCol == null)
            {
                _rbOnBelt.Remove(rbCol);
            }
        }
    }
}