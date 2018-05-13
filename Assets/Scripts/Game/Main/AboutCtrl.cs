using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class AboutCtrl : BaseUI, UIMgr.ILoadUIListener
{
    private List<string> mFindNames = new List<string>()
    {
        "BtnBack",
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
        UIMgr.Instance.ShowUI(UIDef.StartUI, typeof(StartCtrl), this);

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
        UIMgr.Instance.HideUI(base.UIName);
    }

    public void Failure()
    {
    }
}