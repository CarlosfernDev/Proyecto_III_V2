using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LInteractableParent : MonoBehaviour, Iinteractable
{
    public string _TextoInteraccion;
    [SerializeField] private bool _isInteractable;
    [SerializeField] private bool checkInput1Time = false;

    public GameObject Selector;
    public GameObject ButtonPrompt;
    public UItype WhatInput;

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
        if (ButtonPrompt != null) 
        {
           
            if (!checkInput1Time)
            {
                checkInputType();
            }
            
            ButtonPrompt.GetComponent<AnimatorUIController>().PopIn();

        }
    }

    public virtual void Unhover()
    {
        if (Selector != null) Selector.SetActive(false);
        if (ButtonPrompt != null) 
        {
            
            ButtonPrompt.GetComponent<AnimatorUIController>().PopOut();

        }
        
    }

    public void checkInputType()
    {
        ButtonPrompt.GetComponent<UIswapTest>().selecImage = WhatInput;
        checkInput1Time = true;
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
