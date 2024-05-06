using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class GUIAreUSure : MonoBehaviour
{
    [SerializeField] private GameObject _button;
    [SerializeField] private GameObject _Canvas;

    public Action _FunctionOnYes;

    public void EnableMenu()
    {
        if (GameManager.Instance == null) Debug.LogError("Falta un GameManager (Si otra vez pendejo).");


        GameManager.Instance.isPaused = true;
        InputManager.Instance.pauseEvent.AddListener(No);

        _Canvas.SetActive(true);
        GameManager.Instance.eventSystem.SetSelectedGameObject(_button);
    }

    public void DisableMenu()
    {
        GameManager.Instance.isPaused = false;
        InputManager.Instance.pauseEvent.RemoveListener(No);
        _Canvas.SetActive(false);
    }

    public void Yes()
    {
        _FunctionOnYes?.Invoke();
    }

    public void No()
    {
        DisableMenu();
    }
}
