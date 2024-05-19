using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using Yarn.Unity;

public class TestDialogueInteract : LInteractableParent
{
    [SerializeField] private string _targetDialogueNode;
    private float TimeReference;

    public override void Interact()
    {
        if (GameManager.Instance.isDialogueActive) return;
        if (Time.time - TimeReference < 0.5f) return;

        if (!IsInteractable) return;
        DialogueRunnerSingleton.Instance._LineView.ResetTimer();
        DialogueRunnerSingleton.Instance._LineView.OnComplete += ResetColdownDialogue;
        DialogueRunnerSingleton.Instance._dialogueRunner.StartDialogue(_targetDialogueNode);
    }

    public void ResetColdownDialogue()
    {
        TimeReference = Time.time;
        DialogueRunnerSingleton.Instance._LineView.OnComplete -= ResetColdownDialogue;
    }
}
