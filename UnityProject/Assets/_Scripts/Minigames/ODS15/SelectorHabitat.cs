using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorHabitat : LInteractableParent
{
    public int HabitatNumber;

    public override void Interact()
    {
        GameManager15.Instance.SetHabitatActiva(HabitatNumber);
    }
}
