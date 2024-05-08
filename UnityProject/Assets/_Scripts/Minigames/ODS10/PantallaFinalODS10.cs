using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PantallaFinalODS10 : MonoBehaviour
{
    public Animator _animator;
    public GameObject ObjetosFinales;
    public string StarAnimation;

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
    }
}
