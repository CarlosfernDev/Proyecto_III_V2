using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using Unity.VisualScripting;
using UnityEngine;

public class SwapBridge : MonoBehaviour, Iinteractable
{
    [SerializeField] public bool _isInteractable;
    [SerializeField] private string _TextoInteraccion;
    [SerializeField] private int costMaterial;
    [SerializeField] private float _transitionDuration;

    [SerializeField] private Material _GhostMat;
    [SerializeField] private Material _BridgeWoodMat;
    [SerializeField] private Material _BridgeWoodMat2;
    [SerializeField] private Material _BridgeGoldMat;
    
    [SerializeField] private Renderer _rendererWood1;
    [SerializeField] private Renderer _rendererWood2;
    [SerializeField] private Renderer _rendererWoodMat2;
    [SerializeField] private Renderer _rendererGold;

    private List<Renderer> _bridgeRenderer = new List<Renderer>();
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
    
    public void Start()
    {
        _GhostMat.SetFloat("_RectangleHeight", 0);
        _TextoInteraccion = "Coste de construccion: " + costMaterial;

        _bridgeRenderer.Add(_rendererWood1);
        _bridgeRenderer.Add(_rendererWood2);
        _bridgeRenderer.Add(_rendererWoodMat2);
        _bridgeRenderer.Add(_rendererGold);

        foreach (Renderer r in _bridgeRenderer)
        {
            r.material = _GhostMat;
        }
    }
    public void Interact()
    {
        if (GameManagerSergio.Instance.checkMaterial() >= costMaterial)
        {
            Debug.Log("Pago para construir puente");
            GameManagerSergio.Instance.minusMaterial(costMaterial);
            Swap();
        }
        else
        {
            Debug.Log("Necesito mas monedas");
        }
        
    }

    public void SetInteractFalse()
    {
        IsInteractable = false;
    }

    public void SetInteractTrue()
    {
        IsInteractable = true;

    }

    public void Swap()
    {
        if (GameObject.Find("Player") != null)
        {
            GameObject.Find("Player").GetComponent<TestInputs>().hideTextFunction();
        }
        
        StartCoroutine(SwapBridgeCoroutine());
    }

    private IEnumerator SwapBridgeCoroutine()
    {
        float lerp = 0;
        while (lerp < 1)
        {
            _GhostMat.SetFloat("_RectangleHeight", Mathf.Lerp(0f, 2f, lerp));
            lerp += Time.deltaTime * _transitionDuration;
            yield return null;
        }
        if (lerp >= 1)
        { 
            Invoke(nameof(SetNormalBridgeMat), 0.4f);
        }
    }

    public void SetNormalBridgeMat()
    {
        _rendererGold.material = _BridgeGoldMat;
        _rendererWood1.material = _BridgeWoodMat;
        _rendererWood2.material = _BridgeWoodMat;
        _rendererWoodMat2.material = _BridgeWoodMat2;
        GetComponent<BoxCollider>().isTrigger = true;
    }
}
