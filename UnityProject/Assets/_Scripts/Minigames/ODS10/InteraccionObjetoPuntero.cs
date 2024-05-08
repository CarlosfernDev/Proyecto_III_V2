using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteraccionObjetoPuntero : LInteractableParent
{
    public Animation_SelectionInteractuable animationHover;

    public override void Interact()
    {
        animationHover.StopAllCoroutines();
        transform.localScale = animationHover.oldScale;

        PunteroScript scriptpuntero = ODS10Singleton.Instance.puntero;
        gameObject.transform.SetParent(scriptpuntero.transform);
        scriptpuntero.refObjetoInteract = this.gameObject;
    }

    public void DesInteract()
    {

    }
}
