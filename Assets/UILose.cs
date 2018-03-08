using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UILose : UIBase {

    public Text text_iq;

    public override void DoOnEntering()
    {
        transform.DOLocalMoveY(0, 1f);
        int IQ = Random.Range(-200, 500);
        text_iq.text = IQ.ToString();
    }

    public override void DoOnPausing()
    {

    }

    public override void DoOnResuming()
    {
        int IQ = Random.Range(-200, 500);
        text_iq.text = IQ.ToString();
        CanvasGroup.alpha = 0f;


    }

    public override void DoOnExiting()
    {

    }

    public void OnRePlayClick()
    {

    }

}
