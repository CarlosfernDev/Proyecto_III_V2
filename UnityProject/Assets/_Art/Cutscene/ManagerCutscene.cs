using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class ManagerCutscene : MonoBehaviour
{
    public DialogueRunner DialogueRunner;
    public GameObject cutscene1;
    public GameObject cutscene2;
    public GameObject cutscene3;
    public GameObject cutscene4;
    public GameObject cutscene5;
    public GameObject cutscene6;
    public GameObject cutscene7;
    public GameObject cutscene8;
    public GameObject cutscene9;
    public GameObject cutscene10;
    public GameObject cutscene11;

    

    [YarnCommand("Cutscene0")]
    public void Cutscene0()
    {
        cutscene1.SetActive(false);
        cutscene2.SetActive(false);
        cutscene3.SetActive(false);
        cutscene4.SetActive(false);
        cutscene5.SetActive(false);
        cutscene6.SetActive(false);
        cutscene7.SetActive(false);
        cutscene8.SetActive(false);
        cutscene9.SetActive(false);
        cutscene10.SetActive(false);
        cutscene11.SetActive(false);
    }
    [YarnCommand("Cutscene1")]
    public void Cutscene1()
    {
        cutscene1.SetActive(true);
        cutscene2.SetActive(false);
        cutscene3.SetActive(false);
        cutscene4.SetActive(false);
        cutscene5.SetActive(false);
        cutscene6.SetActive(false);
        cutscene7.SetActive(false);
        cutscene8.SetActive(false);
        cutscene9.SetActive(false);
        cutscene10.SetActive(false);
        cutscene11.SetActive(false);
    }
    [YarnCommand("Cutscene2")]
    public void Cutscene2()
    {
        cutscene1.SetActive(false);
        cutscene2.SetActive(true);
        cutscene3.SetActive(false);
        cutscene4.SetActive(false);
        cutscene5.SetActive(false);
        cutscene6.SetActive(false);
        cutscene7.SetActive(false);
        cutscene8.SetActive(false);
        cutscene9.SetActive(false);
        cutscene10.SetActive(false);
        cutscene11.SetActive(false);
    }
    [YarnCommand("Cutscene3")]
    public void Cutscene3()
    {
        cutscene1.SetActive(false);
        cutscene2.SetActive(false);
        cutscene3.SetActive(true);
        cutscene4.SetActive(false);
        cutscene5.SetActive(false);
        cutscene6.SetActive(false);
        cutscene7.SetActive(false);
        cutscene8.SetActive(false);
        cutscene9.SetActive(false);
        cutscene10.SetActive(false);
        cutscene11.SetActive(false);
    }
    [YarnCommand("Cutscene4")]
    public void Cutscene4()
    {
        cutscene1.SetActive(false);
        cutscene2.SetActive(false);
        cutscene3.SetActive(false);
        cutscene4.SetActive(true);
        cutscene5.SetActive(false);
        cutscene6.SetActive(false);
        cutscene7.SetActive(false);
        cutscene8.SetActive(false);
        cutscene9.SetActive(false);
        cutscene10.SetActive(false);
        cutscene11.SetActive(false);
    }
    [YarnCommand("Cutscene5")]
    public void Cutscene5()
    {
        cutscene1.SetActive(false);
        cutscene2.SetActive(false);
        cutscene3.SetActive(false);
        cutscene4.SetActive(false);
        cutscene5.SetActive(true);
        cutscene6.SetActive(false);
        cutscene7.SetActive(false);
        cutscene8.SetActive(false);
        cutscene9.SetActive(false);
        cutscene10.SetActive(false);
        cutscene11.SetActive(false);
    }
    [YarnCommand("Cutscene6")]
    public void Cutscene6()
    {
        cutscene1.SetActive(false);
        cutscene2.SetActive(false);
        cutscene3.SetActive(false);
        cutscene4.SetActive(false);
        cutscene5.SetActive(false);
        cutscene6.SetActive(true);
        cutscene7.SetActive(false);
        cutscene8.SetActive(false);
        cutscene9.SetActive(false);
        cutscene10.SetActive(false);
        cutscene11.SetActive(false);
    }
    [YarnCommand("Cutscene7")]
    public void Cutscene7()
    {
        cutscene1.SetActive(false);
        cutscene2.SetActive(false);
        cutscene3.SetActive(false);
        cutscene4.SetActive(false);
        cutscene5.SetActive(false);
        cutscene6.SetActive(false);
        cutscene7.SetActive(true);
        cutscene8.SetActive(false);
        cutscene9.SetActive(false);
        cutscene10.SetActive(false);
        cutscene11.SetActive(false);
    }
    [YarnCommand("Cutscene8")]
    public void Cutscene8()
    {
        cutscene1.SetActive(false);
        cutscene2.SetActive(false);
        cutscene3.SetActive(false);
        cutscene4.SetActive(false);
        cutscene5.SetActive(false);
        cutscene6.SetActive(false);
        cutscene7.SetActive(false);
        cutscene8.SetActive(true);
        cutscene9.SetActive(false);
        cutscene10.SetActive(false);
        cutscene11.SetActive(false);
    }
    [YarnCommand("Cutscene9")]
    public void Cutscene9()
    {
        cutscene1.SetActive(false);
        cutscene2.SetActive(false);
        cutscene3.SetActive(false);
        cutscene4.SetActive(false);
        cutscene5.SetActive(false);
        cutscene6.SetActive(false);
        cutscene7.SetActive(false);
        cutscene8.SetActive(false);
        cutscene9.SetActive(true);
        cutscene10.SetActive(false);
        cutscene11.SetActive(false);
    }
    [YarnCommand("Cutscene10")]
    public void Cutscene10()
    {
        cutscene1.SetActive(false);
        cutscene2.SetActive(false);
        cutscene3.SetActive(false);
        cutscene4.SetActive(false);
        cutscene5.SetActive(false);
        cutscene6.SetActive(false);
        cutscene7.SetActive(false);
        cutscene8.SetActive(false);
        cutscene9.SetActive(false);
        cutscene10.SetActive(true);
        cutscene11.SetActive(false);
    }
    [YarnCommand("Cutscene11")]
    public void Cutscene11()
    {
        cutscene1.SetActive(false);
        cutscene2.SetActive(false);
        cutscene3.SetActive(false);
        cutscene4.SetActive(false);
        cutscene5.SetActive(false);
        cutscene6.SetActive(false);
        cutscene7.SetActive(false);
        cutscene8.SetActive(false);
        cutscene9.SetActive(false);
        cutscene10.SetActive(false);
        cutscene11.SetActive(true);
    }
}
