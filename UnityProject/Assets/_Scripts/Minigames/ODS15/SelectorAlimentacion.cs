using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorAlimentacion : LInteractableParent
{
    public int AlimentNumber;

    public override void Interact()
    {
        GameManager15.Instance.SetComidaActiva(AlimentNumber);
    }
}
