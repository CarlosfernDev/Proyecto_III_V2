using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VsyncSlider : LateralSlider
{
    // Start is called before the first frame update
    void Start()
    {
        IndexState = PlayerPrefs.GetInt("isVsync", 1);
        UpdateText();
    }
}
