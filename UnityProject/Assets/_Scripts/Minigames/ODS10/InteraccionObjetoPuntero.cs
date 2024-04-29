using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteraccionObjetoPuntero : LInteractableParent
{
    public override void Interact()
    {
        PunteroScript scriptpuntero = ODS10Singleton.Instance.puntero;
        gameObject.transform.SetParent(scriptpuntero.transform);
        scriptpuntero.refObjetoInteract = this.gameObject;
    }
}
