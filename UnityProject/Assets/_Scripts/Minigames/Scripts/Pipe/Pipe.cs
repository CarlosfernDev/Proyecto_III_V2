using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;


public class Pipe : MonoBehaviour
{
    [SerializeField] public Vector2[] ActiveConections;
    [SerializeField] float yRot = 0;
    [SerializeField] public bool emptyPipe;
    [SerializeField] public GameObject visualGO;
    [SerializeField] public GameObject SelectionVisual;
    [SerializeField] public bool RunningWater = false;
    [SerializeField] public bool WaterSource = false;
    [SerializeField] public bool Endgame = false;
    [SerializeField] public bool Puntuado=false;

    [SerializeField] public Material WaterMat;
    [SerializeField] public Material ShitMat;
    
    [SerializeField] private VisualEffect waterVFX;




    public virtual void Init(int x, int y)
    {
        if(Endgame == true)
        {
            Debug.Log("ASIGNO NUEVOS VECTORES");
            ActiveConections = new Vector2[] { new Vector2(+1, 0), new Vector2(0, +1), new Vector2(0, -1),new Vector2(-1,0)};
        }
        
         waterVFX = Resources.Load<VisualEffect>("Assets/Prefab/V2Pipes/AlexWaterVfx");
        
    }

    public virtual void selectedTile()
    {

        SelectionVisual.SetActive(true);

    }

    public virtual void UNselecteTile()
    {
        SelectionVisual.SetActive(false);

    }

    public virtual void RotateObject90degrees(int rot)
    {
       
        if (rot == 360)
        {
            rot = 0;
        }
        yRot = rot;
        transform.eulerAngles = new Vector3(0, rot, 0);


    }

    public virtual void InstantiateVisualGO(GameObject go,int rotY)
    {
        if (Endgame == true || WaterSource == true)
        {
            return;
        }
        yRot = rotY;
        if (visualGO == null)
        {
            visualGO = Instantiate(go, this.transform.position, Quaternion.Euler(0, rotY, 0));
            visualGO.transform.parent = transform;
            
        }
        else
        {
            DeleteVisualGO();
            visualGO = Instantiate(go, this.transform.position, Quaternion.Euler(0, rotY, 0));
            visualGO.transform.parent = transform;
            
        }
        if (WaterSource == true)
        {
            CheckConections();
        }
        else
        {
            CheckObjectTipe();
        }
       



    }



    public virtual void DeleteVisualGO()
    {
        Destroy(visualGO);
        visualGO = null;
    }

    public virtual void CheckObjectTipe()
    {
        
        if (Endgame)
        {
            return;
        }
        switch (visualGO.name)
        {
            case "LShape(Clone)":
                if (yRot/90 == 0)
                {
                    ActiveConections = new Vector2[]{new Vector2( 0, 1 ),new Vector2( 1, 0 )};
                }

                if (yRot/90 == 1)
                {
                    ActiveConections = new Vector2[] { new Vector2(1, 0), new Vector2(0, -1) };
                }

                if (yRot/90 == 2)
                {
                    ActiveConections = new Vector2[] { new Vector2(-1, 0), new Vector2(0, -1) };
                }

                if (yRot/90 == 3)
                {
                    ActiveConections = new Vector2[] { new Vector2(-1, 0), new Vector2(0, 1) };
                }
                break;
            case "Straight(Clone)":
                if (yRot / 90 == 0)
                {
                    ActiveConections = new Vector2[] { new Vector2(-1, 0), new Vector2(+1, 0) };
                }

                if (yRot / 90 == 1)
                {
                    ActiveConections = new Vector2[] { new Vector2(0, -1), new Vector2(0,+1) };
                }

                if (yRot / 90 == 2)
                {
                    ActiveConections = new Vector2[] { new Vector2(-1, 0), new Vector2(+1, 0) };
                }

                if (yRot / 90 == 3)
                {
                    ActiveConections = new Vector2[] { new Vector2(0, -1), new Vector2(0, +1) };
                }
                // code block
                break;
            case "TShape(Clone)":
                if (yRot / 90 == 0)
                {
                    ActiveConections = new Vector2[] { new Vector2(-1, 0), new Vector2(+1, 0),new Vector2(0, -1) };
                }

                if (yRot / 90 == 1)
                {
                    ActiveConections = new Vector2[] { new Vector2(-1, 0), new Vector2(0, +1), new Vector2(0, -1) };
                }

                if (yRot / 90 == 2)
                {
                    ActiveConections = new Vector2[] { new Vector2(0, +1), new Vector2(+1, 0), new Vector2(-1, 0) };
                }

                if (yRot / 90 == 3)
                {
                    ActiveConections = new Vector2[] { new Vector2(+1, 0), new Vector2(0, +1), new Vector2(0, -1) };
                }
                // code block
                break;
            default:
                // code block
                break;
        }
    }
    public void CheckConections()
    {
        for (int i = 0; i < ActiveConections.Length; i++)
        {
            if (PipeGrid.Instance.GetPipeAtPosition(new Vector2(ActiveConections[i].x + PipeGrid.Instance.GetPositionOfPipe(this).x, ActiveConections[i].y + PipeGrid.Instance.GetPositionOfPipe(this).y)) == null)
            {
                //Si la casilla no existe salto al siguiente ciclo del for
                continue;
            }
            else
            {
                //Si la casilla existe compruebo que tenga una pipe
                if (PipeGrid.Instance.GetPipeAtPosition(new Vector2(ActiveConections[i].x + PipeGrid.Instance.GetPositionOfPipe(this).x, ActiveConections[i].y + PipeGrid.Instance.GetPositionOfPipe(this).y)).visualGO != null)
                {
                    var posiblePipe = PipeGrid.Instance.GetPipeAtPosition(new Vector2(ActiveConections[i].x + PipeGrid.Instance.GetPositionOfPipe(this).x, ActiveConections[i].y + PipeGrid.Instance.GetPositionOfPipe(this).y));
                    if (RunningWater && posiblePipe.RunningWater == false)
                    {
                        //Compruebo si la pipe esta conectada a mi y yo a ella
                        for (int x = 0; x < posiblePipe.ActiveConections.Length; x++)
                        {
                            if (posiblePipe.ActiveConections[x]+PipeGrid.Instance.GetPositionOfPipe(posiblePipe) == PipeGrid.Instance.GetPositionOfPipe(this) && PipeGrid.Instance.GetPositionOfPipe(this)+ActiveConections[i] == PipeGrid.Instance.GetPositionOfPipe(posiblePipe))
                            {
                                if (posiblePipe.Endgame == true)
                                {
                                    Debug.Log("EXISTO HIJOS DE PUTA");
                                }
                                posiblePipe.ActivateWater();
                            }
                        }
                        
                    }
                    if (RunningWater == false && posiblePipe.RunningWater == true)
                    {
                        //Si ellos tienen agua, yo me pongo el agua
                        posiblePipe.CheckConections();
                    }


                }
            }
        }
        
    }


           
    

   

    public void ActivateNewMats()
    {
        if (visualGO != null)
        {
            var Renderer = visualGO.GetComponentsInChildren<Renderer>();
            foreach (var item in Renderer)
            {
                item.material = WaterMat;
            }
            
        }

    }

    public void DesactivateWater()
    {
        if (WaterSource == true)
        {
            return;
        }
        if (visualGO != null)
        {
            var Renderer = visualGO.GetComponentsInChildren<Renderer>();
            foreach (var item in Renderer)
            {
                item.material = ShitMat;
            }
        }
        

        RunningWater = false;
    }
    public void ActivateWater()
    {
        if (visualGO != null)
        {
            var Renderer = visualGO.GetComponentsInChildren<Renderer>();
            foreach (var item in Renderer)
            {
                item.material = WaterMat;
            }
        }


        RunningWater = true;
        if (waterVFX != null)
        {
            Debug.Log("PLAY VFX");
            waterVFX.Play();
        }
        CheckConections();
        if (Puntuado == false)
        {
            if (Endgame == true && RunningWater == true)
            {
                Puntuado = true;
                OD6GameManager.Instance.Score += 1;
                OD6GameManager.Instance.checkConditions();
            }
        }
        if (Puntuado == true && RunningWater == false)
        {
            OD6GameManager.Instance.Score -= 1;

        }

    }

    public void DesactivateWaterSource()
    {

    }
}
