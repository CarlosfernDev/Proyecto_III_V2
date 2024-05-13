using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarPortal : MonoBehaviour
{
    public StarScriptableObject starsprite;
    public MinigamesScriptableObjectScript MinigameData;

    public Image Star1;
    public Image Star2;

    private void Start()
    {
        Debug.Log(MinigameData.CheckPointsState(MinigameData.maxPoints));

        switch (MinigameData.CheckPointsState(MinigameData.maxPoints))
        {
            case 0:
                Star1.sprite = starsprite.Off;
                Star2.sprite = starsprite.Off;
                break;

            case 1:
                Star1.sprite = starsprite.On;
                Star2.sprite = starsprite.Off;
                break;

            case 2:
                Star1.sprite = starsprite.On;
                Star2.sprite = starsprite.On;
                break;
        }
    }
}
