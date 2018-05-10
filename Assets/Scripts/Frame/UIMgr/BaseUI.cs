using UnityEngine;

public abstract class BaseUI : MonoBehaviour
{
    [HideInInspector] public string UIName;

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

    /// <summary>
    /// Muestra UI actual
    /// </summary>
    /// <param name="param"></param>
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

    [HideInInspector] public Canvas mainCanvas = null;

    /// <summary>
    /// Enlace el script y active el método que el objeto del juego llamará
    /// </summary>
    void Awake()
    {
        OnAwake();
    }

    /// <summary>
    /// Inicializar UI y buscar los componentes que están en el UI
    /// </summary>
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