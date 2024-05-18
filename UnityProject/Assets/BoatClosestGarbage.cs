using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatClosestGarbage : MonoBehaviour
{
    public Transform centerPoint;
    public LookAtZAxis indicatorArrow;
    void Start()
    {
        
    }

    void Update()
    {
        indicatorArrow.TargetTransform = NearestGarbagePosition();
    }
    
    private Transform NearestGarbagePosition()
    {
        Transform firstClosest = null;
        float closestDistSqr = Mathf.Infinity;

        foreach (GameObject garbage in ODS14Manager.Instance._floatingGarbage)
        {
            Vector3 garbagePosition = garbage.transform.position;
            Vector3 directionToGarbage = garbagePosition - centerPoint.transform.position;
            float dSqrToGarbage = directionToGarbage.sqrMagnitude;
            if (dSqrToGarbage < closestDistSqr)
            {
                closestDistSqr = dSqrToGarbage;
                firstClosest = garbage.transform;
            }
        }

        return firstClosest;
    }
}
