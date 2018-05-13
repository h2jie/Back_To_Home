using System.Collections;
using UnityEngine;
using System.IO;

public class LevelMgr : BaseUI, IEventListener, UIMgr.ILoadUIListener
{
    
    public int mCurrentLevel;

    private static LevelMgr instance;

    public static LevelMgr Instance
    {
        get { return instance; }
    }


    protected override void OnInit()
    {
        if (AppMgr.Instance.MusicValue)
        {
            MusicManager.Instance.PlayBGM("backgroundAudio");

        }
        else
        {
            MusicManager.Instance.Mute = true;

        }
        
        if (!AppMgr.Instance.SoundValue)
        {
            SoundManager.Instance.Mute = true;

        }


        if (AppMgr.Instance)
        {
            AppMgr.Instance.AttachEventListener((int) EventDef.LevelEvent.PlayerDie, this);
            AppMgr.Instance.AttachEventListener((int) EventDef.LevelEvent.GameOver, this);
            AppMgr.Instance.AttachEventListener((int) EventDef.LevelEvent.SaveGame, this);
        }
    }

    void Awake()
    {
        instance = this;
        mCurrentLevel = this.mCurrentLevel;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            UIMgr.Instance.ShowUI(UIDef.PauseUI, typeof(PauseCtrl), this);
        }
    }

    protected override void OnShow(object param)
    {
        if (param != null)
            mCurrentLevel = (int) param;
        Log.Debug(mCurrentLevel);

        string path = "Assets/Resources/levelNumber.txt";
        //Write some text to the nota.txt file
        StreamWriter writer = new StreamWriter(path, false);
        writer.Write(mCurrentLevel);
        writer.Close();
    }

    protected override void OnHide()
    {
    }

    protected override void OnDestroy()
    {
        if (AppMgr.Instance)
        {
            AppMgr.Instance.DetachEventListener((int) EventDef.LevelEvent.PlayerDie, this);
            AppMgr.Instance.DetachEventListener((int) EventDef.LevelEvent.GameOver, this);
            AppMgr.Instance.DetachEventListener((int) EventDef.LevelEvent.SaveGame, this);
        }
    }


    public bool HandleEvent(int id, object param1, object param2)
    {
        EventDef.LevelEvent evid = (EventDef.LevelEvent) id;
        switch (evid)
        {
            case EventDef.LevelEvent.PlayerDie:
                //Instantiate(DeadExplosion, HeroCtrl.Instance.heroPosition, HeroCtrl.Instance.heroRotation);
                UIMgr.Instance.ShowUI(UIDef.DieUI, typeof(DieCtrl), this, mCurrentLevel);
                return false;
            case EventDef.LevelEvent.GameOver:
                Log.Debug("---------Game Complite！");

                if (!AppMgr.Instance.OpenLevels.Contains(mCurrentLevel + 1))
                {
                    AppMgr.Instance.AddOpenLevel(mCurrentLevel + 1);
                }

                UIMgr.Instance.ShowUI(UIDef.WinUI, typeof(WinCtrl), this, mCurrentLevel);

                return false;
            case EventDef.LevelEvent.SaveGame:
            {
                GameObject go = (GameObject) param1;
                AppMgr.Instance.HeroPos = go.transform.position;
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