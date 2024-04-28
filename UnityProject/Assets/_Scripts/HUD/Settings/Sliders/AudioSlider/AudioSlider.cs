using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSlider : LateralSlider
{
    [Header("AudioSlider")]
    [SerializeField] private Slider _slider;
    [SerializeField] private string loadValue;

    // Start is called before the first frame update
    void Start()    
    {
        _slider.maxValue = TextSettings.Count - 1;

        float Volume = PlayerPrefs.GetFloat(loadValue, 0);
        float Count = -80f / (TextSettings.Count - 1);

        for (int i = 0; i < TextSettings.Count; i++)
        {
            if (-80f - (i * Count) < Volume)
                continue;

            IndexState = i;
            break;
        }
        UpdateText();
    }

    public override void UpdateText()
    {
        _slider.value = IndexState;
        base.UpdateText();
    }
}
