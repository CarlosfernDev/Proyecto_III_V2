using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using Yarn.Unity;

public class TestDialogueInteract : LInteractableParent
{
    [SerializeField] private string _targetDialogueNode;

    public override void Interact()
    {
        if (GameManager.Instance.isDialogueActive) return;

        if (!IsInteractable) return;
        DialogueRunnerSingleton.Instance._LineView.ResetTimer();
        DialogueRunnerSingleton.Instance._dialogueRunner.StartDialogue(_targetDialogueNode);
    }
}
