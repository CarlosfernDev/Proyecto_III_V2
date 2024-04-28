using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QualitySlider : LateralSlider
{
    // Start is called before the first frame update
    void Start()
    {
        IndexState = PlayerPrefs.GetInt("qualityIndex", 3);
        UpdateText();
    }
}
