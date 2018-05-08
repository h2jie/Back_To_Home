using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class SettingsCtrl : BaseUI, UIMgr.ILoadUIListener
{
    private List<string> mFindNames = new List<string>()
    {
        "BtnSound",
        "BtnMusic",
        "BtnBack",
    };

    private GameObject mBtnSound = null;
    private GameObject mBtnMusic = null;

 
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

                if (findTrans[i].name.Equals(mFindNames[0]))
                {
                    mBtnSound = findTrans[i].transform.GetChild(0).gameObject;

                }
                else if (findTrans[i].name.Equals(mFindNames[1]))
                {
                    mBtnMusic = findTrans[i].transform.GetChild(0).gameObject;

                }
            }
        }
    }

    private void OnBtnClick(GameObject arg0)
    {
        if (arg0.name.Equals(mFindNames[0]))
        {
            Transform child = arg0.transform.GetChild(0);
            child.gameObject.SetActive(!child.gameObject.activeSelf);
            AppMgr.Instance.SoundValue = child.gameObject.activeSelf;

            if (!child.gameObject.activeSelf)
            {
                SoundManager.Instance.Mute = true;
                Log.Debug("muteado");
            }
            else
            {
                Log.Debug("no muteado");
                SoundManager.Instance.Mute = false;
            }
        }
        else if (arg0.name.Equals(mFindNames[1]))
        {

            
            Transform child = arg0.transform.GetChild(0);
            child.gameObject.SetActive(!child.gameObject.activeSelf);
            AppMgr.Instance.MusicValue = child.gameObject.activeSelf;

            if (!child.gameObject.activeSelf)
            {
                MusicManager.Instance.Mute = true;
            }
            else
            {
                MusicManager.Instance.Mute = false;
            }
        }
        else
        {
            UIMgr.Instance.ShowUI(UIDef.StartUI, typeof(StartCtrl), this);
        }

        //throw new System.NotImplementedException();
    }


    protected override void OnAwake()
    {
    }

    protected override void OnShow(object param)
    {
        mBtnSound.SetActive(AppMgr.Instance.SoundValue);
        mBtnMusic.SetActive(AppMgr.Instance.MusicValue);
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