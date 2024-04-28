using System.Collections;
using System.Collections.Generic;
//using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class XRayShaderScript : MonoBehaviour
{
    public static int PosID = Shader.PropertyToID("_Player_Position");
    public static int SizeID = Shader.PropertyToID("_Size");

    private Material _WallMaterial;
    [SerializeField] private Camera _Camera;
    [SerializeField] private LayerMask _Mask;
    [SerializeField] private float _Opacity = 0.5f;

    void Update()
    {
        var dir = _Camera.transform.position - transform.position;
        var ray = new Ray(transform.position, dir.normalized);


        if (Physics.Raycast(ray, out RaycastHit hit, 3000, _Mask))
        {
            _WallMaterial = hit.collider.gameObject.GetComponent<Renderer>().material;
            _WallMaterial.SetFloat(SizeID, _Opacity);
        }
        else
        {
            if(_WallMaterial != null)
            _WallMaterial.SetFloat(SizeID, 0);
        }

        var view = _Camera.WorldToViewportPoint(transform.position);

        if(_WallMaterial != null)
        _WallMaterial.SetVector(PosID, view);
    }
}
