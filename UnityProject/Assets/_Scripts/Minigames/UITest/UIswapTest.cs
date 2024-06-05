using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIswapTest : MonoBehaviour
{
    [SerializeField] public UItype selecImage;
    [SerializeField] private ScriptableUI KeyBoardImage;
    [SerializeField] private ScriptableUI GamepadImage;
    [SerializeField] private Animator AnimatorUI;
    void OnEnable()
    {
        transform.localScale = new Vector3(1, 1, 1);
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
                        AnimatorUI.runtimeAnimatorController = KeyBoardImage.Movement.Animator; 
                        break;
                    }
                case UItype.Interact:
                    {
                        AnimatorUI.runtimeAnimatorController = KeyBoardImage.Interact.Animator;
                        break;
                    }
                case UItype.Pausa:
                    {
                        AnimatorUI.runtimeAnimatorController = KeyBoardImage.Pausa.Animator;
                        break;
                    }
                case UItype.UsarEquipable:
                    {
                        AnimatorUI.runtimeAnimatorController = KeyBoardImage.UsarEquipable.Animator;
                        break;
                    }
                case UItype.RotarPieza:
                    {
                        AnimatorUI.runtimeAnimatorController = KeyBoardImage.RotarPieza.Animator;
                        break;
                    }
                case UItype.AnyKey:
                    {
                        AnimatorUI.runtimeAnimatorController = KeyBoardImage.AnyKey.Animator; 
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
                        AnimatorUI.runtimeAnimatorController = GamepadImage.Movement.Animator;
                        break;
                    }
                case UItype.Interact:
                    {
                        AnimatorUI.runtimeAnimatorController = GamepadImage.Interact.Animator;
                        break;
                    }
                case UItype.Pausa:
                    {
                        AnimatorUI.runtimeAnimatorController = GamepadImage.Pausa.Animator;

                        break;
                    }
                case UItype.UsarEquipable:
                    {
                        AnimatorUI.runtimeAnimatorController = GamepadImage.UsarEquipable.Animator;

                        break;
                    }
                case UItype.RotarPieza:
                    {
                        AnimatorUI.runtimeAnimatorController = GamepadImage.RotarPieza.Animator;

                        break;
                    }
                case UItype.AnyKey:
                    {
                        AnimatorUI.runtimeAnimatorController = GamepadImage.AnyKey.Animator;

                        break;
                    }
                default:
                    Debug.LogError("VALOR ENUM ERRONEO");
                    break;

            }
        }

    }
}

public enum UItype
{
    Movement,
    Interact,
    Pausa,
    UsarEquipable,
    RotarPieza,
    AnyKey
}
