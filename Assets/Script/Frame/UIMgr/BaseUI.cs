using UnityEngine;
using System.Collections;

public abstract class BaseUI : MonoBehaviour 
{

    [HideInInspector]
    public string UIName;

    private Transform mTransform;
    public Transform CacheTransform
    {
        get
        {
            if (mTransform == null) mTransform = this.transform;
            return mTransform;
        }
    }

    private GameObject mGo;
    public GameObject CacheGameObject
    {
        get
        {
            if (mGo == null) mGo = this.gameObject;
            return mGo;
        }
    }


    public void Show(object param)
    {
        CacheGameObject.SetActive(true);
        OnShow(param);
    }


    public void Hide()
    {
        CacheGameObject.SetActive(false);
        OnHide();
    }

    [HideInInspector]
    public Canvas mainCanvas = null;



    void Awake()
    {
        OnAwake();
    }




    public void UIInit()
    {
        if (mainCanvas == null)
        {
            mainCanvas = this.GetComponent<Canvas>();
        }
        if (mainCanvas != null)
        {
            mainCanvas.worldCamera = AppMgr.Instance.MainCamera;
        }
        mainCanvas.sortingOrder = UIDef.GetUIOrderLayer(UIName);
        OnInit();
    }


    protected abstract void OnInit();
    protected abstract void OnAwake();




    protected abstract void OnShow(object param);



    protected abstract void OnHide();


    protected abstract void OnDestroy();
}
