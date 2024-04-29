using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimerMinigame : MonoBehaviour
{
    public float TimerValue;
    public UnityEvent<string> UpdateText;
    public UnityEvent OnTimerOver;

    private float ExtraTime;
    private float TimeReference;
    private bool isCountdown = false;

    private float Value;

    private void Update()
    {
        if (!isCountdown)
            return;

        Value = Mathf.Clamp((TimerValue + ExtraTime) - (Time.time - TimeReference), 0, TimerValue + ExtraTime);

        if (UpdateText != null)
        {
            UpdateText.Invoke(GetTimeInSeconds());
        }

        if (Value > 0) return;

        TimerEnd();

    }

    private void TimerEnd()
    {
        isCountdown = false;

        if (OnTimerOver != null)
        {
            OnTimerOver.Invoke();
        }
    }

    public void PreSetTimmer()
    {
        Value = TimerValue;
        if (UpdateText != null)
        {
            UpdateText.Invoke(GetTimeInSeconds());
        }
    }

    public void SetTimer()
    {
        TimeReference = Time.time;
        isCountdown = true;
    }

    public void RestartTimer()
    {
        TimeReference = Time.time;
    }

    public void PauseTimer()
    {
        isCountdown = false;
    }

    public void AddTime(float value)
    {
        ExtraTime += value;
    }

    public void RestTime(float value)
    {
        ExtraTime -= value;

    }

    public float GetTimeInFloat()
    {
        return Value;
    }

    public string GetTimeInSeconds()
    {
        int minutos = Mathf.FloorToInt(Value / 60);
        int segundos = Mathf.FloorToInt(Value % 60);
        float milisegundos = (Value * 100) % 100;
        milisegundos = Mathf.Floor(milisegundos);

        return string.Format("{0:00}:{1:00}:{2:00}", minutos, segundos, milisegundos);
    }
}
