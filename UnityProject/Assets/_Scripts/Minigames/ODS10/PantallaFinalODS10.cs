using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PantallaFinalODS10 : MonoBehaviour
{
    public Animator _animator;
    public GameObject ObjetosFinales;
    public string StarAnimation;
    public PancartaArea _area;

    public MeshRenderer Pancarta; 

    private void Awake()
    {
        ObjetosFinales.SetActive(false);
    }

    public void StartEnd()
    {
        InputManager.Instance.interactEvent.AddListener(FinishGame);
        ObjetosFinales.SetActive(true);
        _animator.SetTrigger(StarAnimation);
    }

    public void ParticlesExplosion()
    {

    }

    public void ChangeMaterialPancarta(Material value)
    {
        Pancarta.material = value;
    }

    public void FinishGame()
    {
        InputManager.Instance.interactEvent.RemoveListener(FinishGame);

        int value = MySceneManager.ActualScene - 50;
        GameManager.Instance.PancartaData[value].Score = _area.Score;
        SaveManager.SavePancarta(value);

        MySceneManager.Instance.NextScene(100, 1, 1, 4);
    }
}
