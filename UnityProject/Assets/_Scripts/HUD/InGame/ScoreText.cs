using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreText : MonoBehaviour
{
    [SerializeField] private string Pretext;
    [SerializeField] private TMP_Text Text;

    private void Awake()
    {
        Text.text = null;
    }

    public void ChangeText(int Value)
    {
        Text.text = Pretext + Value.ToString("000000");
    }

    public void ChangeText(string Value)
    {
        Text.text = Pretext + Value;
    }
}
