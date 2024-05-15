using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuntoEcologicoODS7 : LInteractableParent
{
    [SerializeField] private TestInputs script;
    [SerializeField] private EquipableRedTest redScript;

    private void Start()
    {
        script = GameObject.Find("Player").GetComponent<TestInputs>();
        redScript = FindObjectOfType<EquipableRedTest>();
    }

    public override void Interact()
    {
        base.Interact();
        
        Debug.Log("interacting with eco point");
        if (!script.isEquipado)
        {
            Debug.Log("not equipped");
            return;
        }
        //redScript = script.refObjetoEquipado.GetComponent<EquipableRedTest>();
        if (redScript.cloudCaptured != null)
        {
            Debug.Log("Trying to deposit cloud");
            //Llamar funcion de puntuacion?
            redScript.cloudCaptured.transform.parent = null;

            CloudAI ai = redScript.cloudCaptured.gameObject.GetComponent<CloudAI>();

            if (ai.targetCloudSpawner)
            {
                ai.targetCloudSpawner.TargetAI = null;
            }

            if (ODS7Singleton.Instance.cloudList.Contains(ai)) 
            {
                ODS7Singleton.Instance.cloudList.Remove(ai);
            }
            Destroy(redScript.cloudCaptured.gameObject);

            redScript.cloudCaptured = null;
            redScript._isCloudCaptured = false;
      
            // Bufos
            script.BoostVelocidad(10f, 20f, 0.9f, 5f);
            ODS7Singleton.Instance.timer.AddTime(ODS7Singleton.Instance.AddTime);

            Debug.Log("Nube en objeto");
        }
    }
}
