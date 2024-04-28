using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractuableMateriales : MonoBehaviour, Iinteractable
{
    [SerializeField] public bool _isInteractable;
    [SerializeField] private string _TextoInteraccion;
    public string TextoInteraccion
    {
        get { return _TextoInteraccion; }
        set { _TextoInteraccion = value; }
    }
    public bool IsInteractable
    {
        get { return _isInteractable; }
        set { _isInteractable = value; }
    }
    public void Interact()
    {
        
        GameManagerSergio.Instance.addMaterial(10);
        //Destroy(this.gameObject);
    }

    public void SetInteractFalse()
    {
        IsInteractable = false;
    }

    public void SetInteractTrue()
    {
        IsInteractable = true;

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
