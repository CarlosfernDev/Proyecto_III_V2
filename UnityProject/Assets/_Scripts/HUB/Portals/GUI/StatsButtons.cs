using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsButtons : MonoBehaviour
{
    [SerializeField] private Image _button;

    public void Retry()
    {
        GameManager.Instance.eventSystem.SetSelectedGameObject(null);
        MySceneManager.Instance.RestartScene();
    }

    public void Hub()
    {
        GameManager.Instance.eventSystem.SetSelectedGameObject(null);
        MySceneManager.Instance.NextScene(100, 1, 1, 0);
    }

    public void EnableButtons()
    {
        GameManager.Instance.eventSystem.SetSelectedGameObject(_button.gameObject);
    }
}
