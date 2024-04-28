using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullscreenSlider : LateralSlider
{
    // Start is called before the first frame update
    void Start()
    {
        IndexState = PlayerPrefs.GetInt("isFullscreen", 1);
        UpdateText();
    }
}
