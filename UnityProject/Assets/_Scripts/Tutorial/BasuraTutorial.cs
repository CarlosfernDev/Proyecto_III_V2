using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BasuraTutorial : LInteractableParent
{
    public bool isCentered;

    private bool isPickedUp;
    private Transform _playerPickupTransform;

    // fvx
    [SerializeField] private VisualEffect PickableVFX;

    private void Start()
    {
        if (GameManager.Instance.playerScript.refObjetoEquipado == null)
            enableVFX();
    }



    public override void Interact()
    {
        // Solo puedes agarrar una cosa
        if (GameManager.Instance.playerScript.refObjetoEquipado != null)
        {
            Unhover();
            return;
        }

        base.Interact();
        GameManager.Instance.playerScript.isEquipado = true;

        if (AudioManager.Instance != null) AudioManager.Instance.Play("GrabSound");

        PickableVFX.gameObject.SetActive(false);

        isCentered = false;
        SetInteractFalse();
        isPickedUp = true;
        _playerPickupTransform = GameManager.Instance.playerScript.positionEquipable;
        transform.parent = _playerPickupTransform;

        transform.position = GameManager.Instance.playerScript.positionEquipable.transform.position;
        transform.rotation = GameManager.Instance.playerScript.positionEquipable.rotation;
        //transform.localScale = GameManager.Instance.playerScript.positionEquipable.localScale;
        transform.parent.transform.parent = GameManager.Instance.playerScript.positionEquipable.transform;

        //Necesario para saber que objeto tiene el usuario
        GameManager.Instance.playerScript.refObjetoEquipado = gameObject;
        GameManager.Instance.playerScript.refObjetoInteract = null;
        disableVFX();
        TutorialManager.Instance.OnPickItem?.Invoke();
        Unhover();
    }

    // VFX

    public void enableVFX()
    {
        PickableVFX.Play();
    }

    public void disableVFX()
    {
        PickableVFX.Stop();
    }

    public override void Hover()
    {
        if (GameManager.Instance.playerScript.isEquipado) return;
        base.Hover();
    }
}
