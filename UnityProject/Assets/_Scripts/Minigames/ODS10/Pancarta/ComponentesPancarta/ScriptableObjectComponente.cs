using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PancartaComponentData", menuName = "ScriptableObjects/ODS10/Componente", order = 2)]
public class ScriptableObjectComponente : ScriptableObject
{
    public Sprite ComponentMaterial;
    public bool IsGood;
    public Vector3 Scale;
}
