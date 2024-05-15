using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditosMove : MonoBehaviour
{
    public Vector2 LastPosition;
    public RectTransform canvasTrasnform;

    public float BaseSpeed;

    public float SkipSpeed;

    public float WaitUntilNextScene;

    bool isFinished;

    private void Update()
    {
        if (MySceneManager.Instance.isLoading) return;

        if (LastPosition.y <= canvasTrasnform.anchoredPosition.y)
        {
            if (!isFinished)
            {
                StartCoroutine(StartNextSceneCoroutine());
                isFinished = true;
            }
            return;
        }

        float speed = BaseSpeed;

        if (InputManager.Instance.playerInputs.ActionMap1.Pausa.IsPressed()) speed = SkipSpeed;
        
        canvasTrasnform.anchoredPosition = new Vector2(canvasTrasnform.anchoredPosition.x, Mathf.Clamp(canvasTrasnform.anchoredPosition.y + Time.deltaTime * speed, canvasTrasnform.anchoredPosition.y, LastPosition.y));
    }

    IEnumerator StartNextSceneCoroutine()
    {
        yield return new WaitForSeconds(WaitUntilNextScene);
        MySceneManager.Instance.NextScene(100, 1, 1, 2);
    }
}
