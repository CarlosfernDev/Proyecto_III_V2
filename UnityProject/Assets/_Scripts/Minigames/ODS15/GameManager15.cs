using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager15 : MonoBehaviour
{
    public static GameManager15 Instance { get; private set; }

    //
    public Jurado scoreUI;
    private int puntuacion = 0;
    public GameObject puntuacionUI;
    //PosObjetosHabitat
    public Transform posComida;
    public Transform posHabitat;
    public Transform posDecoracion;

    private GameObject habitatActivoVisual;
    private GameObject comidaActivaVisual;
    private GameObject decoracionActivaVisual;

    [Header("BullshitAssets Settings")]
    //ReferenciasHabitat
    public GameObject Habitat1;
    public GameObject Habitat2;
    public GameObject Habitat3;
    //ReferenciasComida
    public GameObject Comida1;
    public GameObject Comida2;
    public GameObject Comida3;
    //ReferenciasDecoracion
    public GameObject Decoracion1;
    public GameObject Decoracion2;
    public GameObject Decoracion3;


    //CosasActivas
    public int DecoracionActiva = -1;
    public int ComidaActiva = -1;
    public int HabitatActivo = -1;


    //ReferenciasMenus
    public GameObject HabitatMenu;
    public GameObject AlimentacionMenu;
    public GameObject DecoracionMenu;

    [Header("Informacion de animales disponibles")]
    //ObjetoAnimal
    public Animal animalActivo = new Animal();
    public AnimalData[] animales;
    //CanvasSwap
    public GameObject CanvasDialogo;
    public GameObject CanvasGameplay;

    public GameObject imagenUI;
    public GameObject textUI;



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        InputManager.Instance.equipableEvent.AddListener(CanvasSwap);
    }


    private void Start()
    {
        UpdateScore();
        loadNewAnimal();
    }



    private void Update()
    {

        
    }

    public void CheckConditionsButtonCall()
    {
        checkConditionsmet(animalActivo);
    }


    public void MenusSwap(int i)
    {
        switch (i)
        {
            case 0:
                HabitatMenu.SetActive(true);
                AlimentacionMenu.SetActive(false);
                DecoracionMenu.SetActive(false);
                break;
            case 1:
                HabitatMenu.SetActive(false);
                AlimentacionMenu.SetActive(true);
                DecoracionMenu.SetActive(false);
                break;
            case 2:
                HabitatMenu.SetActive(false);
                AlimentacionMenu.SetActive(false);
                DecoracionMenu.SetActive(true);
                break;
            default:
                Debug.LogError("Valor No considerado en FuncionMenuSwap");
                break;
        }
    }

    public void SetComidaActiva(int i)
    {
        ComidaActiva = i;
        Destroy(comidaActivaVisual);
        switch (ComidaActiva)
        {
            case 0:
                comidaActivaVisual = Instantiate(Comida1, posComida.transform.position,Quaternion.identity);
                
                break;
            case 1:
                comidaActivaVisual = Instantiate(Comida2, posComida.transform.position, Quaternion.identity);
                break;
            case 2:
                comidaActivaVisual = Instantiate(Comida3, posComida.transform.position, Quaternion.identity);
                break;
            default:
                break;
        }
    }

    public void SetHabitatActiva(int i)
    {
        HabitatActivo = i;
        Destroy(habitatActivoVisual);
        switch (HabitatActivo)
        {
            case 0:
                habitatActivoVisual = Instantiate(Habitat1, posHabitat.transform.position, Quaternion.identity);
                break;
            case 1:
                habitatActivoVisual = Instantiate(Habitat2, posHabitat.transform.position, Quaternion.identity);
                break;
            case 2:
                habitatActivoVisual = Instantiate(Habitat3, posHabitat.transform.position, Quaternion.identity);
                break;
            default:
                break;
        }

    }

    public void SetDecoracionActiva(int i)
    {
        Debug.Log("DECORACIONCAMBIO");
        DecoracionActiva = i;
        Destroy(decoracionActivaVisual);
        switch (DecoracionActiva)
        {
            case 0:
                decoracionActivaVisual = Instantiate(Decoracion1, posDecoracion.transform.position, Quaternion.identity);
                break;
            case 1:
                decoracionActivaVisual = Instantiate(Decoracion2, posDecoracion.transform.position, Quaternion.identity);
                break;
            case 2:
                decoracionActivaVisual = Instantiate(Decoracion3, posDecoracion.transform.position, Quaternion.identity);
                break;
            default:
                break;
        }

    }



    private void checkConditionsmet(Animal animal)
    {
        if (animal.Comida == ComidaActiva && animal.Habitat == HabitatActivo && animal.Decoracion == DecoracionActiva)
        {
            Debug.Log("win");
            UpdateScore();
            puntuacion += 1;
            puntuacionUI.GetComponent<TMP_Text>().text = puntuacion.ToString();
            Destroy(comidaActivaVisual);
            Destroy(habitatActivoVisual);
            Destroy(decoracionActivaVisual);
            DecoracionActiva = -1;
            ComidaActiva = -1;
            HabitatActivo = -1;
            loadNewAnimal();
        }
        else
        {
            puntuacion -= 1;
            UpdateScore();

        }
    }

    

    void printAnimal(Animal animal)
    {
        Debug.Log("Comida: "+ animal.Comida);
        Debug.Log("Habitat: " + animal.Habitat);
        Debug.Log("Decoracion: " + animal.Decoracion);
    }

    public void CanvasSwap()
    {
        Debug.Log("CANVAS");
        if (CanvasDialogo.activeInHierarchy)
        {
            CanvasDialogo.SetActive(false);
            CanvasGameplay.SetActive(true);
        }
        else
        {
            CanvasDialogo.SetActive(true);
            CanvasGameplay.SetActive(false);
        }
    }

    void loadNewAnimal()
    {
        
        int randomNumber = Random.Range(0, animales.Length);
        animalActivo.Comida = animales[randomNumber].Comida;
        animalActivo.Habitat = animales[randomNumber].Habitat;
        animalActivo.Decoracion = animales[randomNumber].Decoracion;
        imagenUI.GetComponent<Image>().sprite = animales[randomNumber].imagenAnimal;
        textUI.GetComponent<TMP_Text>().text = animales[randomNumber].AnimalDescription;
        printAnimal(animalActivo);
        

    
    }

    public void UpdateScore()
    {
        Debug.Log(puntuacion);
        if (puntuacion<0)
        {
            puntuacion = 0;
        }
        scoreUI.ShowScore(puntuacion);
    }
}


public class Animal
{
    public int Habitat;
    public int Comida;
    public int Decoracion;

    public string AnimalDescription;
    public Sprite imagenAnimal;
}