using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileNavigator : MonoBehaviour
{
    [SerializeField] public Vector2Int PosSelector;
    private Vector2Int addPos;
    private int y;
    private int x;
    [SerializeField] float movementCooldown;
    private float contador;

    private int ContPieza = 0;

    [SerializeField] GameObject Tpipe;
    [SerializeField] GameObject Pluspipe;
    [SerializeField] GameObject Straightpipe;
    GameObject temporalGO;
    GameObject ShowGO;
    float rotYplacement = 0;


    private void OnEnable()
    {
        try
        {
            InputManager.Instance.movementEvent.AddListener(CheckInput);
            InputManager.Instance.interactEvent.AddListener(CheckPlaceKey);
            InputManager.Instance.equipableEvent.AddListener(aumentarContadorPieza);
            InputManager.Instance.AlexRotateEvent.AddListener(CheckRotationKey);
        }
        catch (System.Exception e)
        {
            Debug.Log(e.ToString());
            Debug.LogWarning("No se pudo asignar los eventos, probablemente te faltara un InputManager");
        }
    }
    private void OnDisable()
    {
        InputManager.Instance.movementEvent.RemoveListener(CheckInput);
        InputManager.Instance.interactEvent.RemoveListener(CheckPlaceKey);
        InputManager.Instance.equipableEvent.RemoveListener(aumentarContadorPieza);
        InputManager.Instance.AlexRotateEvent.RemoveListener(CheckRotationKey);
    }







    private void Start()
    {
        CheckBuildKey();
    }
    void Update()
    {
        
        contador = contador + Time.deltaTime;
        

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
        //CheckBuildKey();
        //CheckRotationKey();
        //CheckPlaceKey();
        UpdateTestObjectPos();
    }

    public void CheckInput(Vector2 vec)
    {

        var Matrix = Matrix4x4.Rotate(Quaternion.Euler(0, -45f, 0));
        var inputChueca = Matrix.MultiplyPoint3x4(new Vector3(vec.x, 0f, vec.y));

        if (inputChueca.x>0)
        {
            inputChueca.x = 1;
        }
        if (inputChueca.x < 0)
        {
            inputChueca.x = -1;
        }
        if (inputChueca.z > 0)
        {
            inputChueca.z = 1;
        }
        if (inputChueca.z < 0)
        {
            inputChueca.z = -1;
        }
        addPos = new Vector2Int((int)inputChueca.x, (int)inputChueca.z);
        



    }


    public void aumentarContadorPieza()
    {
        ContPieza += 1;
        if (ContPieza>=3)
        {
            ContPieza = 0;
        }
        CheckBuildKey();
    }
    public void CheckBuildKey()
    {
        Debug.Log("CargoPieza");

        if (!PipeGrid.Instance.GetPipeAtPosition(PosSelector))
        {
            return;
        }
        //Vector Donde se Construye la Pipe
        Vector3 pos = PipeGrid.Instance.GetPipeAtPosition(PosSelector).transform.position;
        pos = new Vector3(pos.x, pos.y + 1, pos.z);

        switch (ContPieza)
        {
            case 0:
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
                break;
            case 1:
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
                break;
            case 2:
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
                break;
            default:
                break;
        }

        /*
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

        */


        //Rotar Visual GO
        /*
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
        */

        //Enter Contruir Temporal Pipe
        /*
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
        */
        //InstanciarWaterSource
        /*
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
        }*/

        if (Input.GetKey(KeyCode.Alpha1))
        {
            Debug.Log("recheck");
            PipeGrid.Instance.ReCheckConectionsToWaterSource();
        }

        /*
        if (Input.GetKey(KeyCode.Space))
        {
            if (PipeGrid.Instance.GetPipeAtPosition(PosSelector) != null)
            {
                PipeGrid.Instance.BorrarPipe(PosSelector);
            }
        }
        */

    }

    public void CheckRotationKey()
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

    public void CheckPlaceKey()
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
        CheckBuildKey();
    }


    public void ResetPosZeroZero()
    {
        PosSelector = new Vector2Int(0, 0);
        PipeGrid.Instance.DesactivarSeleciones();
        PipeGrid.Instance.GetPipeAtPosition(new Vector2(0, 0)).selectedTile();
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

    public void moveToZeroZero()
    {
        PosSelector = new Vector2Int(0, 0);
    }


}
