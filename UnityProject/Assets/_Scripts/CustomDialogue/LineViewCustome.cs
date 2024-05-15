using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using Yarn;
using System;

public class LineViewCustome : LineView
{
    private float TimeReference;
    public override void UserRequestedViewAdvancement()
    {
        if (Time.time - TimeReference < 0.4f) return;
        base.UserRequestedViewAdvancement();
    }

    public void ResetTimer()
    {
        TimeReference = Time.time;
    }
    
}
