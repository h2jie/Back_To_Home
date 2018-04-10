using UnityEngine;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Collections;
using System.Text;

public class ComUtil 
{
    #region CrearNodo

    public static T CreateOrGetComponent<T>(GameObject target) where T : Component
    {
        if (null == target)
        {
            return null;
        }

        T comp = target.GetComponent<T>();
        if (null == comp)
        {
            comp = target.AddComponent<T>();
        }

        return comp;
    }

 

    public static void AddGameObjectToZeroPos(Transform transChild, Transform transParent)
    {
        if (null == transChild || null == transParent)
        {
            return;
        }

        transChild.parent = transParent;
        transChild.localPosition = Vector3.zero;
        transChild.localRotation = Quaternion.identity;
        transChild.localScale = Vector3.one;
    }


    public static void AddGameObjectTo(Transform transChild, Transform transParent)
    {
        if (null == transChild || null == transParent || transChild.parent == transParent)
        {
            return;
        }

        transChild.parent = transParent;
        Transform trans = transParent;
        Vector3 scaleParam = new Vector3();
        while (null != trans)
        {
            scaleParam = trans.localScale;
            scaleParam.Scale(transChild.localPosition);
            transChild.localPosition = trans.localPosition + scaleParam;
            transChild.localRotation = trans.localRotation * transChild.localRotation;

            scaleParam = transChild.localScale;
            scaleParam.Scale(trans.localScale);
            transChild.localScale = scaleParam;

            trans = trans.parent;
        }
    }

#endregion

#region GetUTF8StringEX

    private static StringBuilder strBuilder = new StringBuilder(4048);


    public static string GetUTF8StringTrimZero(byte[] buffer)
	{
        if (null == buffer || 0 == buffer.Length)
        {
            return "";
        }
        strBuilder.Remove(0, strBuilder.Length);
        for (int i = 0; i < buffer.Length; i++)
        {
            if (buffer[i] != 0)
            {
                strBuilder.Append((char)buffer[i]);
            }
            else
            {
                break;
            }
        }
        return strBuilder.ToString();
	}
	
#endregion



    public static string GetVerticalString(string val)
    {
        string ret = null;
        if (null != val)
        {
            string n = "" + '\n';
            ret = val.Replace("[/n]", n);
        }
        return ret;
    }



    #region BuscarNodo

    public static bool IsSubClassOfRawGeneric(Type generic, Type toCheck)
    {
        while (toCheck != null && toCheck != typeof(object))
        {
            Type cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
            if (generic == cur)
            {
                return true;
            }
            toCheck = toCheck.BaseType;
        }
        return false;
    }

    public static bool IsSubClassOf(Type baseType, Type toCheck)
    {
        return baseType.IsAssignableFrom(toCheck);
    }


    public static List<T> FindAllComponentsUnderRoot<T>(Transform root) where T : Component
    {
        List<T> retList = new List<T>();

        if (root.childCount != 0)
        {
            for (int i = 0; i < root.childCount; i++)
            {
                retList.AddRange(FindAllComponentsUnderRoot<T>(root.GetChild(i)));
            }
        }

        T t = root.GetComponent<T>();
        if (t != null)
        {
            retList.Add(t);
        }
        return retList;

    }

    #endregion

    #region OrdenarNodo


    public static List<Transform> SortRoutePointExcludeTag(GameObject root, string tag)
    {
		return SortRoutePoint(root, tag, true);
	}
	

	public static List<Transform> SortRoutePointWithTag(GameObject root, string tag) 
	{
		return SortRoutePoint(root, tag, false);
	}
	
	private static List<Transform> SortRoutePoint(GameObject root, string tag, bool isExclude)
	{
		ArrayList routePointNames = new ArrayList();
		for (int i = 0; i < root.transform.childCount; i++)
		{
			Transform trans = root.transform.GetChild(i);
			bool isAdd = isExclude ? trans.tag != tag : trans.tag == tag;
			if (isAdd)
			{
				routePointNames.Add(trans.name);
			}
		}
		routePointNames.Sort();
		
		List<Transform> ret = new List<Transform>();
		foreach (string na in routePointNames)
		{
			for (int i = 0; i < root.transform.childCount; i++)
			{
				Transform trans = root.transform.GetChild(i);
				if (trans.name == na)
				{
					ret.Add(trans);
					break;
				}
			}
		}
		return ret;
	}
	
	#endregion

    #region Obtenga_componentes_especifica_la_ruta
    public static T GetComponentByLocalPath<T>(GameObject go, string localPath) where T : Component
    {
        GameObject obj = null;
        if (go == null)
        {
            obj = GameObject.Find(localPath) as GameObject;
        }
        else if (go.transform.childCount > 0)
        {
            Transform tran = go.transform.Find(localPath);
            if (tran != null)
            {
                obj = tran.gameObject;
            }
        }
        if (obj == null)
        {
            Debug.LogWarning(string.Format("GetComponentByLocalPath Null={0}", localPath));
            return null;
        }
        else
        {
            return obj.GetComponent<T>();
        }
    }
    #endregion

    #region Obtenga_componentes_especifica_objeto

    public static GameObject FindGameObjectByLocalPath(GameObject go, string localPath)
    {
        GameObject obj = null;
        if (go == null)
        {
            obj = GameObject.Find(localPath) as GameObject;
        }
        else if (go.transform.childCount > 0)
        {
            Transform tran = go.transform.Find(localPath);
            if (tran != null)
            {
                obj = tran.gameObject;
            }
        }
        if (obj == null)
        {
            Debug.LogWarning(string.Format("GetComponentByLocalPath Null={0}", localPath));
            return null;
        }
        else
        {
            return obj;
        }
    }
    #endregion

    #region randomNUmber

    public static float GetRandIn0_1(int seed = 3)
    {

       return UnityEngine.Random.value;
    }
    #endregion

    #region Obtiene_el_objeto_secundario_del_objeto_del_juego_por_nombre

    public static Transform FindTransformInChild(Transform tan, string name, bool oneLevel = false)
    {
        List<Transform> ret = FindTransformInChild(tan, new List<string>(){ name });
        if (ret.Count > 0)
        {
            return ret[0];
        }
        return null;
    }


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

    #region Obtiene_el_objeto_secundario_del_objeto_del_juego_por_nombre

    public static T FindComponentInChild<T>(Transform tan, string name) where T : Component
    {
        List<Transform> ret = FindTransformInChild(tan, new List<string>() { name });
        if (ret.Count > 0)
        {
            return ret[0].GetComponent<T>();
        }
        return null;
    }
    #endregion


    public static int GetNumberLen(int number)
    {
        int tmp = number;
        int ret = 0;
        while (tmp > 0)
        {
            ret += 1;
            tmp /= 10;
        }
        return ret;
    }

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
