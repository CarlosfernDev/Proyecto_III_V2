using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private void OnEnable()
    {
        InputManager.Instance.anyKeyEvent.AddListener(SetPressedButton);
    }

    private void OnDisable()
    {
        InputManager.Instance.anyKeyEvent.RemoveListener(SetPressedButton);
    }

    public void SetPressedButton()
    {
        if (MySceneManager.Instance.isLoading)
            return;

        MySceneManager.Instance.NextScene(10, 1, 1, 0);
        this.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
