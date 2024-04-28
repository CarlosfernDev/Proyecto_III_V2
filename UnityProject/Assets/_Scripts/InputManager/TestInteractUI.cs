using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestInteractUI : MonoBehaviour
{
    private TMP_Text m_TextComponent;

    private void Awake()
    {
        m_TextComponent = GetComponent<TMP_Text>();
    }
    public void ActualizarTexto(string texto)
    {
        m_TextComponent.text = texto;
    }
    public void EsconderTexto()
    {
        m_TextComponent.text = "";
    }
}
