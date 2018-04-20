using UnityEngine;
using System.Collections;

public class LevelMgr : BaseUI , IEventListener , UIMgr.ILoadUIListener
{

    private int mCurrentLevel;


    protected override void OnInit()
    {

        if (AppMgr.Instance)
        {
            AppMgr.Instance.AttachEventListener((int)EventDef.LevelEvent.PlayerDie, this);
            AppMgr.Instance.AttachEventListener((int)EventDef.LevelEvent.GameOver, this);
            AppMgr.Instance.AttachEventListener((int)EventDef.LevelEvent.SaveGame, this);
        }
    }


    protected override void OnShow(object param) 
    {
        mCurrentLevel = (int)param;
    }

    protected override void OnHide() 
    { 
    }

    protected override void OnDestroy() 
    {
        if (AppMgr.Instance)
        {
            AppMgr.Instance.DetachEventListener((int)EventDef.LevelEvent.PlayerDie, this);
            AppMgr.Instance.DetachEventListener((int)EventDef.LevelEvent.GameOver, this);
            AppMgr.Instance.DetachEventListener((int)EventDef.LevelEvent.SaveGame, this);
        }
    }


    public bool HandleEvent(int id, object param1, object param2)
    {
        EventDef.LevelEvent evid = (EventDef.LevelEvent)id;
        switch (evid)
        {
            case EventDef.LevelEvent.PlayerDie:

                UIMgr.Instance.ShowUI(UIDef.DieUI, typeof(DieCtrl), this,mCurrentLevel);

                return false;
            case EventDef.LevelEvent.GameOver:
                Log.Debug("---------Game Complete！");

                if (!AppMgr.Instance.OpenLevels.Contains(mCurrentLevel + 1))
                {
                    AppMgr.Instance.AddOpenLevel(mCurrentLevel + 1);
                }

                return false;
        }
        return false;
    }

    public int EventPriority()
    {
        return 0;
    }

    public void FiniSh(BaseUI ui)
    {
        //throw new System.NotImplementedException();
    }

    public void Failure()
    {
        //throw new System.NotImplementedException();
    }

    protected override void OnAwake()
    {
        
    }
}
