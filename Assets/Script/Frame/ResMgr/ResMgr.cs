using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class ResMgr : EventNode,IEventListener 
{
    private Dictionary<string, string> mAssetPathDic = new Dictionary<string, string>();
    /// <summary>
    /// Obtener la ruta completo al recurso
    /// </summary>
    public string GetFileFullName(string assetName)
    {
        string parent = "";
        if (!GetParentPathName(assetName, ref parent))
        {
            Log.Error("No encontrado archivo assetName =" + assetName);
        }
        return parent == "" ? assetName : parent + "/" + assetName;
    }

    /// <summary>
    /// directorio de padre
    /// </summary>
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
        get
        {
            return mInstance;
        }
    }



	// Awake is called when the script instance is being loaded.
    void Awake()
    {
        mInstance = this;
        DontDestroyOnLoad(this.gameObject);
        AttachEventListener(EventDef.ResLoadFinish, this);
        mProcessorCount = SystemInfo.processorCount > 0 && SystemInfo.processorCount <= 8 ? SystemInfo.processorCount : 1;
    }

    void OnDestroy()
    {
        if (Instance != null)
        {
            Instance.DetachEventListener(EventDef.ResLoadFinish, this);
        }
    }


    private List<RequestInfo> mInLoads = new List<RequestInfo>();


    private Queue<RequestInfo> mWaitting = new Queue<RequestInfo>();


    public Stack<List<string>> mAssetStack = new Stack<List<string>>();

    #region cargarAsset
    public void Load(string assetName, IResLoadListener listener, Type type = null, bool isKeepInMemory = false, bool isAsync = true)
    {
        if (mDicAaaet.ContainsKey(assetName))
        {
            
            listener.Finish(mDicAaaet[assetName].asset);
            return;
        }
        if (isAsync)
        {
            LoadAsync(assetName, listener,isKeepInMemory,type);
        }
    }
    #endregion

    #region 异步Res加载
    private void LoadAsync(string assetName, IResLoadListener listener,bool isKeepInMemory,Type type)
    {
        for (int i = 0; i < mInLoads.Count; i++)
        {
            if (mInLoads[i].assetName == assetName)
            {
                mInLoads[i].AddListener(listener);
                return;
            }
        }

        foreach(RequestInfo info in mWaitting)
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
 #endregion

    #region 资源处理

        public AssetInfo GetAsset(string assetName)
        {
            AssetInfo info = null;
            mDicAaaet.TryGetValue(assetName,out info);
            return info;
        }

        public void ReleaseAsset(string assetName)
        {
            AssetInfo info = null;
            mDicAaaet.TryGetValue(assetName, out info);

            if (info != null && !info.isKeepInMemory)
            {
                mDicAaaet.Remove(assetName);
            }
        }

        public void IsKeepInMemory(string assetName,bool IsKeepInMemory)
        {
            AssetInfo info = null;
            mDicAaaet.TryGetValue(assetName, out info);

            if (info != null)
            {
                info.isKeepInMemory = IsKeepInMemory;
            }
        }
    #endregion

    #region 资源释放以及监听

        public void AddAssetToName(string assetName)
        {
            if (mAssetStack.Count == 0)
            {
                mAssetStack.Push(new List<string>() { assetName });
            }

            List <string> list = mAssetStack.Peek();
            list.Add(assetName);
        }

        public void PushAssetStack()
        {
            List<string> list = new List<string>();
            foreach(KeyValuePair<string,AssetInfo> info in mDicAaaet)
            {
                info.Value.stackCount++;
                list.Add(info.Key);
            }

            mAssetStack.Push(list);
        }

        public void PopAssetStack()
        {
            if (mAssetStack.Count == 0) return;
        
            List<string> list = mAssetStack.Pop();
            List<string> removeList = new List<string>();
            AssetInfo info = null;
            for (int i = 0; i < list.Count;i++ )
            {
                if (mDicAaaet.TryGetValue(list[i],out info))
                {
                    info.stackCount--;
                    if (info.stackCount < 1 && !info.isKeepInMemory)
                    {
                        removeList.Add(list[i]);
                    }
                }
            }
            for (int i = 0; i < removeList.Count;i++ )
            {
                if (mDicAaaet.ContainsKey(removeList[i]))
                mDicAaaet.Remove(removeList[i]);
            }

            GC();
        }

        public void GC()
        {
            Resources.UnloadUnusedAssets();
            System.GC.Collect();
        }


    #endregion

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

                        for (int i = 0; i < info.linsteners.Count;i++ )
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
