using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class CustomEquipableButton : Button
{
    public Action OnSelected;

    public override void OnSelect(BaseEventData eventData)
    {
        Debug.Log("Test");
        OnSelected?.Invoke();
        base.OnSelect(eventData);
    }
}
