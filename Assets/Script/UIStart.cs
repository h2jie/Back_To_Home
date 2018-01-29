using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStart : UIBase
{


    public override void DoOnEntering()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
    }

    public override void DoOnPausing()
    {
    }

    public override void DoOnResuming()
    {
    }

    public override void DoOnExiting()
    {
    }

    public void GoToSetting(){
        UIManager.Instance.PushUIPanel("UISetting");

    }
}
