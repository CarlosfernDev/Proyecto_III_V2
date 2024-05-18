using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarPortal : MonoBehaviour
{
    public StarScriptableObject starsprite;
    public MinigamesScriptableObjectScript MinigameData;

    public Vector3 ScaleStar;
    public Color DisableColor;

    public Image Star1;
    public Image Star2;

    private void Start()
    {
        Debug.Log(MinigameData.CheckPointsState(MinigameData.maxPoints));

        switch (MinigameData.CheckPointsState(MinigameData.maxPoints))
        {
            case 0:
                DisableStar(Star1);
                DisableStar(Star2);
                break;

            case 1:
                EnableStar(Star1);
                DisableStar(Star2);
                Star1.sprite = starsprite.On;
                break;

            case 2:
                EnableStar(Star1);
                EnableStar(Star2);
                break;
        }
    }

    private void DisableStar(Image Star)
    {
        Star.sprite = starsprite.Off;
        Star.color = DisableColor;
        Star.transform.localScale = ScaleStar;
    }

    private void EnableStar(Image Star)
    {
        Star.sprite = starsprite.On;
    }
}
