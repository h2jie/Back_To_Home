
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventNode : MonoBehaviour
{

    public int EventNodePriority { set; get; }


    private Dictionary<int, List<IEventListener>> mListeners = new Dictionary<int, List<IEventListener>>();

    private List<EventNode> mNodeList = new List<EventNode>();


    public bool AttachEventNode(EventNode node)
    {
        if (node == null)
        {
            return false;
        }

        if (mNodeList.Contains(node))
        {
            return false;
        }
        int pos = 0;
        for (int i = 0; i < mNodeList.Count;i++ )
        {
            if (node.EventNodePriority > mNodeList[i].EventNodePriority)
            {
                break;
            }
            pos++;
        }

        mNodeList.Insert(pos,node);
        return true;
    }


    public bool DetachEventNode(EventNode node)
    {
        if (!mNodeList.Contains(node))
        {
            return false;
        }
        mNodeList.Remove(node);
        return true;
    }


    public bool AttachEventListener(int key,IEventListener listener)
    {
        if (listener == null)
        {
            return false;
        }
        if (!mListeners.ContainsKey(key))
        {
            mListeners.Add(key,new List<IEventListener>() { listener });
            return true;
        }
        if (mListeners[key].Contains(listener))
        {
            return false;
        }
        int pos = 0;
        for (int i = 0;i< mListeners[key].Count;i++ )
        {
            if (listener.EventPriority() > mListeners[key][i].EventPriority())
            {
                break;
            }
            pos++;
        }
        mListeners[key].Insert(pos,listener);
        return true;
    }


    public bool DetachEventListener(int key,IEventListener listener)
    {
       if (mListeners.ContainsKey(key) && mListeners[key].Contains(listener))
       {
           mListeners[key].Remove(listener);
           return true;
       }
       return false;
    }

    public void SendEvent(int key,object param1 = null,object param2 = null)
    {
        DispatchEvent(key, param1, param2);
    }


    private bool DispatchEvent(int key,object param1,object param2)
    {
        for (int i = 0; i < mNodeList.Count;i++ )
        {
            if (mNodeList[i].DispatchEvent(key, param1, param2)) return true;
        }
        return TriggerEvent(key, param1, param2);
    }

  

    private bool TriggerEvent(int key,object param1,object param2)
    {
        if (!this.gameObject.activeSelf || !this.gameObject.activeInHierarchy || !this.enabled)
        {
            return false;
        }

        if (!mListeners.ContainsKey(key))
        {
            return false;
        }
        List<IEventListener> listeners = mListeners[key];
        for (int i = 0; i < listeners.Count; i++)
        {
            if (listeners[i].HandleEvent(key, param1, param2)) return true;
        }
        return false;
    }

   void OnApplicationQuit()
    {
        mListeners.Clear();
        mNodeList.Clear();
    }

}
