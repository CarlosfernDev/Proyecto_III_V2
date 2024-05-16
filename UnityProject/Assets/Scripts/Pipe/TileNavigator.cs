using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileNavigator : MonoBehaviour
{
    [SerializeField] Vector2Int PosSelector;
    private Vector2Int addPos;
    private int y;
    private int x;
    [SerializeField] float movementCooldown;
    private float contador;

    [SerializeField] GameObject Tpipe;
    [SerializeField] GameObject Pluspipe;
    [SerializeField] GameObject Straightpipe;
    GameObject temporalGO;
    GameObject ShowGO;
    float rotYplacement = 0;


    private void Start()
    {
        
    }
    void Update()
    {
        
        contador = contador + Time.deltaTime;
        addPos = CheckInput();

        x = 0; y = 0;
        if (addPos.magnitude != 0)
        {
            if (movementCooldown > contador) { return; }
            contador = 0;
            PosSelector =PosSelector + addPos;
           if( PipeGrid.Instance.GetPipeAtPosition(PosSelector) != null)
            {
                PipeGrid.Instance.DesactivarSeleciones();
                PipeGrid.Instance.GetPipeAtPosition(PosSelector).selectedTile();
            }
            else
            {
                PosSelector = PosSelector - addPos;
            }
        }
        CheckBuildKey();
        UpdateTestObjectPos();
    }

    public Vector2Int CheckInput()
    {
        
        if (Input.GetKey(KeyCode.W)) { y = +1; }
        if (Input.GetKey(KeyCode.S)) { y = -1; }
        if (Input.GetKey(KeyCode.A)) { x = -1; }
        if (Input.GetKey(KeyCode.D)) { x = +1; }

        return new Vector2Int(x,y);
    }

    public void CheckBuildKey()
    {
        if (!PipeGrid.Instance.GetPipeAtPosition(PosSelector))
        {
            return;
        }

        //Vector Donde se Construye la Pipe
        Vector3 pos = PipeGrid.Instance.GetPipeAtPosition(PosSelector).transform.position;
        pos = new Vector3(pos.x, pos.y + 1, pos.z);


        if (Input.GetKey(KeyCode.Z))
        {
            temporalGO = Tpipe;

            if (ShowGO != Tpipe)
            {
                if (ShowGO != null)
                {
                    Destroy(ShowGO.gameObject);
                    ShowGO = Instantiate(Tpipe, pos, Quaternion.identity);
                }
                else
                {
                    ShowGO = Instantiate(Tpipe, pos, Quaternion.identity);
                }
            }
            else
            {
                
            }
            
        }


        if (Input.GetKey(KeyCode.X))
        {
            temporalGO = Pluspipe;

            if (ShowGO != Pluspipe)
            {
                if (ShowGO != null)
                {
                    Destroy(ShowGO.gameObject);
                    ShowGO = Instantiate(Pluspipe, pos, Quaternion.identity);
                }
                else
                {
                    ShowGO = Instantiate(Pluspipe, pos, Quaternion.identity);
                }
            }

        }


        if (Input.GetKey(KeyCode.C))
        {
            temporalGO = Straightpipe;

            if (ShowGO != Straightpipe)
            {
                if (ShowGO != null)
                {
                    Destroy(ShowGO.gameObject);
                    ShowGO = Instantiate(Straightpipe, pos, Quaternion.identity);
                }
                else
                {
                    ShowGO = Instantiate(Straightpipe, pos, Quaternion.identity);
                }
            }

        }


        //Rotar Visual GO
        if (Input.GetKeyDown(KeyCode.R))
        { 
            if (ShowGO != null)
            {
               
                rotYplacement += 90;
                if (rotYplacement == 360)
                {
                    rotYplacement = 0;
                }
                
                ShowGO.transform.eulerAngles = new Vector3(0, rotYplacement, 0);
            }
        }

        //Enter Contruir Temporal Pipe
        if (Input.GetKey(KeyCode.Return))
        {
            if (temporalGO == null)
            {
                return;
            }
            var tempoGO2 = PipeGrid.Instance.GetPipeAtPosition(PosSelector);
            tempoGO2.InstantiateVisualGO(temporalGO, (int)rotYplacement);
            tempoGO2.DesactivateWater();
            temporalGO = null;
            Destroy(ShowGO);
            rotYplacement = 0;
            PipeGrid.Instance.ReCheckConectionsToWaterSource();
        }
        //InstanciarWaterSource
        if (Input.GetKey(KeyCode.Y))
        {
            if (temporalGO == null)
            {
                return;
            }
            var tempoGO2 = PipeGrid.Instance.GetPipeAtPosition(PosSelector);
            tempoGO2.InstantiateVisualGO(temporalGO, (int)rotYplacement);
            tempoGO2.WaterSource = true;
            tempoGO2.ActivateWater();
            temporalGO = null;
            Destroy(ShowGO);
            rotYplacement = 0;
            PipeGrid.Instance.ReCheckConectionsToWaterSource();
        }

        if (Input.GetKey(KeyCode.Alpha1))
        {
            Debug.Log("recheck");
            PipeGrid.Instance.ReCheckConectionsToWaterSource();
        }


        if (Input.GetKey(KeyCode.Space))
        {
            if (PipeGrid.Instance.GetPipeAtPosition(PosSelector) != null)
            {
                PipeGrid.Instance.BorrarPipe(PosSelector);
            }
        }

    }

    void UpdateTestObjectPos()
    {
        if (ShowGO == null)
        {
            return;
        }
        Vector3 pos = PipeGrid.Instance.GetPipeAtPosition(PosSelector).transform.position;
        pos = new Vector3(pos.x, pos.y + 1, pos.z);
        ShowGO.transform.position = pos;
    }



}
