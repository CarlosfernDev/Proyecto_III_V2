using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

/// Revision: Esta god, pero deberiamos buscar una manera de que no sea en el pickup, setea como singleton el testinputs y haz que siempre en el awake sea machada por uno nuevo.
/// De esta manera mantienes las referencias al jugador sin necesidad de que ponerselas a mano en Unity, y luego lo suyo seria crear una manera de poder implementar el powerup a mano en el inspector,
/// La solucion mas rapida es asignarlo todo a un padre llamado "PlayerUpgrade" y donde tienes el "TestInput" cambiar la logica de "refObjetoEquipado" que ya seria de esa clase en vez de ser un gameobject.
/// A su vez la logica de TestInputs seria agarrar la clase UseEquipment del padre. 

public class EquipableRedTest : LInteractableParent, Iequipable
{
    [SerializeField] private GameObject player;
    [SerializeField] private TestInputs script;
    [SerializeField] private Transform insideRed;
    [SerializeField] public bool _isCloudCaptured = false;
    [SerializeField] public GameObject cloudCaptured;
    [SerializeField] private VisualEffect _AuraVFX;


    public override void Interact()
    {

        player = GameObject.Find("Player");
        _AuraVFX.Stop();
        script = player.GetComponent<TestInputs>();
        transform.GetComponent<Collider>().enabled = false;
        
        //Refs en char controller
        script.isEquipado = true;
        script.refObjetoEquipado = transform.gameObject;

        //pos y rot seteadas 
        transform.position = script.positionEquipable.transform.position;
        transform.parent = script.positionEquipable.transform;
        transform.rotation = script.positionEquipable.rotation;

        
        SetInteractFalse();
        //Maybe hacer la ui estatica o un singleton que la maneje, es un coñazo tener que llamar asi
        script.hideTextFunction();

    }

    public void UseEquipment()
    {
        /// Revision: Falta gestionar que la corutina no se repita guardandola en una variable Coroutine y revisando que si no es null lo pare o no ejecute el start
        StartCoroutine(AccionUsarRed());
      
    }

    IEnumerator AccionUsarRed()
    {
        if (script.isEquipableInCooldown == true)
        {
            Debug.Log("Estoy en cooldown PLAP PLAP PLAP");
            yield break;
        }
        if (_isCloudCaptured)
        {
            Debug.Log("Red Llena");
            yield break;
        }
        Debug.Log("HE SIDO USADO");
        script.isEquipableInCooldown = true;
        //Do shit
        transform.localRotation = Quaternion.Euler(90, 0, 0);

        Collider[] hitColliders = Physics.OverlapSphere(script.interactZone.transform.position, 1f);
        
        foreach (var item in hitColliders)
        {
            /// Revision: Usa FindObjectsOfType, ademas de restringir colisiones con una layer. Esto nos permitira en un futuro reutilizar codigo y tenerlo todo mas centralizado,
            /// ademas de no abusar de las tags que se usa para postprocesado tambien y no son ilimitadas.
            if (item.gameObject.tag == "Nube")
            {
                if (_isCloudCaptured == false)
                {
                    _isCloudCaptured = true;
                    item.gameObject.GetComponent<IAnube>().isStandBY = true;
                    item.gameObject.GetComponent<Collider>().enabled = false;
                    item.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    item.gameObject.transform.parent = insideRed.transform;
                    item.gameObject.transform.localPosition = new Vector3(0, 0, 0);
                    //item.gameObject.transform.position = insideRed.position;
                   // item.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    cloudCaptured = item.gameObject;

                }
                
            }
        }
        /// Revision: Si la animacion es esto, mejor separa el codigo principal fuera de la corutina y una vez realizado que ejecute una corutina u otra
        yield return new WaitForSeconds(0.5f);

        transform.localRotation = Quaternion.Euler(0, 0, 0);
        script.isEquipableInCooldown = false;
    }


    void OnDrawGizmos()
    {
        if(script == null) 
        {
            return; 
        }
        else 
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(script.interactZone.transform.position, 1f);
        }
        
        
    }
}



