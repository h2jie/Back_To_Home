using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AssetInfo
{

    public object asset;


    public bool isKeepInMemory;

    public int stackCount = 0;
}


public class RequestInfo
{

    public ResourceRequest request;


    public bool isKeepInMemory;

    public List<IResLoadListener> linsteners;

    public void AddListener(IResLoadListener listener)
    {
        if (linsteners == null)
        {
            linsteners = new List<IResLoadListener>() { listener };
        }
        else
        {
            if (!linsteners.Contains(listener))
            {
                linsteners.Add(listener);
            }
        }
    }

    public string assetName;

    public string assetFullName
    {
        get
        {
            return ResMgr.Instance.GetFileFullName(assetName);
        }
    }

    public Type type;


    public bool IsDone
    {
        get
        {
            return (request != null && request.isDone);
        }
    }

    public object Asset
    {
        get
        {
            return request != null ? request.asset : null;
        }
    }

    public void LoadAsync()
    {

        request = type == null ? Resources.LoadAsync(assetFullName) : Resources.LoadAsync(assetFullName,type);
    }
}
