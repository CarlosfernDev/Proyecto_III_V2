using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement_Crab : MonoBehaviour
{
    [SerializeField] private Transform _startPos;
    [SerializeField] private Transform _endPos;
    [SerializeField] private float speed = 1.0f;
    void Start()
    {
           
    }
    void Update()
    {
        transform.position = Vector3.Lerp(_startPos.position, _endPos.position, (Mathf.Sin(speed * Time.time) + 1.0f) / 2.0f);
    }
}
