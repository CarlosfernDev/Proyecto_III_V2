using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "JuradoData", menuName = "ScriptableObjects/Utility/JuradoData", order = 3)]
public class JuradoScriptable : ScriptableObject
{
    public List<JuradoDataComponent> JuradoList;
}

[System.Serializable]
public class JuradoDataComponent
{
    public Sprite sprite;
    public string AnimationTrigger;
}
