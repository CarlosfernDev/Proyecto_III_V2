using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractuableMateriales : LInteractableParent
{
    public override void Interact()
    {
        
        GameManagerSergio.Instance.addMaterial(10);
        //Destroy(this.gameObject);
    }
}
