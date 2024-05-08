using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class GUIAreUSure : MonoBehaviour
{
    [SerializeField] private Image _button;
    [SerializeField] private GameObject _Canvas;

    [SerializeField] private Sprite _EnableSprite;
    [SerializeField] private Sprite _DisableSprite;

    public Action _FunctionOnYes;

    public void EnableMenu()
    {
        if (GameManager.Instance == null) Debug.LogError("Falta un GameManager (Si otra vez pendejo).");

        GameManager.Instance.playerScript.DisablePlayer();
        GameManager.Instance.playerScript.sloopyMovement = true;
        GameManager.Instance.isPaused = true;
        _Canvas.SetActive(true);

        StartCoroutine(Wait1SecondEnable());
    }
    
    IEnumerator Wait1SecondEnable()
    {
        GameManager.Instance.eventSystem.SetSelectedGameObject(null);
        _button.sprite = _EnableSprite;

        while (InputManager.Instance.playerInputs.ActionMap1.Movement.ReadValue<Vector2>().x != 0f) {
            yield return new WaitForNextFrameUnit();
        }
        yield return new WaitForNextFrameUnit();

        _button.sprite = _DisableSprite;
        InputManager.Instance.pauseEvent.AddListener(No);
        GameManager.Instance.eventSystem.SetSelectedGameObject(_button.gameObject);
    }

    public void DisableMenu()
    {
        GameManager.Instance.eventSystem.SetSelectedGameObject(null);
        InputManager.Instance.pauseEvent.RemoveListener(No);
        _Canvas.SetActive(false);

        StartCoroutine(Wait1SecondDisable());
    }

    IEnumerator Wait1SecondDisable()
    {
        GameManager.Instance.eventSystem.SetSelectedGameObject(null);

        while (InputManager.Instance.playerInputs.ActionMap1.Interact.IsPressed())
        {
            yield return new WaitForNextFrameUnit();
        }
        yield return new WaitForNextFrameUnit();
        GameManager.Instance.isPaused = false;
    }

    public void Yes()
    {
        GameManager.Instance.isPaused = false;
        _FunctionOnYes?.Invoke();
    }

    public void No()
    {
        DisableMenu();
    }
}
