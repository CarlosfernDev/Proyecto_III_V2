using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_SelectionInteractuable : SelectionInteractableParent, ISelectionInteractable
{
    [HideInInspector] public Vector2 oldScale;
    public UnityEngine.UI.Outline EnableOutline;
    public Transform transformToAffect;

    [Header("Hover")]

    public bool IsHoverAnimated;
    [SerializeField]
    private AnimationCurve AnimationHoverCurve = new AnimationCurve(
        new Keyframe(0f, 0f),
        new Keyframe(0.25f, 1f),
        new Keyframe(1f, 0f)
    );
    [SerializeField, Range(0, 1f)] private float animationHoverDuration = 0.25f;
    public Vector2 ScaleUp;

    [Header("UnHover")]

    [SerializeField]
    public bool IsUnhoverAnimated;
    [SerializeField]
    private AnimationCurve AnimationUnhoverCurve = new AnimationCurve(
        new Keyframe(0f, 0f),
        new Keyframe(0.25f, 1f),
        new Keyframe(1f, 0f)
    );
    [SerializeField, Range(0, 1f)] private float animationUnhoverDuration = 0.25f;
    public Vector2 ScaleDown;

    private void Start()
    {
        oldScale = transformToAffect.localScale;
    }

    public override void Hover()
    {
        StopAllCoroutines();
        if(EnableOutline != null) EnableOutline.enabled = true;

        if(IsHoverAnimated) StartCoroutine(ScaleUPAnimation());

        base.Hover();
    }

    public override void UnHover()
    {
        StopAllCoroutines();
        if (EnableOutline != null) EnableOutline.enabled = false;

        if (IsHoverAnimated) StartCoroutine(ScaleDownAnimation());
        else transformToAffect.localScale = oldScale;

        base.UnHover();
    }


    IEnumerator ScaleUPAnimation()
    {
        float elapsedTime = 0;

        while (elapsedTime < animationHoverDuration)
        {
            elapsedTime += Time.deltaTime;

            float curvePosition = elapsedTime / animationHoverDuration;

            float curveValue = AnimationHoverCurve.Evaluate(curvePosition);
            Vector2 remappedValue = new Vector2(oldScale.x + (curveValue * (ScaleUp.x)), oldScale.y + (curveValue * (ScaleUp.y)));
            transformToAffect.localScale = remappedValue;
            yield return null;
        }
    }

    IEnumerator ScaleDownAnimation()
    {
        float elapsedTime = 0;

        while (elapsedTime < animationUnhoverDuration)
        {
            elapsedTime += Time.deltaTime;

            float curvePosition = elapsedTime / animationUnhoverDuration;

            float curveValue = AnimationUnhoverCurve.Evaluate(curvePosition);
            Vector2 remappedValue = new Vector2(oldScale.x + (curveValue * (ScaleDown.x)), oldScale.y + (curveValue * (ScaleDown.y)));
            transformToAffect.localScale = remappedValue;
            yield return null;
        }
    }
}
