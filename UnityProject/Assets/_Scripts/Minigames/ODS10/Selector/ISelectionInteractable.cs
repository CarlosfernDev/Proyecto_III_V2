using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectionInteractable
{
    public virtual void Select() { }
    public virtual void Hover() { }
    public virtual void UnHover() { }
}