using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseCtrl : BaseUI, UIMgr.ILoadUIListener
{
    private int mCurrentLevel;
    private int mDieNum = 0;
    private readonly List<string> mFindNames = new List<string>() {"BtnBack", "BtnMenu", "BtnRestart"};
    private Text mScoreText;

    protected override void OnInit()
    {
        mCurrentLevel = LevelMgr.Instance.mCurrentLevel;


        List<Transform> findTrans = new List<Transform>();
        ComUtil.GetTransformInChild(mFindNames, CacheTransform, ref findTrans);
        Time.timeScale = 0;
        for (int i = 0; i < findTrans.Count; i++)
        {
            if (findTrans[i].name.Equals(mFindNames[0]))
            {
                Button btn = findTrans[i].GetComponent<Button>();
                btn.onClick.AddListener(OnBackClick);
            }
            else if (findTrans[i].name.Equals(mFindNames[1]))
            {
                Button btn = findTrans[i].GetComponent<Button>();
                btn.onClick.AddListener(OnMenuClick);
            }
            else if (findTrans[i].name.Equals(mFindNames[2]))
            {
                Button btn = findTrans[i].GetComponent<Button>();
                btn.onClick.AddListener(OnRestartClick);
            }
        }
    }

    private void OnMenuClick()
    {
        Time.timeScale = 1;
        MusicManager.Instance.StopBGM();
        UIMgr.Instance.DestroyUI(UIDef.GetLevelName(mCurrentLevel));

        UIMgr.Instance.ShowUI(UIDef.SelectLevelUI, typeof(SelectLevelCtrl), this);
        UIMgr.Instance.DestroyUI(UIDef.DieUI);
    }

    private void OnBackClick()
    {
        Time.timeScale = 1;

        UIMgr.Instance.DestroyUI(UIDef.PauseUI);

    }

    private void OnRestartClick()
    {
        Time.timeScale = 1;
        UIMgr.Instance.DestroyUI(UIDef.GetLevelName(mCurrentLevel));
        UIMgr.Instance.ShowUI(UIDef.GetLevelName(mCurrentLevel), typeof(LevelMgr), this, mCurrentLevel);
        UIMgr.Instance.DestroyUI(UIDef.DieUI);
    }


    protected override void OnAwake()
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
        UIMgr.Instance.HideUI(this.UIName);
    }

    public void Failure()
    {
    }

    protected override void OnShow(object param)
    {
        try
        {
        }
        catch (System.Exception ex)
        {
        }
    }
}