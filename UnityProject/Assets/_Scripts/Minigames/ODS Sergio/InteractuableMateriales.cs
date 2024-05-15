using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractuableMateriales : LInteractableParent
{
    public override void Interact()
    {
        
        GameManagerSergio.Instance.addMaterial(1);
        GameObject.Find("Player").GetComponent<TestInputs>().refObjetoInteract = null;

        Destroy(this.gameObject);
    }
}
