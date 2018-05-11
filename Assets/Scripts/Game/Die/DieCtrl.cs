using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using DG.Tweening;

public class DieCtrl : BaseUI, UIMgr.ILoadUIListener
{
    private int mCurrentLevel;
    private int mDieNum = 0;
    private readonly List<string> mFindNames = new List<string>() {"Score", "BtnRestarat"};
    private Text mScoreText;

    public int scoreFinal = 250;

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
            else
            {
                Button btn = findTrans[i].GetComponent<Button>();
                btn.onClick.AddListener(OnRestartClick);
            }
        }
    }

    private void OnRestartClick()
    {
        UIMgr.Instance.DestroyUI(UIDef.GetLevelName(mCurrentLevel));
        UIMgr.Instance.ShowUI(UIDef.GetLevelName(mCurrentLevel), typeof(LevelMgr), this, mCurrentLevel);
    }

    void Awake()
    {
        mDieNum = 0;
    }

    protected override void OnAwake()
    {
    }
    
    
    protected override void OnShow(object param)
    {
        SoundManager.Instance.PlaySound("loser");
        System.Threading.Thread.Sleep(400);

        int level = (int) param;
        if (level != mCurrentLevel)
        {
            mDieNum = 0;
        }
        else
        {
            mDieNum += 1;
        }

        scoreFinal = 250 - 50 * mDieNum;
        mScoreText.text = scoreFinal.ToString();

        string path = "Assets/Resources/nota.txt";
        //Write some text to the nota.txt file
        StreamWriter writer = new StreamWriter(path, false);
        writer.Write(scoreFinal);
        writer.Close();
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
}