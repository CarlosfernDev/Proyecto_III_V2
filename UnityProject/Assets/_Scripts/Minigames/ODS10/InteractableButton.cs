using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableButton : LInteractableParent
{
    public UnityEvent OnClick;

    public override void Interact()
    {
        Debug.Log("Click");
        OnClick?.Invoke();  
    }
}
