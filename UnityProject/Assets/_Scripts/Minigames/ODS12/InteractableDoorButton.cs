using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDoorButton : LInteractableParent
{
    [SerializeField] private GameObject interactObj;
    [SerializeField] private string animationName;
    
    public override void Interact()
    {
        base.Interact();
        
        if (!interactObj.TryGetComponent(out Animator targetAnim)) return;
        targetAnim.Play(animationName);
    }
}
