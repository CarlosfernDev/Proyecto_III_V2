using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextSceneEscenaFinal : MonoBehaviour
{
    public void NextScene()
    {
        MySceneManager.Instance.NextScene(103, 1, 1, 0);
    }
}
