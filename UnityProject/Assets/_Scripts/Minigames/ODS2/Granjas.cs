using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static CloudSpawner;

public class Granjas : LInteractableParent
{
    public enum FarmState {Disable, Wait, WaitPlayerInteraction, LoadSeed, WaitingWater, Recolect }

    public List<GameObject> GranjasRender;
    private GameObject ActualRender;

    public GameObject VegetalPrefab;

    public FarmState _farmState = FarmState.Disable;

    [SerializeField] private Slider SliderMain;
    [SerializeField] private Slider SliderSecondary;

    private float TimeReferenceSeed;
    private float TimeReferenceSecondary;
    private float TimeExtraWater;

    public int waterindex = 0;

    private void Start()
    {
        ActualRender = GranjasRender[0];
        ChangeRender(0);

        SliderMain.gameObject.SetActive(false);
        SliderSecondary.gameObject.SetActive(false);
        SliderMain.maxValue = ODS2Singleton.Instance.SeedTimer;
        SliderMain.value = 0;
        IsInteractable = false;
    }

    public void StarFarm()
    {
        ChangeRender(0);
        ResetFarm();
        SetSeed();
    }

    // Update is called once per frame
    void Update()
    {
        if (!ODS2Singleton.Instance.gameIsActive)
            return;

        switch (_farmState)
        {
            case FarmState.Disable:
                {
                    return;
                }

            case FarmState.LoadSeed:
                {
                    if (UpdateCoreTimer()) 
                    {
                        MainSliderComplete();
                    };
                    break;
                }
            case FarmState.WaitingWater:
                {
                    if (UpdateSecondarySlider())
                    {
                        FarmDestroyed();
                    }
                    break;
                }
            case FarmState.Recolect:
                if (UpdateSecondarySlider())
                {
                    FarmDestroyed();
                }
                break;
            default:
                {
                    return;
                }
        }
    }

    #region Interaction with farm

    public override void Interact()
    {
        if (_farmState != FarmState.WaitPlayerInteraction && _farmState != FarmState.WaitingWater && _farmState != FarmState.Recolect)
            return;

        if (GameManager.Instance.playerScript.isEquipado && _farmState == FarmState.Recolect)
        {
            return;
        }

        SliderSecondary.gameObject.SetActive(false);

        switch (_farmState)
        {
            case FarmState.Recolect:
            {
                    GameObject go = GameObject.Find("Player");
                    if(go.GetComponent<TestInputs>().isEquipado == true) 
                    {
                        Debug.Log("Objeto ya equipado");
                        return; 
                    }
                    ODS2Singleton.Instance.AddScore(ODS2Singleton.Instance.ScoreWatering);
                    FarmComplete();
                    return;
            }
            case FarmState.WaitingWater:
                {
                    ODS2Singleton.Instance.AddScore(ODS2Singleton.Instance.ScoreWatering);
                    break;
                }
        }

        SetSeed();


        base.Interact();
    }

    void SetSeed()
    {
        ChangeRender(3);

        if (_farmState == FarmState.WaitPlayerInteraction)
        {
            waterindex = 0;
            TimeExtraWater = 0;
        }
        SliderMain.gameObject.SetActive(true);
        TimeReferenceSeed = Time.time;

        IsInteractable = false;
        _farmState = FarmState.LoadSeed;
    }

    #endregion

    #region Timers
    bool UpdateCoreTimer()
    {
        if (!SliderMain.gameObject.activeSelf)
        {
            SliderMain.gameObject.SetActive(true);
        }

        float TimeLoad = TimeExtraWater + (Time.time - TimeReferenceSeed);
        TimeLoad = Mathf.Clamp(TimeLoad, 0, ODS2Singleton.Instance.SeedTimer);

        SliderMain.value = TimeLoad;

        if (TimeLoad == ODS2Singleton.Instance.SeedTimer)
        {
            return true;
        }

        IsWatereable(TimeLoad);

        return false;
    }

    bool UpdateSecondarySlider()
    {
        float TimeLoad = Time.time - TimeReferenceSecondary;
        SliderSecondary.value = TimeLoad;

        if (SliderSecondary.value == SliderSecondary.maxValue)
            return true;

        return false; 
    }

    #endregion

    void FarmDestroyed()
    {
        ChangeRender(1);

        IsInteractable = true;

        ODS2Singleton.Instance.timer.RestTime(ODS2Singleton.Instance.ReduceTime);

        SliderSecondary.gameObject.SetActive(false);
        _farmState = FarmState.WaitPlayerInteraction;
    }

    void MainSliderComplete()
    {
        ChangeRender(2);

        TimeReferenceSecondary = Time.time;
        SliderSecondary.maxValue = ODS2Singleton.Instance.CollectingTimer;
        SliderSecondary.value = 0;

        SliderSecondary.gameObject.SetActive(true);

        IsInteractable = true;
        _farmState = FarmState.Recolect;
    }

    void FarmComplete()
    {
        _farmState = FarmState.WaitPlayerInteraction;
        SetSeed();

        GameObject vegetal = Instantiate(VegetalPrefab);
        Vegetal script = vegetal.GetComponent<Vegetal>();

        script.SetEquipableToPlayer();
        script.myFarm = this;
    }

    void ResetFarm()
    {
        SliderMain.gameObject.SetActive(false);
        _farmState = FarmState.WaitPlayerInteraction;
        IsInteractable = true;
        waterindex = 0;
    }

    bool IsWatereable(float value)
    {
        if (waterindex > ODS2Singleton.Instance.WaterTime.Length - 1)
            return false;

        if (ODS2Singleton.Instance.WaterTime[waterindex] > value) return false;

        _farmState = FarmState.WaitingWater;
        IsInteractable = true;
        waterindex += 1;

        SliderSecondary.gameObject.SetActive(true);
        SliderSecondary.value = 0;
        SliderSecondary.maxValue = ODS2Singleton.Instance.WaterMaxTimer;

        TimeReferenceSecondary = Time.time;
        TimeExtraWater = value;
        return true;
    }

    public void ChangeRender(int value)
    {
        ActualRender.SetActive(false);
        GranjasRender[value].SetActive(true);
        ActualRender = GranjasRender[value];
    }

}
