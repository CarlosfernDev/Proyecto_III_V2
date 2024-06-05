using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GarbageScript : LInteractableParent
{
    public enum garbageType { UNDEFINED, PAPER, PLASTIC, GLASS }
    public garbageType thisGarbageType = garbageType.UNDEFINED;

    public bool isCentered;

    private bool isPickedUp;
    private Transform _playerPickupTransform;

    // Referencias a textura y mesh
    [SerializeField] private MeshFilter Mesh3d;
    [SerializeField] private MeshRenderer MeshTexture;
    [SerializeField] private List<ScriptableBasura> _scriptable;

    // Referencia obligatoria para desactivar interaccion con la cinta al interactuar
    [SerializeField] private BoxCollider Collider;
    [SerializeField] private Rigidbody _rb;

    // fvx
    [SerializeField] private VisualEffect PickableVFX;

    private void Awake()
    {
        ScriptableBasura TempScriptable = _scriptable[(int)Random.Range(0, _scriptable.Count)];
        Mesh3d.mesh = TempScriptable.Modelo3D;
        MeshTexture.material = TempScriptable.Textura;
    }

    private void Start()
    {
        if (GameManager.Instance.playerScript.refObjetoEquipado == null)
            enableVFX();

        ODS12Singleton.Instance.PickItemEvent += disableVFX;
        ODS12Singleton.Instance.DropItemEvent += enableVFX;
    }

    private void OnDisable()
    {
        ODS12Singleton.Instance.PickItemEvent -= disableVFX;
        ODS12Singleton.Instance.DropItemEvent -= enableVFX;
    }

    void Update()
    {
        //if (isPickedUp)
        //{
        //    transform.position = _playerPickupTransform.position;
        //}
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

        // Se soluciona el bug donde la icnta da momento al jugador cuando agarras el objeto  
        Collider.enabled = false;
        _rb.useGravity = false;
        _rb.isKinematic = true;

        _rb.freezeRotation = true;

        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;

        PickableVFX.gameObject.SetActive(false);

        isCentered = false;
        SetInteractFalse();
        isPickedUp = true;
        _playerPickupTransform = ODS12Singleton.Instance.playerPickupTransform;
        transform.parent = _playerPickupTransform;
        gameObject.GetComponent<Rigidbody>().freezeRotation = true;

        transform.position = GameManager.Instance.playerScript.positionEquipable.transform.position;
        transform.rotation = GameManager.Instance.playerScript.positionEquipable.rotation;
        //transform.localScale = GameManager.Instance.playerScript.positionEquipable.localScale;
        transform.parent.transform.parent = GameManager.Instance.playerScript.positionEquipable.transform;

        //Necesario para saber que objeto tiene el usuario
        GameManager.Instance.playerScript.refObjetoEquipado = gameObject;
        GameManager.Instance.playerScript.refObjetoInteract = null;
        Unhover();
        ODS12Singleton.Instance.PickItems();
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
