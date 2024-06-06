using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerShaderTest : MonoBehaviour
{
    //Este es el material que aloja el shader de las letras, est� en la carpeta de Shader Materials y 
    //se llama M_PsjLetras
    [SerializeField] private Material _material;

    //Estas son las texturas
    [SerializeField] private Texture2D _baseTexture;
    [SerializeField] private Texture2D _newTexture;

    //Estos son los colores, es importante el tema del hdr, si no est� activo pierde el bloom
    [ColorUsage(true, hdr: true)]
    [SerializeField] private Color _baseColor;

    [ColorUsage(true, hdr: true)]
    [SerializeField] private Color _newColor;

    //Ni caso, esta es una variable para testear en el script
    [SerializeField] private bool _isChanged = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!_isChanged)
            {
                //En estas dos l�neas es donde se cambian las variables del shader, es muy importante que lo del string
                //est� bien escrito, sino no lo reconoce
                _material.SetTexture("_MainTex", _newTexture);
                _material.SetColor("_Color", _newColor);

                _isChanged = true;
            }
            else
            {
                //En estas l�neas igual xD
                _material.SetTexture("_MainTex", _baseTexture);
                _material.SetColor("_Color", _baseColor);


                _isChanged = false;
            }
        }
    }
}
