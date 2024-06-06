using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractuableMateriales : LInteractableParent
{
    public override void Interact()
    {
        
        GameManagerSergio.Instance.addMaterial(1);

        if(AudioManager.Instance != null) AudioManager.Instance.Play("BobFish");

        GameObject.Find("Player").GetComponent<TestInputs>().refObjetoInteract = null;

        Destroy(this.gameObject);
    }
}
