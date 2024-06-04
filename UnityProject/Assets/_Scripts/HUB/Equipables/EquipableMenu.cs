using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using Unity.VisualScripting;

[Serializable]
public class EquipableMenuInfo
{
    public CustomEquipableButton Button;
    public GameObject MenuCanvas;
    public List<Button> ButtonList;
}

public class EquipableMenu : MonoBehaviour
{
    public GameObject Canvas;

    public bool PauseGame = true;

    [SerializeField] private TMP_Text _Text;
    [SerializeField] private Image _button;
    [SerializeField] private Sprite _EnableSprite;
    [SerializeField] private Sprite _DisableSprite;

    public List<EquipableMenuInfo> ItemMenuList;
    private GameObject ActualMenu;

    private void Awake()
    {
        int id = 0;
        foreach(EquipableMenuInfo menu in ItemMenuList)
        {
            int value = id;
            menu.Button.OnSelected += () => SetMenu(value);
            id++;
        }
    }

    public void OpenMenu()
    {
        if (GameManager.Instance == null) Debug.LogError("Falta un GameManager (Si otra vez pendejo).");

        GameManager.Instance.playerScript.DisablePlayer();
        GameManager.Instance.playerScript.sloopyMovement = true;
        if (PauseGame) GameManager.Instance.isPaused = true;

        GameManager.Instance.isPlaying = false;

        SetMenu(0);
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
        GameManager.Instance.isPlaying = true;
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

    public void SetMenu(int id)
    {
        Debug.Log(id);
        if(ActualMenu != null)ActualMenu.SetActive(false);
        ActualMenu = ItemMenuList[id].MenuCanvas;

        ActualMenu.SetActive(true);
    }

    public void ChangeSubMenu(int id)
    {
        int value = 0;
        switch (id)
        {
            case 0:
                value = SaveManager.savePlayerData.PlayerItems.Item1;
                break;

            case 1:
                value = SaveManager.savePlayerData.PlayerItems.Item2;
                break;

            case 2:
                value = SaveManager.savePlayerData.PlayerItems.Item3;
                break;
        }
        GameManager.Instance.eventSystem.SetSelectedGameObject(ItemMenuList[id].ButtonList[value].gameObject);
        InputManager.Instance.pauseEvent.RemoveListener(CloseMenu);
        InputManager.Instance.pauseEvent.AddListener(() => CancelSubmenu(id));
    }

    public void CancelSubmenu(int id)
    {
        InputManager.Instance.pauseEvent.AddListener(CloseMenu);
        GameManager.Instance.eventSystem.SetSelectedGameObject(ItemMenuList[id].Button.gameObject);
    }

    public void SetHat(int id)
    {
        UnlockablesManager.instance.SaveHat(id);
    }

    public void SetCape(int id)
    {
        UnlockablesManager.instance.SaveCape(id);
    }

    public void SetPet(int id)
    {
        UnlockablesManager.instance.SavePet(id);
    }
}
