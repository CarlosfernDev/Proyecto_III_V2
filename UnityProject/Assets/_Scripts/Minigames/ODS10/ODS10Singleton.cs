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
        ExecuteGame(0);
    }

    public void ExecuteGame(int Value)
    {
        List<ScriptableObjectComponente> TemporalList =  new List<ScriptableObjectComponente>(scriptables[Value].ListaComponentes);

        ListValue = Value;

        foreach(InGameComponentCartel cartel in Carteles)
        {
            int temporalValue = UnityEngine.Random.Range(0, TemporalList.Count);
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
        scriptables[ListValue].SaveTexture();
        scriptables[ListValue].LoadTexture();
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
