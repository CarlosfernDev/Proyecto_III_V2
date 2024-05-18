using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class ODS7Actions
{
    public static Action OnCloudDelivered;
    public static Action OnFactoryDisabled;
    public static Action OnCloudSpawned;
    public static Action<CloudSpawner> OnRequestCloud;
}
