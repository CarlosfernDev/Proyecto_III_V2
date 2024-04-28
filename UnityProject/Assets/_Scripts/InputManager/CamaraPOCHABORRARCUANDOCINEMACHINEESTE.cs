using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraPOCHABORRARCUANDOCINEMACHINEESTE : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private Vector3 offsetPosition;
    [SerializeField] private Vector3 offsetRotation;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        transform.rotation = Quaternion.Euler(offsetRotation);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position+offsetPosition;
    }
}
