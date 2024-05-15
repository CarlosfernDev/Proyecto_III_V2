using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class IAnube : MonoBehaviour
{
    [SerializeField] private Transform posFactory;
    [SerializeField] private Vector3 vecMovementDir;
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private LayerMask layer;
    [SerializeField] private Collider col;
    [SerializeField] public bool isReturningToFactory = false;
    [SerializeField] public bool isStandBY = false;

    private Vector3 factoryDirection;

    [HideInInspector] public CloudSpawner targetCloudSpawner;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        moveNewDirection();
    }
    private void Update()
    {
        if (isStandBY)
        {
            rb.velocity = Vector3.zero;
            return;
        }
        if (isReturningToFactory)
        {
            rb.velocity = factoryDirection * speed;
            //transform.LookAt(posFactory);
            return;
        }

        rb.velocity = transform.forward * speed;
    }

    void getRandomDir()
    {
        vecMovementDir = new Vector3(0, Random.Range(0, 360), 0);
    }
    void setRotation(Vector3 vec)
    {
        transform.rotation = Quaternion.Euler(vec);
    }

    void moveNewDirection()
    {
        getRandomDir();
        setRotation(vecMovementDir);
    }

  

    private void OnCollisionStay(Collision collision)
    {

        //Funcion de detectar cosas con lo que rebotar, deben tener la misma layer
        Debug.Log(collision.gameObject.layer + "////" + ToLayer(layer));
        if (collision.gameObject.layer == ToLayer(layer) && !isReturningToFactory)
        {
            moveNewDirection();
        }

        if (collision.gameObject.TryGetComponent<CloudSpawner>(out CloudSpawner cloud))
        {
            Debug.Log("FACTORY DETECTED");
            if (isReturningToFactory)
            {
                // Aqui metere cosas yo AT: Stamp
                cloud.RestoreFactory();
                isReturningToFactory = false;
            }
            else
            {
                moveNewDirection();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent<CloudSpawner>(out CloudSpawner cloud))
        {
            if (isReturningToFactory)
            {

                // Aqui metere cosas yo AT: Stamp
                col.isTrigger = false;
                cloud.RestoreFactory();
                isReturningToFactory = false;
            }
        }
    }

    public static int ToLayer(int bitmask)
    {
        int result = bitmask > 0 ? 0 : 31;
        while (bitmask > 1)
        {
            bitmask = bitmask >> 1;
            result++;
        }
        return result;
    }

    public void WEGOINGTOFACTORYYYYYYYYBOYSSS(Transform _posFactory)
    {
        col.isTrigger = true;
        posFactory = _posFactory;
        factoryDirection = new Vector3(posFactory.position.x - this.transform.position.x, 0, posFactory.position.z - this.transform.position.z).normalized;
        isReturningToFactory = true;
    }
}
