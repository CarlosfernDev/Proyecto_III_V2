using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreText : MonoBehaviour
{
    [SerializeField] public string Pretext;
    [SerializeField] public string Preset;
    [SerializeField] private TMP_Text Text;

    private void Awake()
    {
        Text.text = null;
    }

    public void ChangeText(int Value)
    {
        Text.text = Pretext + Value.ToString(Preset);
    }

    public void ChangeText(string Value)
    {
        Text.text = Pretext + Value;
    }
}
