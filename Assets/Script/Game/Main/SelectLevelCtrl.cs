using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

public class SelectLevelCtrl : BaseUI, UIMgr.ILoadUIListener
{

    public static SelectLevelCtrl Instance{
        private set;
        get;
    }

    private List<Image> mPages = new List<Image>();//, mPage2, mPage3;

    private Button mBtnDown;
    private Button mBtnUp;

    private readonly List<string> mFindNames = new List<string>()
    {
        "Page1",
        "Page2",
        "Page3",

        "BtnBack",
        "BtnDown",
        "BtnUp"
    };

    private readonly List<string> mFindNames1 = new List<string>()
    {
        "1",
        "2",
        "3",
    };



    private class LevelBtn
    {

        public Button btn;
        public Image mImagelock;
        public int level;



        public Image Imagelock
        {
            get
            {
                return mImagelock;
            }
        }

        public LevelBtn(Button btn, int level)
        {
            this.btn = btn;
            this.level = level;
            if (!mImagelock)
            {
                Transform tran = ComUtil.FindTransformInChild(btn.transform, "Lock", true);
                if (tran != null)
                {
                    mImagelock = tran.GetComponent<Image>();
                }
            }
        }



        /// <summary>
        /// Establecer el estado abierto actual
        /// </summary>
        public void SetLock()
        {
            if (AppMgr.Instance.OpenLevels.Contains(level))
            {
                Imagelock.gameObject.SetActive(false);
                btn.enabled = true;
            }
            else
            {
                Imagelock.gameObject.SetActive(true);
                btn.enabled = false;
            }
        }


        
        /// <summary>
        /// nivel seleccionado
        /// </summary>
        public void SelectLevel()
        {
            if (Imagelock.gameObject.activeSelf)
            {
                return;
            }
            AppMgr.Instance.HeroPos = Vector3.zero;
            UIMgr.Instance.ShowUI(UIDef.GetLevelName(level), typeof(LevelMgr), SelectLevelCtrl.Instance, level);
            Log.Debug("Nivel seleccionado ：" + level);
        }
    }








    private List<LevelBtn> mLevels = new List<LevelBtn>();


    protected override void OnInit()
    {
        Instance = this;
        List<Transform> findPages = new List<Transform>();
        ComUtil.GetTransformInChild(mFindNames, CacheTransform, ref findPages);
        List<Transform> findLevels = new List<Transform>();
        for (int i = 0; i < findPages.Count; i++)
        {
            if (findPages[i].name.Equals(mFindNames[0]) || findPages[i].name.Equals(mFindNames[1]) || findPages[i].name.Equals(mFindNames[2]))
            {
                Image mPage1 = findPages[i].GetComponent<Image>();

                mPages.Add(mPage1);

                string s = findPages[i].name.Substring(findPages[i].name.Length - 1, 1);
                int page = 0;
                int.TryParse(s, out page);


                if (page != 0)
                {
                    findLevels.Clear();
                    ComUtil.GetTransformInChild(mFindNames1, findPages[i], ref findLevels);
                    for (int j = 0; j < findLevels.Count; j++)
                    {
                        
                        int level = 0;
                        int.TryParse(findLevels[j].name, out level);


                        if (level != 0)
                        {
                            Button btn = findLevels[j].GetComponent<Button>();
                            LevelBtn levelBtn = new LevelBtn(btn, level + ((page - 1) * 3));
                            btn.onClick.AddListener(levelBtn.SelectLevel);
                            mLevels.Add(levelBtn);
                        }
                    }
                }
            }else{
                Button btn = findPages[i].GetComponent<Button>();
                btn.onClick.AddListener(() => { OnBtnClick(btn); });

                if (findPages[i].name.Equals(mFindNames[4]))
                {
                    mBtnDown = btn;
                }
                else
                {
                    mBtnUp = btn;
                }
            }
        }
        mCurPage = mPages[0];
        DetectionPageBtn();

        mLevels.Sort((LevelBtn a, LevelBtn b) => 
        { 
            return a.level.CompareTo(b.level); 
        });
    }

    private Image mCurPage;

    private void OnBtnClick(Button btn)
    {
        
        if (btn.name.Equals(mFindNames[3]))
        {
            UIMgr.Instance.ShowUI(UIDef.StartUI, typeof(StartCtrl), this);
        }
        else if (btn.name.Equals(mFindNames[4]))
        {
            
            if(mCurPage == mPages[mPages.Count -1])
            {
                return;
            }

            string s = mCurPage.name.Substring(mCurPage.name.Length - 1);
            int index = 0;
            int.TryParse(s, out index);

            if (index != 0)
            {
                index -= 1;
                mCurPage.rectTransform.DOLocalMoveX(-700.0f, 0.5f);
                mCurPage = mPages[index + 1];
                mCurPage.rectTransform.DOLocalMoveX( 0.0f, 0.5f);
                DetectionPageBtn();
            }
            ///seguiente pagina
        }
        else if (btn.name.Equals(mFindNames[5]))
        {
            if (mCurPage == mPages[0])
            {
                return;
            }

            string s = mCurPage.name.Substring(mCurPage.name.Length - 1);
            int index = 0;
            int.TryParse(s, out index);

            if (index != 0)
            {
                index -= 1;
                mCurPage.rectTransform.DOLocalMoveX(700, 0.5f);
                mCurPage = mPages[index - 1];
                mCurPage.rectTransform.DOLocalMoveX(0, 0.5f);
            }
            DetectionPageBtn();
            ///pagina anterior
        }
    }
    
    void DetectionPageBtn()
    {
        Color cDown = mBtnDown.image.color;

        mBtnDown.enabled = (!(mCurPage == mPages[mPages.Count - 1]));
        cDown.a = mBtnDown.enabled ? 1.0f : (60.0f / 255.0f);

        mBtnDown.image.color = cDown;

        Color cUp = mBtnUp.image.color;

        mBtnUp.enabled = (!(mCurPage == mPages[0]));
        cUp.a = mBtnUp.enabled ? 1.0f : (60.0f / 255.0f);
        mBtnUp.image.color = cUp;
    }

    protected override void OnAwake() 
    {
    }



    protected override void OnShow(object param)
    {
        for (int i = 0; i < AppMgr.Instance.OpenLevels.Count;i++ )
        {
            Log.Debug(AppMgr.Instance.OpenLevels[i]);
        }

        for (int i = 0; i < mLevels.Count;i++ )
        {
            mLevels[i].SetLock();

        }
    }

    protected override void OnHide() {

    }

    protected override void OnDestroy() {
        Instance = null;
    }

    public void FiniSh(BaseUI ui)
    {
        UIMgr.Instance.HideUI(UIName);
    }

    public void Failure()
    {
    }
}
