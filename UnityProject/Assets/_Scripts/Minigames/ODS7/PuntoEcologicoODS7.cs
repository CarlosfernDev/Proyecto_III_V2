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
       

        if (!script.isEquipado)
        {
            return;
        }
        if (redScript.cloudCaptured != null)
        {
            //Llamar funcion de puntuacion?
            redScript.cloudCaptured.transform.parent = null;

            CloudAI ai = redScript.cloudCaptured.gameObject.GetComponent<CloudAI>();

            ODS7Singleton.Instance.DestroyCloud(ai);

            redScript.cloudCaptured = null;
            redScript.isCloudCaptured = false;
      
            ODS7Actions.OnCloudDelivered();

            ODS7Singleton.Instance._gpsAnimator.SetTrigger("Off");

            // Bufos
            script.BoostVelocidad(10f, 20f, 0.9f, 5f);
            ODS7Singleton.Instance.timer.AddTime(ODS7Singleton.Instance.AddTime);
        }
    }

    public override void Hover()
    {
        if (!script.isEquipado && redScript.cloudCaptured == null) return;
        base.Hover();
    }

    public override void Unhover()
    {
        if (!script.isEquipado && redScript.cloudCaptured == null) return;
        base.Unhover();
    }
}
