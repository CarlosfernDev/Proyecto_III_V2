using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine.VFX;

[Serializable]
public class InGameComponentCartel
{
    public GameObject cartel;
    public Image material;
}

[Serializable]
public class InGameColor
{
    public GameObject button;
    public Color color;
}

public class ODS10Singleton : MinigameParent
{
    public int ListValue;

    public PunteroScript puntero;

    public Camera PancartaCamera;
    public static ODS10Singleton Instance;
    public TMP_Text _PancartaText;

    public Image CartelImage;
    public List<InGameColor> ColorBackground;
    private int ColorValue = 0;

    public List<PancartaScriptableObject> scriptables;
    public List<InGameComponentCartel> Carteles;
    public Dictionary<GameObject, ScriptableObjectComponente> CartelDictionary;
    public PantallaFinalODS10 pantallafinal;

    [SerializeField] PancartaScriptableObject pancartaScriptableObject;

    [SerializeField] private VisualEffect _cambioColorVFX;

    protected override void personalAwake()
    {
        Instance = this;
        CartelDictionary = new Dictionary<GameObject, ScriptableObjectComponente>();
        base.personalAwake();

        if(MySceneManager.ActualScene >= 50 && MySceneManager.ActualScene < 60)
        {
            ExecuteGame(MySceneManager.ActualScene - 50);
        }
        else
        {
            ExecuteGame();
        }
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

        _PancartaText.text = pancartaScriptableObject._PancartaName;
        ChangeBackground(UnityEngine.Random.Range(0, ColorBackground.Count));

        List<ScriptableObjectComponente> TemporalList = new List<ScriptableObjectComponente>(pancartaScriptableObject.ListaComponentes);

        foreach (InGameComponentCartel cartel in Carteles)
        {
            int temporalValue = UnityEngine.Random.Range(0, TemporalList.Count);
            Debug.Log(temporalValue);
            ScriptableObjectComponente Script = TemporalList[temporalValue];

            CartelDictionary.Add(cartel.cartel.gameObject, Script);
            cartel.cartel.transform.localScale = Script.Scale;
            cartel.material.sprite = Script.ComponentMaterial;

            TemporalList.Remove(Script);
        }

        pantallafinal.ChangeMaterialPancarta(pancartaScriptableObject._PancartaMaterial);
    }

    protected override void OnGameStart()
    {
        base.OnGameStart();
        GameManager.Instance.isPlaying = true;
    }

    protected override void personalStart()
    {
        base.personalStart();
        if (IsDeveloping || !MySceneManager.Instance.isLoading)
        {
            GameManager.Instance.isPlaying = true;
        }
    }

    public void SavePhoto()
    {
        pancartaScriptableObject.SaveTexture();
        pancartaScriptableObject.LoadTexture();
        puntero.enabled = false;
        GameManager.Instance.isPlaying = false;
        pantallafinal.StartEnd();
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

    public void NextColor()
    {
        if (ColorValue + 1 < ColorBackground.Count)
        {
            ChangeBackground(ColorValue + 1);
            return;
        }
        ChangeBackground(0);
    }

    public void ChangeBackground(int value)
    {
        _cambioColorVFX.Play();
        value = Mathf.Clamp(value, 0, ColorBackground.Count - 1);
        CartelImage.color = ColorBackground[value].color;
        ColorValue = value;
    }
}
