using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIswapTest : MonoBehaviour
{
    [SerializeField] private UItype selecImage;
    [SerializeField] private SpriteRenderer Sren;
    [SerializeField] private ScriptableUI KeyBoardImage;
    [SerializeField] private ScriptableUI GamepadImage;
    
    void OnEnable()
    {
        try
        {
            InputManager.Instance.ChangeUIto.AddListener(checkEnum);
        }
        catch (System.Exception e)
        {
            Debug.Log(e.ToString());
            Debug.LogWarning("No se pudo asignar los eventos, probablemente te faltara un InputManager");
        }
        

    }

    void OnDisable()
    {
        InputManager.Instance.ChangeUIto.RemoveListener(checkEnum);
    }

    

    

    void checkEnum(string type)
    {
        if (type == "Keyboard")
        {
            switch (selecImage)
            {
                case UItype.Movement:
                    {
                        Sren.sprite = KeyBoardImage.Movement;
                        break;
                    }
                case UItype.Interact:
                    {
                        Sren.sprite = KeyBoardImage.Interact;
                        break;
                    }
                case UItype.Pausa:
                    {
                        Sren.sprite = KeyBoardImage.Pausa;
                        break;
                    }
                case UItype.UsarEquipable:
                    {
                        Sren.sprite = KeyBoardImage.UsarEquipable;
                        break;
                    }
                case UItype.RotarPieza:
                    {
                        Sren.sprite = KeyBoardImage.RotarPieza;
                        break;
                    }
                case UItype.AnyKey:
                    {
                        Sren.sprite = KeyBoardImage.AnyKey;
                        break;
                    }
                default:
                    Debug.LogError("VALOR ENUM ERRONEO");
                    break;

            }
        }

        if (type == "Gamepad")
        {
            switch (selecImage)
            {
                case UItype.Movement:
                    {
                        Sren.sprite = GamepadImage.Movement;
                        break;
                    }
                case UItype.Interact:
                    {
                        Sren.sprite = GamepadImage.Interact;
                        break;
                    }
                case UItype.Pausa:
                    {
                        Sren.sprite = GamepadImage.Pausa;
                        break;
                    }
                case UItype.UsarEquipable:
                    {
                        Sren.sprite = GamepadImage.UsarEquipable;
                        break;
                    }
                case UItype.RotarPieza:
                    {
                        Sren.sprite = GamepadImage.RotarPieza;
                        break;
                    }
                case UItype.AnyKey:
                    {
                        Sren.sprite = GamepadImage.AnyKey;
                        break;
                    }
                default:
                    Debug.LogError("VALOR ENUM ERRONEO");
                    break;

            }
        }

    }
}

enum UItype
{
    Movement,
    Interact,
    Pausa,
    UsarEquipable,
    RotarPieza,
    AnyKey
}
