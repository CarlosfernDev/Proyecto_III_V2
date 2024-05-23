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
        checkEnum(InputManager.Instance.LastInputName);
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
                        
                        break;
                    }
                case UItype.Interact:
                    {
                        
                        break;
                    }
                case UItype.Pausa:
                    {
                        break;
                    }
                case UItype.UsarEquipable:
                    {
                        break;
                    }
                case UItype.RotarPieza:
                    {
                        break;
                    }
                case UItype.AnyKey:
                    {
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
                        break;
                    }
                case UItype.Interact:
                    {
                        break;
                    }
                case UItype.Pausa:
                    {
                        break;
                    }
                case UItype.UsarEquipable:
                    {
                        break;
                    }
                case UItype.RotarPieza:
                    {
                        break;
                    }
                case UItype.AnyKey:
                    {
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
