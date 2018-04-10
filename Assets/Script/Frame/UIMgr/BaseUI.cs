using UnityEngine;
using System.Collections;

public class BaseUI : MonoBehaviour 
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


    protected virtual void OnInit() { }
    protected virtual void OnAwake() { }

    protected virtual void OnShow(object param) { }


    protected virtual void OnHide() { }

    protected virtual void OnDestroy() { }
}
