using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PipeGrid : MonoBehaviour
{
    public static PipeGrid Instance;
    [SerializeField] private int LevelTestingW, LevelTestingH;
    [SerializeField] private int _width, _height;
    [SerializeField] private int _width2, _height2;
    [SerializeField] private int _width3, _height3;
    [SerializeField] private int _width4, _height4;
    [SerializeField] private int _width5, _height5;

    [SerializeField] private Vector2[] puntosFinales;

    [SerializeField] private GameObject level1;
    [SerializeField] private GameObject level2;
    [SerializeField] private GameObject level3;
    [SerializeField] private GameObject level4;
    [SerializeField] private GameObject level5;


    private Dictionary<Vector2, Pipe> _pipe;
    [SerializeField] Pipe emptyPipe;
    [SerializeField] GameObject PipeParent;
    [SerializeField] Pipe EndPipe;

    private void Awake()
    {
        Instance = this;
        GenerateGrid();
       // LoadLevel(level1, 1);
       // LoadLevel(level2, 2);
       // LoadLevel(level3, 3);
    }

    public void GenerateGrid()
    {
        _pipe = new Dictionary<Vector2, Pipe>();
        for (int x = 0; x < LevelTestingW; x++)
        {
            for (int y = 0; y < LevelTestingH; y++)
            {
              
                var spawnedTile = Instantiate(emptyPipe, new Vector3(x, 1, y), Quaternion.identity);
                spawnedTile.name = "Pipe " + x + " " + y;
                spawnedTile.transform.parent = PipeParent.transform;
                _pipe[new Vector2(x, y)] = spawnedTile;
                    
                for (int i = 0; i < puntosFinales.Length; i++)
                {
                    if (puntosFinales[i].x ==x &&puntosFinales[i].y==y)
                    {
                        Debug.Log("PUNTO SCORE");
                        var pipeline = GetPipeAtPosition(new Vector2(x, y));
                        spawnedTile = Instantiate(EndPipe, new Vector3(x, 1, y), Quaternion.identity);
                        spawnedTile.name = "Pipe " + x + " " + y;



                        spawnedTile.transform.parent = PipeParent.transform;
                        spawnedTile.Init(0, 0);
                        _pipe[new Vector2(x, y)] = spawnedTile;
                        Destroy(pipeline.gameObject);
                    }
                }
               
                
                
        }
        }

    }

    //Paso Posicion devuelve Tile
    public Pipe GetPipeAtPosition(Vector2 pos)
    {
        if (_pipe.TryGetValue(pos, out Pipe pipe))
        {
            

            return pipe;
        }
        else
        {
           // Debug.Log("Tile no accesible");
            return null;
        }
    }
    //Paso Tile me devuelve la posicion
    public Vector2 GetPositionOfPipe(Pipe mytile)
    {
        return _pipe.FirstOrDefault(x => x.Value == mytile).Key;
    }

    

    public void DesactivarSeleciones()
    {
        foreach (Pipe pipe in _pipe.Values)
        {
            pipe.UNselecteTile();
        }
    }

    public void BorrarPipe(Vector2 pos)
    {
        if (GetPipeAtPosition(pos) == null)
        {
            Debug.Log("YEET");
            return;
        }
        Debug.Log(pos);
        GetPipeAtPosition(pos).gameObject.transform.position = new Vector3(0, 0, 0);
       // Destroy(GetTileAtPosition(pos).gameObject);
        _pipe[pos] = null;
    }

    public void RefreshWater()
    {
        foreach (Pipe pipe in _pipe.Values)
        {

        }
    }

    public void ReCheckConectionsToWaterSource()
    {
        foreach (Pipe pipe in _pipe.Values)
        {
            pipe.DesactivateWater();
        }

        foreach (Pipe pipe in _pipe.Values)
        {
            if (pipe.WaterSource == true)
            {
                pipe.CheckConections();
            }

        }

        foreach (Pipe pipe in _pipe.Values)
        {
            if (pipe.RunningWater &&pipe.Endgame == false)
            {
                pipe.ActivateNewMats();
            }
            if (pipe.Endgame == true)
            {
                Debug.Log("HEHEHE");
                pipe.CheckConections();
            }
        }
    }

    public void LoadLevel(int level)
    {
        GameObject g;
        for (var i = PipeParent.transform.childCount - 1; i >= 0; i--)
        {
            Object.Destroy(PipeParent.transform.GetChild(i).gameObject);
        }

        int temporalW, temporalH;
        GameObject temporalGO;
        switch (level)
        {
            case 1:
                temporalGO= Instantiate(level1);
                temporalW = _width;
                temporalH = _height;
                break;
            case 2:
                temporalGO = Instantiate(level2);

                temporalW = _width2;
                temporalH = _height2;
                break;
            case 3:
                temporalGO = Instantiate(level3);

                temporalW = _width3;
                temporalH = _height3;
                break;
            case 4:
                temporalGO = Instantiate(level4);

                temporalW = _width4;
                temporalH = _height4;
                break;
            case 5:
                temporalGO = Instantiate(level5);

                temporalW = _width5;
                temporalH = _height5;
                break;
            default:
                temporalGO = Instantiate(level1);
                temporalW = 0;
                temporalH = 0;
                break;

        }

        _pipe = new Dictionary<Vector2, Pipe>();
        
        for (int x = 0; x < temporalW; x++)
        {
            for (int y = 0; y < temporalH; y++)
            {
                if (x == 0 && y == 0)
                {
                    Debug.Log("Pipe " + x + " " + y);
                    var spawnedTile = temporalGO.transform.Find("Pipe " + x + " " + y).gameObject.GetComponent<Pipe>();
                    if (spawnedTile)
                    {
                        spawnedTile.name = "Pipe " + x + " " + y;
                        spawnedTile.transform.parent = PipeParent.transform;
                        spawnedTile.Init(0, 0);
                        _pipe[new Vector2(x, y)] = spawnedTile;
                    }

                }
                else
                {

                    var spawnedTile = temporalGO.transform.Find("Pipe " + x + " " + y).gameObject.GetComponent<Pipe>();
                    if (spawnedTile)
                    {
                        spawnedTile.name = "Pipe " + x + " " + y;
                        spawnedTile.transform.parent = PipeParent.transform;
                        spawnedTile.Init(0, 0);
                        _pipe[new Vector2(x, y)] = spawnedTile;
                    }
                }
            }
        }
    }
}

