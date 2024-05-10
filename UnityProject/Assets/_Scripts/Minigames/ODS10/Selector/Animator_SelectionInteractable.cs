using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Animator_SelectionInteractable : SelectionInteractableParent
{
    public Animator _animator;

    [Header("Hover")]
    public string HoverTrigger = "Highlighted";

    [Header("Unhover")]
    public string UnhoverTrigger = "Normal";

    private void Start()
    {
        _animator.SetTrigger(UnhoverTrigger);
    }

    public override void Hover()
    {
        _animator.SetTrigger(HoverTrigger);
        base.Hover();
    }

    public override void UnHover()
    {
        _animator.SetTrigger(UnhoverTrigger);
        base.UnHover();
    }
}
