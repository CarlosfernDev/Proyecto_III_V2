using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Image_SelectionInteractable : SelectionInteractableParent
{
    public Image _image;

    [Header("Hover")]
    public Sprite _ImgHover;

    [SerializeField]
    private AnimationCurve AnimationCurve = new AnimationCurve(
        new Keyframe(0f, 0f),
        new Keyframe(0.25f, 1f),
        new Keyframe(1f, 0f)
    );
    [SerializeField, Range(0, 1f)] private float animationDuration = 0.25f;

    public Transform transformToAffect;
    public Vector2 ScaleUp;
    private Vector2 oldScale;

    [Header("UnHover")]
    public Sprite _ImgUnhover;

    [Header("Select")]
    public Sprite _ImgSelect;

    private void Start()
    {
        oldScale = transformToAffect.localScale;
        _image.sprite = _ImgUnhover;
    }

    public override void Hover()
    {
        _image.sprite = _ImgHover;

        StartCoroutine(ScaleAnimation());

        base.Hover();
    }

    public override void UnHover()
    {
        transformToAffect.localScale = oldScale;
        _image.sprite = _ImgUnhover;
        base.UnHover();
    }


    IEnumerator ScaleAnimation()
    {
        float elapsedTime = 0;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;

            float curvePosition = elapsedTime / animationDuration;

            float curveValue = AnimationCurve.Evaluate(curvePosition);
            Vector2 remappedValue = new Vector2 ( oldScale.x + (curveValue * (ScaleUp.x)), oldScale.y + (curveValue * (ScaleUp.y)));
            transformToAffect.localScale = remappedValue;
            yield return null;
        }
    }
}
