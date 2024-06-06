using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerShaderTest : MonoBehaviour
{
    //Este es el material que aloja el shader de las letras, está en la carpeta de Shader Materials y 
    //se llama M_PsjLetras
    [SerializeField] private Material _material;

    //Estas son las texturas
    [SerializeField] private Texture2D _baseTexture;
    [SerializeField] private Texture2D _newTexture;

    //Estos son los colores, es importante el tema del hdr, si no está activo pierde el bloom
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
                //En estas dos líneas es donde se cambian las variables del shader, es muy importante que lo del string
                //esté bien escrito, sino no lo reconoce
                _material.SetTexture("_MainTex", _newTexture);
                _material.SetColor("_Color", _newColor);

                _isChanged = true;
            }
            else
            {
                //En estas líneas igual xD
                _material.SetTexture("_MainTex", _baseTexture);
                _material.SetColor("_Color", _baseColor);


                _isChanged = false;
            }
        }
    }
}
