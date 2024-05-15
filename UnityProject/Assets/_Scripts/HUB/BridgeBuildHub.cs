using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeBuildHub : SwapBridge
{
    public override void Interact()
    {
        if (GameManager.Instance == null) return;
        if (GameManager.Instance.state == GameManager.GameState.Puentes) return;
        base.Interact();
    }
}
