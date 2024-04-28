using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using Yarn.Unity;

public class TestDialogueInteract : MonoBehaviour, Iinteractable
{
    [SerializeField] private bool _isInteractable = true;
    [SerializeField] private string _textoInteraccion;
    [SerializeField] private string _targetDialogueNode;

    public string TextoInteraccion
    {
        get { return _textoInteraccion; }
        set { _textoInteraccion = value; }
    }
    public bool IsInteractable
    {
        get { return _isInteractable; }
        set { _isInteractable = value; }
    }

    private void Awake()
    {
        DialogueRunnerSingleton.Instance.onDialogueComplete.AddListener(SetInteractTrue);
    }

    public void Interact()
    {
        if (!IsInteractable) return; 
        IsInteractable = false;
        DialogueRunnerSingleton.Instance.StartDialogue(_targetDialogueNode);
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
