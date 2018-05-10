using UnityEngine;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Collections;
using System.Text;

public class ComUtil 
{
    #region Obtiene el objeto secundario del objeto del juego por nombre

    /// <summary>
    ///  Obtiene el objeto secundario del objeto del juego por nombre
    /// </summary>
    /// <param name="tan">objeto</param>
    /// <param name="name">nombre</param>
    /// <returns></returns>
    public static Transform FindTransformInChild(Transform tan, string name, bool oneLevel = false)
    {
        List<Transform> ret = FindTransformInChild(tan, new List<string>(){ name });
        if (ret.Count > 0)
        {
            return ret[0];
        }
        return null;
    }

    /// <summary>
    /// Obtiene el objeto secundario del objeto del juego por nombre
    /// </summary>
    /// <param name="tan"></param>
    /// <param name="names"></param>
    /// <returns></returns>
    public static List<Transform> FindTransformInChild(Transform tan, List<string> names, bool oneLevel = false)
    {
        List<Transform> ret = new List<Transform>();

        if (tan == null)
        {
            Log.Error("ComUtil.FindTransformInChild -> tan Is Null");
            return ret;
        }

        if (tan.childCount != 0)
        {
            for (int i = 0; i < tan.childCount; i++)
            {
                if (oneLevel)
                {
                    if (names.Contains(tan.GetChild(i).name))
                    {
                        ret.Add(tan);
                    }
                }
                else
                {
                    ret.AddRange(FindTransformInChild(tan.GetChild(i), names));
                }
            }
        }

        if (names.Contains(tan.name))
        {
            ret.Add(tan);
        }

        return ret;
    }

    #endregion

    /// <summary>
    /// Encontrar objetos de los objetos
    /// </summary>
    /// <param name="listName">lista de nombre del objeto</param>
    /// <param name="tran">objeto padre</param>
    /// <param name="trans">resultado</param>
    public static void GetTransformInChild(List<string> listName, Transform tran, ref List<Transform> trans)
    {
        if (listName.Contains(tran.name))
        {
            trans.Add(tran);
        }
        if (trans.Count == listName.Count)
        {
            return;
        }
        for (int i = 0; i < tran.childCount; i++)
        {
            GetTransformInChild(listName, tran.GetChild(i), ref trans);
        }
    }
}
