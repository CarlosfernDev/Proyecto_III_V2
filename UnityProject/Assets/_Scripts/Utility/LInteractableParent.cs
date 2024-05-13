using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LInteractableParent : MonoBehaviour, Iinteractable
{
    public string _TextoInteraccion;
    [SerializeField] private bool _isInteractable;

    public GameObject Selector;

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

    public virtual void Hover()
    {
        if (Selector != null) Selector.SetActive(true);
    }

    public virtual void Unhover()
    {
        if (Selector != null) Selector.SetActive(false);
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
