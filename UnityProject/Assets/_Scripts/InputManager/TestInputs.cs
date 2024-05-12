using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(-1)]
public class TestInputs : MonoBehaviour
{

    //Equipment (Red nubes fran)
    [SerializeField] public Transform positionEquipable;
    public bool isEquipado = false;
    public GameObject refObjetoEquipado;
    public bool isEquipableInCooldown;

    //Interaction
    [SerializeField] public GameObject interactZone;
    private GameObject refObjetoInteract;
    private bool isInteractable = false;



    //Movement
    [SerializeField] public bool sloopyMovement;
    [SerializeField] public PhysicMaterial materialStop;
    [SerializeField] public PhysicMaterial materialDrag;
    [SerializeField] public PhysicMaterial materialNormal;
    [SerializeField] public PhysicMaterial materialRampa;
    public Rigidbody rb;
    private float grav;
    public float actualAcceSpeed;
    public float actualMaxSpeed;
    public float actualDesSpeed;
    public float rotationSpeed;

    private float baseAcceSpeed;
    private float baseMaxSpeed;
    private float baseDesAccSpeed;

    private IEnumerator coroutineBoostVelocidad;



    //Actualizador de UI? maybe hay que moverlo a los scripts interactuables y hacer que los objetos busquen la ui en la escena
    [SerializeField] private UnityEvent hideText;
    [SerializeField] private UnityEvent<string> TextoInteractChange;
    //Modificacion de la clase event para poder pasar en las llamadas strings
    [System.Serializable]
    public class MyStringEvent : UnityEvent<string>
    {
    }

    public void Awake()
    {
        rb = transform.GetComponent<Rigidbody>();
        baseAcceSpeed = actualAcceSpeed;
        baseMaxSpeed = actualMaxSpeed;
        baseDesAccSpeed = actualDesSpeed;

    }

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.playerScript = this;
        }
    }

    private void OnEnable()
    {
        try
        {
            InputManager.Instance.movementEvent.AddListener(MeMuevo);
            InputManager.Instance.equipableEvent.AddListener(UsarObjetoEquipable);
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
        InputManager.Instance.equipableEvent.RemoveListener(UsarObjetoEquipable);
        InputManager.Instance.interactEvent.RemoveListener(Interactuo);

    }

    public void DisablePlayer()
    {
        sloopyMovement = false;
        rb.velocity = Vector3.zero;
    }

    public void Interactuo()
    {
        
        if (GameManager.Instance.isDialogueActive || (MySceneManager.Instance != null ? MySceneManager.Instance.isLoading : false) || (GameManager.Instance != null ? GameManager.Instance.isPaused : false)) return;

        if (isInteractable)
        {
            refObjetoInteract.GetComponent<Iinteractable>().Interact();
            Debug.Log("INTERACTUO");
        }
        else
        {
            Debug.Log("NO PUEDO INTERACTUAR");
        }
    }
    public void MeMuevo(Vector2 vec)
    {
        
        if (GameManager.Instance.isDialogueActive || (MySceneManager.Instance != null ? MySceneManager.Instance.isLoading : false) || (GameManager.Instance != null ? GameManager.Instance.isPaused : false)) return;
        if (sloopyMovement)
        {
            if (vec.magnitude == 0)
            {
                if (rb.velocity.magnitude < 0.1f)
                {
                    transform.GetComponent<Collider>().material = materialStop;
                }
                else
                {

                    transform.GetComponent<Collider>().material = materialDrag;

                }

                rb.AddForce(Vector3.down * 9.8f, ForceMode.Acceleration);
                // Debug.Log(rb.velocity.magnitude);

            }
            else
            {
                bool rampa = false;
                RaycastHit[] hit;
                grav = 0f;
                hit = Physics.RaycastAll(transform.position, Vector3.down, 1.1F);

                foreach (var obj in hit)
                {
                    if (obj.transform.tag == "Rampa")
                    {
                        rampa = true;
                        transform.GetComponent<Collider>().material = materialRampa;
                        grav = 0f;

                    }
                }

                if (!rampa)
                {
                    transform.GetComponent<Collider>().material = materialNormal;
                    grav = 9.8f;
                }









                Quaternion toRotation = Quaternion.LookRotation(new Vector3(vec.x + transform.position.x, 0f, vec.y + transform.position.z) - new Vector3(transform.position.x, 0f, transform.position.z));
                if (Mathf.Rad2Deg * Mathf.Abs(rb.rotation.y - toRotation.y) > 45f)
                {
                    transform.rotation = Quaternion.LookRotation(new Vector3(vec.x + transform.position.x, 0f, vec.y + transform.position.z) - new Vector3(transform.position.x, 0f, transform.position.z));
                }
                else
                {
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
                }

                if (rb.velocity.magnitude > actualMaxSpeed)
                {
                    rb.velocity = rb.velocity.normalized * actualMaxSpeed;
                }
                //Movement con gravedad
                rb.AddForce(new Vector3(vec.x * actualAcceSpeed, -1 * grav, vec.y * actualAcceSpeed), ForceMode.Acceleration);
            }


        }



    }

    public void BoostVelocidad(float velocidadMaximaNueva, float velocidadAceleracionNueva, float velocidadDesacNueva, float Tiempo)
    {
        if (coroutineBoostVelocidad != null)
        {
            StopCoroutine(coroutineBoostVelocidad);
        }

        coroutineBoostVelocidad = BoostVelocidadCoroutine(velocidadMaximaNueva, velocidadAceleracionNueva, velocidadDesacNueva, Tiempo);
        StartCoroutine(coroutineBoostVelocidad);
    }

    public IEnumerator BoostVelocidadCoroutine(float velocidadMaximaNueva, float velocidadAceleracionNueva, float velocidadDesacNueva, float Tiempo)
    {
        //actualAcceSpeed = velocidadAceleracionNueva;
        //actualMaxSpeed = velocidadMaximaNueva;
        //actualDesSpeed = velocidadDesacNueva;
        BoostVelocidadPermanente(velocidadAceleracionNueva, velocidadAceleracionNueva, velocidadDesacNueva);

        yield return new WaitForSeconds(Tiempo);

        //actualAcceSpeed = baseAcceSpeed;
        //actualMaxSpeed = baseMaxSpeed;
        //actualDesSpeed = baseDesAccSpeed;
        BoostVelocidadRestaurar();
    }

    public void BoostVelocidadPermanente(float velocidadMaximaNueva, float velocidadAceleracionNueva, float velocidadDesacNueva)
    {
        actualAcceSpeed = velocidadAceleracionNueva;
        actualMaxSpeed = velocidadMaximaNueva;
        actualDesSpeed = velocidadDesacNueva;
    }

    public void BoostVelocidadRestaurar()
    {
        actualAcceSpeed = baseAcceSpeed;
        actualMaxSpeed = baseMaxSpeed;
        actualDesSpeed = baseDesAccSpeed;
    }



    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.TryGetComponent<Iinteractable>(out Iinteractable interactable))
        {
            if (interactable.IsInteractable)
            {
                refObjetoInteract = other.gameObject;
                isInteractable = true;
                TextoInteractChange.Invoke(other.GetComponent<Iinteractable>().TextoInteraccion);
                Debug.Log("inrangeofobject");
            }

        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<Iinteractable>(out Iinteractable interactable) && refObjetoInteract == other.gameObject)
        {
            refObjetoInteract = null;
            isInteractable = false;
            hideTextFunction();

        }
    }


    public void UsarObjetoEquipable()
    {

        if (isEquipado)
        {
            //Llamo a la funcion que deben implementar todos los objetos equipables.
            refObjetoEquipado.GetComponent<Iequipable>().UseEquipment();

            Debug.Log("Uso objeto" + refObjetoEquipado.name);
        }
        else
        {
            Debug.Log("OBJETO NO EQUIPADO");
        }
    }


    public void hideTextFunction()
    {
        hideText.Invoke();
    }
}
