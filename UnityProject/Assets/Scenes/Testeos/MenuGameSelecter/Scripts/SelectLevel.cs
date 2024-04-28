using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectLevel : MonoBehaviour
{
   public void NextLevel(int i)
    {
        MySceneManager.Instance.NextScene(i,1, 1,0);
    }
}
