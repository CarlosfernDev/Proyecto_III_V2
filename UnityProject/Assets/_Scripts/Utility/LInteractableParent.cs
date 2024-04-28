using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LInteractableParent : MonoBehaviour, Iinteractable
{

    public string _TextoInteraccion;
    [SerializeField] private bool _isInteractable;
    public string TextoInteraccion
    {
        get { return _TextoInteraccion; }
        set { _TextoInteraccion = value; }
    }
    public bool IsInteractable
    {
        get { return _isInteractable; }
        set { _isInteractable = value; }
    }

    public virtual void Interact()
    {

    }

    public void SetInteractFalse()
    {
        IsInteractable = false;
    }

    public void SetInteractTrue()
    {
        IsInteractable = true;
    }
}
