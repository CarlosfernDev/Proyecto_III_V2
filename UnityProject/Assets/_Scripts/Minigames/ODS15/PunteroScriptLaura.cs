using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(1)]
public class PunteroScriptLaura: MonoBehaviour
{
    [SerializeField] public bool disableMovement = false;
    [SerializeField] private bool IsPointingSomething = false;
    [SerializeField] private GameObject PointedGameobject;

    //Equipment (Red nubes fran)
    [SerializeField] public Transform positionEquipable;
    public bool isEquipado = false;
    public GameObject refObjetoEquipado;
    public bool isEquipableInCooldown;

    //Interaction
    [SerializeField] public GameObject interactZone;
    public GameObject refObjetoInteract;
    private bool isInteractable = false;

    [SerializeField] public float speed;

    //Actualizador de UI? maybe hay que moverlo a los scripts interactuables y hacer que los objetos busquen la ui en la escena
    [SerializeField] private UnityEvent hideText;
    [SerializeField] private UnityEvent<string> TextoInteractChange;
    //Modificacion de la clase event para poder pasar en las llamadas strings

    [SerializeField] public SpriteRenderer imVisible;
    private Vector3 poInicial;
    [System.Serializable]
    public class MyStringEvent : UnityEvent<string>
    {
    }

    public void Awake()
    {


    }

    private void Start()
    {
        poInicial = transform.position;
    }

    private void OnEnable()
    {
        try
        {
            InputManager.Instance.movementEvent.AddListener(MeMuevo);
            InputManager.Instance.interactEvent.AddListener(Interactuo);
        }
        catch (System.Exception e)
        {
            Debug.Log(e.ToString());
            Debug.LogWarning("No se pudo asignar los eventos, probablemente te faltara un InputManager");
        }
    }

    public void Update()
    {
        RaycastHit hit;
        bool ThisFrameIsPoiting = RaycastGenerator(out hit);
        ISelectionInteractable interactable;

        if (ThisFrameIsPoiting && ((!IsPointingSomething) || PointedGameobject != hit.transform.gameObject))
        {
            if (PointedGameobject != null)
            {
                PointedGameobject.GetComponent<ISelectionInteractable>().UnHover();
            }
            if (!hit.transform.gameObject.TryGetComponent<ISelectionInteractable>(out interactable)) return;
            interactable.Hover();
            IsPointingSomething = true;
            PointedGameobject = hit.transform.gameObject;
        }
        else if (!ThisFrameIsPoiting && IsPointingSomething)
        {
            if (PointedGameobject != null)
            {
                PointedGameobject.GetComponent<ISelectionInteractable>().UnHover();
            }
            IsPointingSomething = false;
            PointedGameobject = null;
        }
    }

    private void OnDisable()
    {
        InputManager.Instance.movementEvent.RemoveListener(MeMuevo);
        InputManager.Instance.interactEvent.RemoveListener(Interactuo);
    }

    public void Interactuo()
    {
        if ((MySceneManager.Instance != null ? MySceneManager.Instance.isLoading : false) || (GameManager.Instance != null ? GameManager.Instance.isPaused : false)) return;
        if (GameManager15.Instance.CanvasDialogo.activeInHierarchy) return;

        if (disableMovement)
        {
            return;
        }
        RaycastHit hit;
        if (RaycastGenerator(out hit))
        {
            if (hit.transform.gameObject.TryGetComponent<Iinteractable>(out Iinteractable inter))
            {
                inter.Interact();
                
            }
        }
       
    }
    public void MeMuevo(Vector2 vec)
    {
        if ( (MySceneManager.Instance != null ? MySceneManager.Instance.isLoading : false) || (GameManager.Instance != null ? GameManager.Instance.isPaused : false)) return;
        if (GameManager15.Instance.CanvasDialogo.activeInHierarchy) return;

        if (disableMovement)
        {
            return;
        }
        var Matrix = Matrix4x4.Rotate(Quaternion.Euler(0, -45f, 0));
        var inputChueca = Matrix.MultiplyPoint3x4(new Vector3(vec.x, 0f, vec.y));

        transform.position += new Vector3(inputChueca.x, 0, inputChueca.z)*speed*Time.deltaTime;
        //FormaCutre si no me veo en la pantalla vuelvo al centro de la pantalla
        if (!imVisible.isVisible)
        {
            transform.position = poInicial;
        }
    }

    public bool RaycastGenerator(out RaycastHit hit)
    {
        Debug.DrawRay(transform.position, Vector3.down*200f, Color.green);
        return Physics.Raycast(transform.position, Vector3.down, out hit, 200f);
    }



    public void hideTextFunction()
    {
        hideText.Invoke();
    }

    
}
