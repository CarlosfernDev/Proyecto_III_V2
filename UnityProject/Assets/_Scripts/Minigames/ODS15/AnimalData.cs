using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Animal", menuName = "ScriptableObjects/AnimalData", order = 1)]
public class AnimalData : ScriptableObject
{
    public int Habitat;
    public int Comida;
    public int Decoracion;

    public string AnimalDescription;
    public Sprite imagenAnimal;
}
