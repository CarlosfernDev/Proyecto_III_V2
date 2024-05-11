using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstScene : MonoBehaviour
{
    public float startDurationLerp;
    [SerializeField]
    private AnimationCurve AnimationStartCurve = new AnimationCurve(
    new Keyframe(0f, 0f),
    new Keyframe(0.25f, 1f),
    new Keyframe(1f, 0f)
);
    public float waitDuration;
    public float endDurationLerp;
    public Image endFade;
    [SerializeField]
    private AnimationCurve AnimationEndCurve = new AnimationCurve(
    new Keyframe(0f, 0f),
    new Keyframe(0.25f, 1f),
    new Keyframe(1f, 0f)
);

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
        MySceneManager.Instance.NextScene(1, 1, 1, 3);
    }

    IEnumerator startingAnimation()
    {
        float journey = 0;

        InputManager.Instance.anyKeyEvent.AddListener(SetPressedButton);

        while (journey <= startDurationLerp)
        {
            journey = journey + Time.deltaTime;

            float percent = Mathf.Clamp01(journey / startDurationLerp);

            float curveValue = AnimationStartCurve.Evaluate(percent);

            if(endFade.color.a != 1)
                endFade.color = new Color (endFade.color.r, endFade.color.g, endFade.color.b, 1 - curveValue);

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

        if (isAnyKeyDown)
        {
            InputManager.Instance.anyKeyEvent.RemoveListener(SetPressedButton);
            NextScene();
            StopCoroutine(_coroutine);
        }

        while (journey <= endDurationLerp)
        {
            journey = journey + Time.deltaTime;

            float percent = Mathf.Clamp01(journey / endDurationLerp);

            float curveValue = AnimationEndCurve.Evaluate(percent);

            if (endFade.color.a != 0)
                endFade.color = new Color(endFade.color.r, endFade.color.g, endFade.color.b, (curveValue));

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
