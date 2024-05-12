using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;
using static CloudSpawner;

public class Granjas : LInteractableParent
{
    public enum FarmState {Disable, Wait, WaitPlayerInteraction, LoadSeed, WaitingWater, Recolect }

    public List<GameObject> GranjasRender;
    private GameObject ActualRender;

    public GameObject VegetalPrefab;

    public FarmState _farmState = FarmState.Disable;

    [SerializeField] private Slider SliderMain;

    private float TimeReferenceSeed;
    private float TimeReferenceSecondary;
    private float TimeExtraWater;

    public int waterindex = 0;

    [Header("Visual")]
    public Animator animatorBocadillo;
    public Animator animatorSlider;
    public Animator animatorRegadera;
    public List<BocadillosGranjasScriptables> AnimatorList;
    public string TriggerAnimatorEnabled;
    public string TriggerAnimatorDisabled;
    public string BoolAnimatorBright;
    public string TriggerAnimatorBright;
    private bool isBrightEnable;

    [Header("VFX")]
    [SerializeField] private VisualEffect _PlantarVFX;

    private void Start()
    {
        animatorBocadillo.SetTrigger(TriggerAnimatorDisabled);
        animatorSlider.SetTrigger(TriggerAnimatorDisabled);

        ActualRender = GranjasRender[0];
        ChangeRender(0);

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

        if (isBrightEnable)
        {
            animatorBocadillo.SetBool(BoolAnimatorBright, false);
            animatorBocadillo.SetTrigger(TriggerAnimatorBright);
            isBrightEnable = false;
        }


        animatorBocadillo.SetTrigger(TriggerAnimatorDisabled);

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
                    Vector3 VectorPlayer = GameManager.Instance.playerScript.transform.position;
                    Vector3 targetPoint = new Vector3(VectorPlayer.x, transform.position.y, VectorPlayer.z) - transform.position;
                    Quaternion targetRotation = Quaternion.LookRotation(targetPoint, Vector3.up);

                    animatorRegadera.transform.parent.transform.rotation = targetRotation;
                    animatorRegadera.transform.parent.transform.eulerAngles = animatorRegadera.transform.parent.transform.eulerAngles + -45 * Vector3.up;

                    animatorRegadera.SetTrigger("Reset");
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
        SliderMain.gameObject.SetActive(true);
        TimeReferenceSeed = Time.time;

        if (_farmState == FarmState.WaitPlayerInteraction)
        {
            waterindex = 0;
            TimeExtraWater = 0;
            animatorSlider.SetTrigger(TriggerAnimatorEnabled);
            _PlantarVFX.Play();
        }

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

        if (!isBrightEnable && (ODS2Singleton.Instance.WaterMaxTimer - ODS2Singleton.Instance.BirghtTimeLeft) < TimeLoad)
        {
            animatorBocadillo.SetBool(BoolAnimatorBright, true);
            animatorBocadillo.SetTrigger(TriggerAnimatorBright);
            isBrightEnable = true;
        }

        if(TimeLoad >= ODS2Singleton.Instance.WaterMaxTimer)
        {
            return true;
        }

        return false; 
    }

    #endregion

    void FarmDestroyed()
    {
        ChangeRender(1);
        animatorBocadillo.runtimeAnimatorController = AnimatorList[(int)FarmState.WaitPlayerInteraction]._animator;
        animatorBocadillo.SetTrigger(TriggerAnimatorEnabled);
        animatorSlider.SetTrigger(TriggerAnimatorDisabled);

        if (isBrightEnable)
        {
            animatorBocadillo.SetBool(BoolAnimatorBright, false);
            animatorBocadillo.SetTrigger(TriggerAnimatorBright);
            isBrightEnable = false;
        }

        IsInteractable = true;

        ODS2Singleton.Instance.timer.RestTime(ODS2Singleton.Instance.ReduceTime);

        _farmState = FarmState.WaitPlayerInteraction;
    }

    void MainSliderComplete()
    {
        ChangeRender(2);
        animatorBocadillo.runtimeAnimatorController = AnimatorList[(int)FarmState.Recolect]._animator;
        animatorBocadillo.SetTrigger(TriggerAnimatorEnabled);
        animatorSlider.SetTrigger(TriggerAnimatorDisabled);

        TimeReferenceSecondary = Time.time;

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
        _farmState = FarmState.WaitPlayerInteraction;
        IsInteractable = true;
        waterindex = 0;
    }

    bool IsWatereable(float value)
    {
        if (waterindex > ODS2Singleton.Instance.WaterTime.Length - 1)
            return false;

        if (ODS2Singleton.Instance.WaterTime[waterindex] > value) return false;

        animatorBocadillo.runtimeAnimatorController = AnimatorList[(int)FarmState.WaitingWater]._animator;
        animatorBocadillo.SetTrigger(TriggerAnimatorEnabled);

        _farmState = FarmState.WaitingWater;
        IsInteractable = true;
        waterindex += 1;

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
