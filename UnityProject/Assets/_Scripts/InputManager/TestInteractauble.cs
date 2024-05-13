using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInteractauble : LInteractableParent
{

    public override void Interact()
    {
        Debug.Log("INTERACTUO CON " + transform.name+" "+ _TextoInteraccion);
        transform.GetComponent<MeshRenderer>().enabled = false;
        
    }
}
