using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorDecoracion : LInteractableParent
{
    public int DecorNumber;


    public override void Interact()
    {
        GameManager15.Instance.SetDecoracionActiva(DecorNumber);
    }
}
