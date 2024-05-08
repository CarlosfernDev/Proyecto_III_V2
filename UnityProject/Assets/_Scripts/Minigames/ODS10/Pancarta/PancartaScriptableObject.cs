using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

[Serializable]
[CreateAssetMenu(fileName = "PancartaData", menuName = "ScriptableObjects/ODS10/Pancarta", order = 1)]
public class PancartaScriptableObject : ScriptableObject
{
    public List<ScriptableObjectComponente> ListaComponentes;

    public Material _PancartaMaterial;
    public RenderTexture _PacartaRenderTexture;
    public string _PancartaName;

    public void LoadTexture()
    {
        if (!File.Exists(SaveManager.SAVE_FOLDER + _PancartaName + ".png")) return;
        byte[] bytes = File.ReadAllBytes(SaveManager.SAVE_FOLDER + _PancartaName + ".png");

        Texture2D LoadedImage = new Texture2D(_PacartaRenderTexture.width, _PacartaRenderTexture.height);
        LoadedImage.LoadImage(bytes);
        _PancartaMaterial.mainTexture = LoadedImage;
    }

    public void SaveTexture()
    {
        Texture2D screenshotTexture = new Texture2D(_PacartaRenderTexture.width, _PacartaRenderTexture.height, TextureFormat.ARGB32, false);
        RenderTexture.active = _PacartaRenderTexture;

        Rect rect = new Rect(0, 0, _PacartaRenderTexture.width, _PacartaRenderTexture.height);
        screenshotTexture.ReadPixels(rect, 0, 0);
        screenshotTexture.Apply();

        byte[] byteArray = screenshotTexture.EncodeToPNG();
        System.IO.File.WriteAllBytes(SaveManager.SAVE_FOLDER + _PancartaName + ".png", byteArray);

        RenderTexture.active = null;
        Debug.Log("Foto realizada");
    }
}
