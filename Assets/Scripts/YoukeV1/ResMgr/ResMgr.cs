using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;

public class ResMgr : EventNode, IEventListener
{
    private Dictionary<string, string> mAssetPathDic = new Dictionary<string, string>();

    public string GetFileFullName(string assetName)
    {
        string parent = "";
        if (!GetParentPathName(assetName, ref parent))
        {
            Log.Error("Archivo que necesita cargar no está en 'Resources' assetName =" + assetName);
        }

        return parent == "" ? assetName : parent + "/" + assetName;
    }

    /// <summary>
    /// Obtener el directorio principal del recurso
    /// </summary>
    /// <param name="assetName">nombre de asset</param>
    /// <param name="r">string vuele</param>
    /// <returns></returns>
    public bool GetParentPathName(string assetName, ref string r)
    {
        if (mAssetPathDic.Count == 0)
        {
            UnityEngine.TextAsset tex = Resources.Load<TextAsset>("res");
            StringReader sr = new StringReader(tex.text);
            string fileName = sr.ReadLine();
            while (fileName != null)
            {
                //Debug.Log("fileName =" + fileName);
                string[] ss = fileName.Split('=');
                mAssetPathDic.Add(ss[0], ss[1]);
                fileName = sr.ReadLine();
            }
        }

        if (mAssetPathDic.ContainsKey(assetName))
        {
            r = mAssetPathDic[assetName];
            return true;
        }
        else
        {
            return false;
        }
    }

    
    private Dictionary<string, AssetInfo> mDicAaaet = new Dictionary<string, AssetInfo>();

    private int mProcessorCount = 0;

    private static ResMgr mInstance;

    public static ResMgr Instance
    {
        get { return mInstance; }
    }


    // Awake is called when the script instance is being loaded.
    void Awake()
    {
        mInstance = this;
        DontDestroyOnLoad(this.gameObject);
        AttachEventListener(EventDef.ResLoadFinish, this);
        mProcessorCount = SystemInfo.processorCount > 0 && SystemInfo.processorCount <= 8
            ? SystemInfo.processorCount
            : 1;
    }

    void OnDestroy()
    {
        if (Instance != null)
        {
            Instance.DetachEventListener(EventDef.ResLoadFinish, this);
        }
    }

    /// <summary>
    /// Lista que está cargando
    /// </summary>
    private List<RequestInfo> mInLoads = new List<RequestInfo>();

    /// <summary>
    /// lista de cola
    /// </summary>
    private Queue<RequestInfo> mWaitting = new Queue<RequestInfo>();

    
    
    public Stack<List<string>> mAssetStack = new Stack<List<string>>();

    #region cargar assets

    public void Load(string assetName, IResLoadListener listener, Type type = null, bool isKeepInMemory = false,
        bool isAsync = true)
    {
        if (mDicAaaet.ContainsKey(assetName))
        {
            listener.Finish(mDicAaaet[assetName].asset);
            return;
        }

        if (isAsync)
        {
            LoadAsync(assetName, listener, isKeepInMemory, type);
        }
    }

    #endregion


    private void LoadAsync(string assetName, IResLoadListener listener, bool isKeepInMemory, Type type)
    {
        for (int i = 0; i < mInLoads.Count; i++)
        {
            if (mInLoads[i].assetName == assetName)
            {
                mInLoads[i].AddListener(listener);
                return;
            }
        }

        foreach (RequestInfo info in mWaitting)
        {
            if (info.assetName == assetName)
            {
                info.AddListener(listener);
                return;
            }
        }

        RequestInfo requestInfo = new RequestInfo();
        requestInfo.assetName = assetName;
        requestInfo.AddListener(listener);
        requestInfo.isKeepInMemory = isKeepInMemory;
        requestInfo.type = type;
        mWaitting.Enqueue(requestInfo);
    }

    public void AddAssetToName(string assetName)
    {
        if (mAssetStack.Count == 0)
        {
            mAssetStack.Push(new List<string>() {assetName});
        }

        List<string> list = mAssetStack.Peek();
        list.Add(assetName);
    }


    void Update()
    {
        if (mInLoads.Count > 0)
        {
            for (int i = mInLoads.Count - 1; i >= 0; i--)
            {
                if (mInLoads[i].IsDone)
                {
                    RequestInfo info = mInLoads[i];
                    SendEvent(EventDef.ResLoadFinish, info);
                    mInLoads.RemoveAt(i);
                }
            }
        }

       while (mInLoads.Count < mProcessorCount && mWaitting.Count > 0)
        {
            RequestInfo info = mWaitting.Dequeue();
            mInLoads.Add(info);
            info.LoadAsync();
        }
    }


    public bool HandleEvent(int id, object param1, object param2)
    {
        switch (id)
        {
            case EventDef.ResLoadFinish:
                RequestInfo info = param1 as RequestInfo;
                if (info != null)
                {
                    if (info.Asset != null)
                    {
                        AssetInfo asset = new AssetInfo();
                        asset.isKeepInMemory = info.isKeepInMemory;
                        asset.asset = info.Asset;
                        if (!mDicAaaet.ContainsKey(info.assetName))
                        {
                            mDicAaaet.Add(info.assetName, asset);
                        }

                        for (int i = 0; i < info.linsteners.Count; i++)
                        {
                            if (info.linsteners[i] != null)
                            {
                                info.linsteners[i].Finish(info.Asset);
                            }
                        }

                        AddAssetToName(info.assetName);
                    }
                }
                else
                {
                    for (int i = 0; i < info.linsteners.Count; i++)
                    {
                        if (info.linsteners[i] != null)
                        {
                            info.linsteners[i].Failure();
                        }
                    }
                }

                return false;
        }

        return false;
    }

    public int EventPriority()
    {
        return 0;
    }
}