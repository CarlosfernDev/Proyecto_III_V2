using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageScript : LInteractableParent
{
    public enum garbageType { UNDEFINED, PAPER, PLASTIC, GLASS }
    public garbageType thisGarbageType = garbageType.UNDEFINED;

    public bool isCentered;

    private bool isPickedUp;
    private Transform _playerPickupTransform;
    
    void Update()
    {
        if (isPickedUp)
        {
            transform.position = _playerPickupTransform.position;
        }
    }

    public override void Interact()
    {
        base.Interact();
        isCentered = false;
        SetInteractFalse();
        isPickedUp = true;
        _playerPickupTransform = ODS12Singleton.Instance.playerPickupTransform;
        transform.parent = _playerPickupTransform;
        gameObject.GetComponent<Rigidbody>().freezeRotation = true;
    }
}
