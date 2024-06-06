using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "JuradoData", menuName = "ScriptableObjects/Utility/Unlockables", order = 3)]
public class SUnlockable : ScriptableObject
{
    public string UnlockableName;
    public string UnlockableDescription;
    public Sprite UnlockableImageOn;
    public Sprite UnlockableImageOff;

    public bool IsUnlocked;
}
