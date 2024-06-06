using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TortugaInteract : LInteractableParent
{
    

    public override void Interact()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.Play("Tortuga");
    }


}
