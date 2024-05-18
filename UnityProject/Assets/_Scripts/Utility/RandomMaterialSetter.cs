using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMaterialSetter : MonoBehaviour
{
    public MeshRenderer Render;
    public Material[] MaterialList;
    private void Start()
    {
        Render.material = MaterialList[Random.Range(0, MaterialList.Length)];
    }
}
