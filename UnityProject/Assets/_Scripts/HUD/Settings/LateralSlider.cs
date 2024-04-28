using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[Serializable]
public class SettingData
{
    public string Name;
    public UnityEvent ValueAction = new();
}

public class LateralSlider : Selectable
{
    public List<SettingData> TextSettings = new();
    [SerializeField] TMP_Text m_text;
    public int IndexState = 0;

    [Header("UI Elements")]
    [SerializeField] private Button LButton;
    [SerializeField] private Button RButton;

    private void Start()
    {
        if (TextSettings.Count - 1 < IndexState)
            return;
        UpdateText();
    }

    public void NextAction(int i)
    {
        IndexState = IndexState + i;

        if (IndexState > TextSettings.Count - 1)
            IndexState = 0;
        else if(IndexState < 0)
            IndexState = TextSettings.Count - 1;

        UpdateText();

        ApplyChange();
    }

    public void UpdateValue() 
    {
        if (IndexState > TextSettings.Count - 1)
            IndexState = 0;
        else if (IndexState < 0)
            IndexState = TextSettings.Count - 1;

        UpdateText();
    }

    void ApplyChange()
    {
        if(TextSettings[IndexState].ValueAction == null)
        {
            Debug.LogWarning("Este ajuste no se puede aplicar por que le falta una accion");
            return;
        }

        TextSettings[IndexState].ValueAction.Invoke();
    }

    public virtual void UpdateText()
    {
        m_text.text = TextSettings[IndexState].Name;
    }

    public override void OnMove(AxisEventData eventData)
    {
        NextAction(Mathf.RoundToInt(eventData.moveVector.x));
        base.OnMove(eventData);
    }

}
