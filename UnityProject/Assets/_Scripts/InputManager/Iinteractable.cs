using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Iinteractable 
{
    public bool IsInteractable { get; set; }
    string TextoInteraccion { get; set; }
    public void Interact();
    public void SetInteractFalse();
    public void SetInteractTrue();
    public void Hover();
    public void Unhover();
}
