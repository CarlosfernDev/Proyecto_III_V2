using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using Unity.VisualScripting;
using UnityEngine;

public class SwapBridge : LInteractableParent
{
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

    [SerializeField] private bool AcabarJuegoAlinteract = false;
    [SerializeField] private bool CambioPuente = false;

    private List<Renderer> _bridgeRenderer = new List<Renderer>();    
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
    public override void Interact()
    {
        if (GameManagerSergio.Instance.checkMaterial() >= costMaterial)
        {
            Debug.Log("Pago para construir puente");
            GameManagerSergio.Instance.minusMaterial(costMaterial);
            Swap();

            if (AcabarJuegoAlinteract)
            {
                GameManagerSergio.Instance.youWin = true;
                GameManagerSergio.Instance.EnseñarPantallaFinal();
            }
        }
        else
        {
            Debug.Log("Necesito mas monedas");
        }
        
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
        CambioPuente = true;
        desactivarSelector();
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


    public override void Hover()
    {
        if (Selector != null && CambioPuente == false) Selector.SetActive(true);
    }

    public override void Unhover()
    {
        if (Selector != null && CambioPuente == false) Selector.SetActive(false);
    }

    public void desactivarSelector()
    {
        Selector.SetActive(false);
    }
}
