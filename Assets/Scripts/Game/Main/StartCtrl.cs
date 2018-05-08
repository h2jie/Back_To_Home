using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class StartCtrl : BaseUI, UIMgr.ILoadUIListener
{
    private List<string> mFindNames = new List<string>()
    {
        "BtnStart",
        "BtnSettings",
        "BtnAbout",
        "BtnOut"
    };


    protected override void OnInit()
    {
        List<Transform> findTrans = new List<Transform>();
        ComUtil.GetTransformInChild(mFindNames, CacheTransform, ref findTrans);

        for (int i = 0; i < findTrans.Count; i++)
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
            //Clicar boton de start//
        }
        else if (arg0.name.Equals(mFindNames[1]))
        {
            UIMgr.Instance.ShowUI(UIDef.SettingsUI, typeof(SettingsCtrl), this);
            //Clicar boton del seeting//
        }
        else if (arg0.name.Equals(mFindNames[2]))
        {
            UIMgr.Instance.ShowUI(UIDef.AboutUI, typeof(AboutCtrl), this);
        }
        else if (arg0.name.Equals(mFindNames[3]))
        {
            Application.Quit();
        }

        //throw new System.NotImplementedException();
    }

    protected override void OnAwake()
    {
    }


    protected override void OnShow(object param)
    {
    }

    protected override void OnHide()
    {
    }

    protected override void OnDestroy()
    {
    }


    public void FiniSh(BaseUI ui)
    {
        if (ui.UIName == UIDef.SettingsUI || ui.UIName == UIDef.SelectLevelUI || ui.UIName == UIDef.AboutUI)
        {
            UIMgr.Instance.HideUI(this.UIName);
        }
    }

    public void Failure()
    {
    }
}