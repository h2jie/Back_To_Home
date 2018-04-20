using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StartCtrl : BaseUI , UIMgr.ILoadUIListener
{

    private List<string> mFindNames = new List<string>()
    {
        "BtnStart",
        "BtnSettings",
        "BtnAbout"
    };

    /// <summary>
    /// inicializa interface
    /// </summary>
    protected override void OnInit()
    {
        List<Transform> findTrans = new List<Transform>();
        ComUtil.GetTransformInChild(mFindNames, CacheTransform, ref findTrans);


        for (int i = 0; i < findTrans.Count;i++ )
        {
            if (findTrans[i].name.StartsWith("Btn"))
            {
                EventTrigger btn = findTrans[i].GetComponent<EventTrigger>();
                EventTrigger.Entry ev = new EventTrigger.Entry();
                ev.callback.AddListener((BaseEventData arg0) => { OnBtnClick(btn.gameObject); });
                ev.eventID = EventTriggerType.PointerClick;

                
                btn.triggers.Add(ev);
            }
        }
    }

    private void OnBtnClick(GameObject arg0)
    {
        
        if (arg0.name.Equals(mFindNames[0]))
        {
            UIMgr.Instance.ShowUI(UIDef.SelectLevelUI, typeof(SelectLevelCtrl), this);
            //Clickar boton de inicio//
        }
        else if (arg0.name.Equals(mFindNames[1]))
        {
            UIMgr.Instance.ShowUI(UIDef.SettingsUI,typeof(SettingsCtrl),this);
            //Clickar boton de settings//
        }
        else if (arg0.name.Equals(mFindNames[2]))
        {
            UIMgr.Instance.ShowUI(UIDef.AboutUI, typeof(AboutCtrl), this);
            //Clickar boton de About//
        }
        //throw new System.NotImplementedException();
    }

    protected override void OnAwake() 
    {

    }

    /// <summary>
    /// muestra interface actual
    /// </summary>
    protected override void OnShow(object param) 
    {

    }

    /// <summary>
    /// ocultar pantalla actual
    /// </summary>
    protected override void OnHide() 
    {

    }

    /// <summary>
    /// borrar pantalla actual 
    /// </summary>
    protected override void OnDestroy()
    {

    }


    public void FiniSh(BaseUI ui)
    {
        if (ui.UIName == UIDef.SettingsUI)
        {
            UIMgr.Instance.HideUI(this.UIName);
        }
        else if (ui.UIName == UIDef.AboutUI)
        {
            UIMgr.Instance.HideUI(this.UIName);
        }
        else if(ui.UIName == UIDef.SelectLevelUI)
        {
            UIMgr.Instance.HideUI(this.UIName);
        }
    }

    public void Failure()
    {
    }
}
