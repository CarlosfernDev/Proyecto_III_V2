using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PancartaArea : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    

    private void OnTriggerEnter(Collider other)
    {
       Debug.Log("INSIDE"+other.gameObject.name); 
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("OUTSIDE"+other.gameObject.name);

    }

}
