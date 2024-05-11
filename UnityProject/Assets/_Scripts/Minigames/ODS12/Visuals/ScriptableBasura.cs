using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MinigameData", menuName = "ScriptableObjects/Utility/ODS12/BasuraData", order = 1)]
public class ScriptableBasura : ScriptableObject
{
    public Mesh Modelo3D;
    public Material Textura;

    public Vector3 Postion;
    public Vector3 Scale;
    public Vector3 Angle;
}
