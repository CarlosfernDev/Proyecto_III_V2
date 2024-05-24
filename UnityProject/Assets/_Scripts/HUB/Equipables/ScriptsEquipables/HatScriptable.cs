using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sombrero", menuName = "ScriptableObjects/Equipment/HatScriptable", order = 1)]
public class HatScriptable : ScriptableObject
{
    public Material HeadMaterial;
    public bool DisableDefaultHat;
    public GameObject HatPrefab;

}
