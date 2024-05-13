using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using Yarn.Unity;

public class TestDialogueInteract : LInteractableParent
{
    [SerializeField] private string _targetDialogueNode;


    private void Awake()
    {
        DialogueRunnerSingleton.Instance.onDialogueComplete.AddListener(SetInteractTrue);
    }

    public override void Interact()
    {
        if (!IsInteractable) return; 
        IsInteractable = false;
        DialogueRunnerSingleton.Instance.StartDialogue(_targetDialogueNode);
    }
}
