using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private string Pretext;
    [SerializeField] private TMP_Text Text;

    private void Awake()
    {
        Text.text = null;
    }

    public void ChangeText(string Value)
    {
        Text.text = Pretext + Value;
    }
}
