using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSwaps : LInteractableParent
{
    public int MenuNumber;

    public override void Interact()
    {
        GameManager15.Instance.MenusSwap(MenuNumber);
    }
}
