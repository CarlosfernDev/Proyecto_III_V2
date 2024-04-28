using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorAlimentacion : MonoBehaviour, Iinteractable
{
    [SerializeField] private bool _isInteractable;
    public string _TextoInteraccion;
    public int AlimentNumber;

    //Por como se implementan las interfaces se debe poner el set y get de las variables.
    //Ni idea de si hay otra manera de implementar variables de una interfaz sin poner esto.
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


    public void Interact()
    {
        GameManager15.Instance.SetComidaActiva(AlimentNumber);
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
