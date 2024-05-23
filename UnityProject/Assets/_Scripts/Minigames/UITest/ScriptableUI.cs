using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "UIdata", menuName = "ScriptableObjects/UIdata", order = 1)]
public class ScriptableUI : ScriptableObject
{
    public Sprite Movement;
    public Sprite Interact;
    public Sprite Pausa;
    public Sprite UsarEquipable;
    public Sprite RotarPieza;
    public Sprite AnyKey;

    public ScriptableUiComponent Test;
}

[Serializable]
public class ScriptableUiComponent
{
    public Sprite Sprite1;
    public Sprite Sprite2;
    public AnimatorOverrideController Animator;
}
