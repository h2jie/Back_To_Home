using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class WinCtrl : BaseUI, UIMgr.ILoadUIListener
{
    private int mCurrentLevel;
    private readonly List<string> mFindNames = new List<string>() {"Score", "BtnMenu", "BtnNext"};
    private Text mScoreText;
    private HeroCtrl hero;

    protected override void OnInit()
    {
        mCurrentLevel = LevelMgr.Instance.mCurrentLevel;

        List<Transform> findTrans = new List<Transform>();
        ComUtil.GetTransformInChild(mFindNames, CacheTransform, ref findTrans);
        for (int i = 0; i < findTrans.Count; i++)
        {
            if (findTrans[i].name.Equals(mFindNames[0]))
            {
                mScoreText = findTrans[i].GetComponent<Text>();
            }
            else if (findTrans[i].name.Equals(mFindNames[1]))
            {
                Button btn = findTrans[i].GetComponent<Button>();
                btn.onClick.AddListener(OnMenuClick);
            }
            else if (findTrans[i].name.Equals(mFindNames[2]))
            {
                Button btn = findTrans[i].GetComponent<Button>();
                btn.onClick.AddListener(OnNextClick);
            }
        }
    }

    private void OnMenuClick()
    {
        MusicManager.Instance.StopBGM();
        UIMgr.Instance.DestroyUI(UIDef.GetLevelName(mCurrentLevel));
        UIMgr.Instance.DestroyUI(UIDef.DieUI);
        UIMgr.Instance.DestroyUI(UIDef.WinUI);

        UIMgr.Instance.ShowUI(UIDef.SelectLevelUI, typeof(SelectLevelCtrl), this);
    }

    private void OnNextClick()
    {
        MusicManager.Instance.StopBGM();
        UIMgr.Instance.DestroyUI(UIDef.GetLevelName(mCurrentLevel));
        UIMgr.Instance.DestroyUI(UIDef.DieUI);
        UIMgr.Instance.DestroyUI(UIDef.WinUI);

        if (mCurrentLevel != 2)
        {
            UIMgr.Instance.ShowUI(UIDef.GetLevelName(mCurrentLevel + 1), typeof(LevelMgr), this, mCurrentLevel + 1);
        }
        else
        {
            UIMgr.Instance.ShowUI(UIDef.SelectLevelUI, typeof(SelectLevelCtrl), this);
        }
    }

    void Awake()
    {
        AppMgr.Instance.HeroPos = Vector3.zero;
    }

    protected override void OnAwake()
    {
    }

    protected override void OnShow(object param)
    {
        SoundManager.Instance.PlaySound("winner");

        string path = "Assets/Resources/nota.txt";
        string nota;
        try
        {
            //Read the text from directly from the test.txt file
            StreamReader reader = new StreamReader(path);
            nota = reader.ReadToEnd();
            reader.Close();
        }
        catch (FileNotFoundException)
        {
            nota = "250";
        }

        mScoreText.text = nota;
        File.Delete(path);
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
        SoundManager.Instance.StopSound();
    }

    public void Failure()
    {
    }
}