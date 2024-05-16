using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;
    public UnityEvent OnPickItem;
    public UnityEvent OnDropItem;

    private void Awake()
    {
        Instance = this;
        MySceneManager.Instance.OnLoadFinish += IsPlayingEnable;
    }

    public void IsPlayingEnable()
    {
        GameManager.Instance.isPlaying = true;
        MySceneManager.Instance.OnLoadFinish -= IsPlayingEnable;
    }
}
