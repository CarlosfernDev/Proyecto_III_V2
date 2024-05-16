using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager15 : MonoBehaviour
{
    public static GameManager15 Instance { get; private set; }

    public PunteroScriptLaura puntero;

    public int contAnimal = 0;

    public Jurado scoreUI;
    private int puntuacion = 0;
    public GameObject puntuacionUI;
    //PosObjetosHabitat
    public Transform posComida;
    public Transform posHabitat;
    public Transform posDecoracion;
    public Transform posAnimal;

    private GameObject habitatActivoVisual;
    private GameObject comidaActivaVisual;
    private GameObject decoracionActivaVisual;
    private GameObject animalActivaVisual;

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
    public GameObject Decoracion4;
    public GameObject Decoracion5;
    public GameObject Decoracion6;


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
    public GameObject nameUI;
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
        puntuacion = 0;

        UpdateScore();
        loadNewAnimal();
        InputManager.Instance.interactEvent.AddListener(disableCanvas);
    }



    private void Update()
    {

        
    }

    public void CheckConditionsButtonCall()
    {
        if (CanvasDialogo.activeInHierarchy)
        {
            return;
        }
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
                //Desactiva todos los menus
                Debug.Log("MenuReset");
                HabitatMenu.SetActive(false);
                AlimentacionMenu.SetActive(false);
                DecoracionMenu.SetActive(false);
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
                comidaActivaVisual = Instantiate(Comida1, posComida.transform.position, posComida.transform.rotation);
                break;
            case 1:
                comidaActivaVisual = Instantiate(Comida2, posComida.transform.position, posComida.transform.rotation);
                break;
            case 2:
                comidaActivaVisual = Instantiate(Comida3, posComida.transform.position, posComida.transform.rotation);
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
                habitatActivoVisual = Instantiate(Habitat1, posHabitat.transform.position,posHabitat.transform.rotation);
                break;
            case 1:
                habitatActivoVisual = Instantiate(Habitat2, posHabitat.transform.position, posHabitat.transform.rotation);
                break;
            case 2:
                habitatActivoVisual = Instantiate(Habitat3, posHabitat.transform.position, posHabitat.transform.rotation);
                break;
            default:
                break;
        }

    }

    public void SetDecoracionActiva(int i)
    {
        Debug.Log("DECORACIONCAMBIO"+i);
        DecoracionActiva = i;
        Destroy(decoracionActivaVisual);
        switch (DecoracionActiva)
        {
            case 0:
                decoracionActivaVisual = Instantiate(Decoracion1, posDecoracion.transform.position, posDecoracion.transform.rotation);
                break;
            case 1:
                decoracionActivaVisual = Instantiate(Decoracion2, posDecoracion.transform.position, posDecoracion.transform.rotation);
                break;
            case 2:
                decoracionActivaVisual = Instantiate(Decoracion3, posDecoracion.transform.position, posDecoracion.transform.rotation);
                break;
            case 3:
                decoracionActivaVisual = Instantiate(Decoracion4, posDecoracion.transform.position, posDecoracion.transform.rotation);
                break;
            default:
                break;
        }

    }



    private void checkConditionsmet(Animal animal)
    {
        if (animal.Comida == ComidaActiva && animal.Habitat == HabitatActivo && animal.Decoracion == DecoracionActiva)
        {
            Debug.Log(puntuacion + " Puntos");
            Debug.Log("RespuestaAceptada");
            puntuacion += 1;
            UpdateScore();
            
            puntuacionUI.GetComponent<TMP_Text>().text = puntuacion.ToString();

            
            Destroy(comidaActivaVisual);
            Destroy(habitatActivoVisual);
            Destroy(decoracionActivaVisual);
            DecoracionActiva = -1;
            ComidaActiva = -1;
            HabitatActivo = -1;
            if (puntuacion >= 5)
            {
                
                Gano();
            }
            else
            {
                loadNewAnimal();
                CanvasSwap();
                MenusSwap(3);
            }
            

        }
        else
        {
            Debug.Log("RespuestaErronea");
            puntuacion -= 1;
            
            UpdateScore();
            loadNewAnimal();
            CanvasSwap();

        }
    }

    

    void printAnimal(Animal animal)
    {
        Debug.Log("Habitat: " + ((int)animal.Habitat + 1));
        Debug.Log("Comida: "+ ((int)animal.Comida+1));
        Debug.Log("Decoracion: " + ((int)animal.Decoracion+1));
    }

    public void CanvasSwap()
    {
        Debug.Log("CANVAS");
        if (CanvasDialogo.activeInHierarchy)
        {
            disableCanvas();
        }
        else
        {
            CanvasDialogo.SetActive(true);
            CanvasGameplay.SetActive(false);
            InputManager.Instance.interactEvent.AddListener(disableCanvas);
        }
    }

    public void disableCanvas()
    {
        if ((MySceneManager.Instance != null ? MySceneManager.Instance.isLoading : false) || (GameManager.Instance != null ? GameManager.Instance.isPaused : false)) return;
        CanvasDialogo.SetActive(false);
        CanvasGameplay.SetActive(true);
        InputManager.Instance.interactEvent.RemoveListener(disableCanvas);
    }

    void loadNewAnimal()
    {
        if (contAnimal >= 5)
        {
            Gano();
            return;
        }
        if (animalActivaVisual != null)
        {
            Destroy(animalActivaVisual);
        }
        animalActivaVisual = Instantiate(animales[contAnimal].animalGO, posAnimal.transform.position,posAnimal.transform.rotation);
        animalActivo.Comida = animales[contAnimal].Comida;
        animalActivo.Habitat = animales[contAnimal].Habitat;
        animalActivo.Decoracion = animales[contAnimal].Decoracion;
        imagenUI.GetComponent<Image>().sprite = animales[contAnimal].imagenAnimal;
        textUI.GetComponent<TMP_Text>().text = animales[contAnimal].AnimalDescription;
        nameUI.GetComponent<TMP_Text>().text = animales[contAnimal].Name;
        contAnimal += 1;
        printAnimal(animalActivo);
        

    
    }

    public void UpdateScore()
    {
        if (puntuacion<0)
        {
            puntuacion = 0;
        }
        if (puntuacion<5)
        {
            scoreUI.ShowScore(puntuacion);
        }
        else
        {
            Debug.Log("No new image");
        }
        
        Debug.Log(puntuacion);
    }

    public void Gano()
    {
        //Llamar funcion enseñar score? o ganar directamente idk man
        Debug.Log("YOU WON");
        puntero.disableMovement = true;
        ODS15MinigameManager.instance.ScoreToSave = puntuacion;
        ODS15MinigameManager.instance.EnseñarPantallaFinal();
    }
}


public class Animal
{
    public int Habitat;
    public int Comida;
    public int Decoracion;

    public string AnimalName;
    public string AnimalDescription;
    public Sprite imagenAnimal;
}