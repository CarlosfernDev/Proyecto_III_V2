using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(1)]
public class PunteroScriptLaura: MonoBehaviour
{

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

    [SerializeField] MeshRenderer imVisible;
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

    private void OnDisable()
    {
        InputManager.Instance.movementEvent.RemoveListener(MeMuevo);
        InputManager.Instance.interactEvent.RemoveListener(Interactuo);
    }

    public void Interactuo()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 200f))
        {
            if (hit.transform.gameObject.TryGetComponent<Iinteractable>(out Iinteractable inter))
            {
                inter.Interact();
                
            }
        }
       
    }
    public void MeMuevo(Vector2 vec)
    {
        var Matrix = Matrix4x4.Rotate(Quaternion.Euler(0, -45f, 0));
        var inputChueca = Matrix.MultiplyPoint3x4(new Vector3(vec.x, 0f, vec.y));

        transform.position += new Vector3(inputChueca.x, 0, inputChueca.z)*speed*Time.deltaTime;
        //FormaCutre si no me veo en la pantalla vuelvo al centro de la pantalla
        if (!imVisible.isVisible)
        {
            transform.position = poInicial;
        }
    }





    public void hideTextFunction()
    {
        hideText.Invoke();
    }
}
