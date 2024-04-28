using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

[Serializable]
public class InGameComponentCartel
{
    public GameObject cartel;
    public Image material;
}

public class ODS10Singleton : MinigameParent
{
    public int ListValue;

    public PunteroScript puntero;

    public Camera PancartaCamera;
    public static ODS10Singleton Instance;

    public List<PancartaScriptableObject> scriptables;
    public List<InGameComponentCartel> Carteles;

    [SerializeField] PancartaScriptableObject pancartaScriptableObject;

    protected override void personalAwake()
    {
        Instance = this;
        base.personalAwake();
        ExecuteGame();
    }

    public void ExecuteGame(int Value)
    {
        pancartaScriptableObject = scriptables[Value];
        ExecuteGame();
    }

    public void ExecuteGame()
    {
        if (pancartaScriptableObject == null)
            Debug.LogError("No se ha definido el tipo de pancarta");

        List<ScriptableObjectComponente> TemporalList = new List<ScriptableObjectComponente>(pancartaScriptableObject.ListaComponentes);

        foreach (InGameComponentCartel cartel in Carteles)
        {
            int temporalValue = UnityEngine.Random.Range(0, TemporalList.Count);
            Debug.Log(temporalValue);
            ScriptableObjectComponente Script = TemporalList[temporalValue];

            cartel.cartel.transform.localScale = Script.Scale;
            cartel.material.sprite = Script.ComponentMaterial;

            TemporalList.Remove(Script);
        }
    }

    protected override void personalStart()
    {
        base.personalStart();
        StartCoroutine(CoroutineScreenshot());
    }

    public void SavePhoto()
    {
        pancartaScriptableObject.SaveTexture();
        pancartaScriptableObject.LoadTexture();
    }

    private void Update()
    {

    }

    private IEnumerator CoroutineScreenshot()
    {
        yield return new WaitForEndOfFrame();

        pancartaScriptableObject.SaveTexture();
        pancartaScriptableObject.LoadTexture();
    }
}
