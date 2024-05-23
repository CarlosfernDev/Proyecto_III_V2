using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class EquipableMenu : MonoBehaviour
{
    public GameObject Canvas;

    public bool PauseGame = true;

    [SerializeField] private TMP_Text _Text;
    [SerializeField] private Image _button;
    [SerializeField] private Sprite _EnableSprite;
    [SerializeField] private Sprite _DisableSprite;

    public void OpenMenu()
    {
        if (GameManager.Instance == null) Debug.LogError("Falta un GameManager (Si otra vez pendejo).");

        GameManager.Instance.playerScript.DisablePlayer();
        GameManager.Instance.playerScript.sloopyMovement = true;
        if (PauseGame) GameManager.Instance.isPaused = true;

        Canvas.SetActive(true);
        StartCoroutine(Wait1SecondEnable());
    }

    IEnumerator Wait1SecondEnable()
    {
        GameManager.Instance.eventSystem.SetSelectedGameObject(null);
        _button.sprite = _EnableSprite;

        while (InputManager.Instance.playerInputs.ActionMap1.Movement.ReadValue<Vector2>().x != 0f)
        {
            yield return new WaitForNextFrameUnit();
        }
        yield return new WaitForNextFrameUnit();

        _button.sprite = _DisableSprite;
        InputManager.Instance.pauseEvent.AddListener(CloseMenu);
        GameManager.Instance.eventSystem.SetSelectedGameObject(_button.gameObject);
    }

    public void CloseMenu()
    {
        GameManager.Instance.eventSystem.SetSelectedGameObject(null);
        InputManager.Instance.pauseEvent.RemoveListener(CloseMenu);
        Canvas.SetActive(false);

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
        if (PauseGame) GameManager.Instance.isPaused = false;
    }
}
