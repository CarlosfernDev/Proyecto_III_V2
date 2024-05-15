using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnEnableEvent : MonoBehaviour
{
    public UnityEvent EventOnEnable;

    private void OnEnable()
    {
        EventOnEnable?.Invoke();
    }
}
