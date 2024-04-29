using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstScene : MonoBehaviour
{
    public float startDurationLerp;
    public float waitDuration;
    public float endDurationLerp;
    public Image endFade;

    private Coroutine _coroutine;
    private bool isAnyKeyDown = false;

    void Start()
    {
        //UnityEngine.Rendering.DebugManager.instance.enableRuntimeUI = false;
        _coroutine = StartCoroutine(startingAnimation());
    }

    public void SetPressedButton()
    {
        isAnyKeyDown = true;
    }

    private void NextScene()
    {
        MySceneManager.Instance.NextScene(1, 1, 1, 0);
    }

    IEnumerator startingAnimation()
    {
        float journey = 0;

        InputManager.Instance.anyKeyEvent.AddListener(SetPressedButton);

        while (journey <= startDurationLerp)
        {
            journey = journey + Time.deltaTime;
            float percent = Mathf.Clamp01(journey / startDurationLerp);
            float size = Mathf.Lerp(0.93f,1,percent);
            if(endFade.color.a != 1)
                endFade.color = new Color (endFade.color.r, endFade.color.g, endFade.color.b, 1 - (percent * 3.4f));

            if (isAnyKeyDown)
            {
                InputManager.Instance.anyKeyEvent.RemoveListener(SetPressedButton);
                NextScene();
                StopCoroutine(_coroutine);
            }

            yield return new WaitForSeconds(Time.deltaTime);
        }

        yield return new WaitForSeconds(waitDuration);
        journey = 0;

        while (journey <= endDurationLerp)
        {
            journey = journey + Time.deltaTime;
            float percent = Mathf.Clamp01(journey / endDurationLerp);
            if (endFade.color.a != 0)
                endFade.color = new Color(endFade.color.r, endFade.color.g, endFade.color.b, (percent));

            if (isAnyKeyDown)
            {
                InputManager.Instance.anyKeyEvent.RemoveListener(SetPressedButton);
                NextScene();
                StopCoroutine(_coroutine);
            }

            yield return new WaitForSeconds(Time.deltaTime);
        }
        NextScene();
    }
}
