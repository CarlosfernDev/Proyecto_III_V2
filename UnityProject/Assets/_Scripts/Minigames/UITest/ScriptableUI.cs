using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "UIdata", menuName = "ScriptableObjects/UIdata", order = 1)]
public class ScriptableUI : ScriptableObject
{
    public ScriptableUiComponent Movement;
    public ScriptableUiComponent Interact;
    public ScriptableUiComponent Pausa;
    public ScriptableUiComponent UsarEquipable;
    public ScriptableUiComponent RotarPieza;
    public ScriptableUiComponent AnyKey;
    
}

[Serializable]
public class ScriptableUiComponent
{
    public Sprite SinPulsar;
    public Sprite Pulsado;
    public AnimatorOverrideController Animator;
}
