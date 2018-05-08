using UnityEngine;
using System.Collections.Generic;

public class EventNode : MonoBehaviour
{

    /// <summary>
    /// Todos los mensajes
    /// </summary>
    private Dictionary<int, List<IEventListener>> mListeners = new Dictionary<int, List<IEventListener>>();

    /// <summary>
    /// nodo de la mensaje
    /// </summary>
    private List<EventNode> mNodeList = new List<EventNode>();

    /// <summary>
    /// CN: 挂载一个消息监听器到当前的消息节点
    /// ES: Monte un oyente de mensajes en el nodo de mensaje actual
    /// </summary>
    /// <param name="key">ID mensaje</param>
    /// <param name="listener">listener del mensaje</param>
    /// <returns>CN: 当前消息节点已经挂载了这个消息监听器那么返回false</returns>
    /// <returns>ES: El nodo de mensaje actual ya ha montado este detector de mensaje y devuelve falso</returns>
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

    /// <summary>
    /// Desinstalar un listener de mensaje
    /// </summary>
    /// <returns>Si no está devuelve false</returns>
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

    /// <summary>
    /// CN: 派发消息到子消息节点以及自己节点下的监听器上
    /// ES: Enviar mensajes a nodos de mensajes secundarios y oyentes debajo de sus propios nodos
    /// </summary>
    /// <param name="key">IDmensaje</param>
    /// <param name="param1"></param>
    /// <param name="param2"></param>
    /// <returns>CN: 如果中断消息返回true</returns>
    /// <returns>ES: Si el mensaje de interrupción devuelve </returns>
    private bool DispatchEvent(int key,object param1,object param2)
    {
        for (int i = 0; i < mNodeList.Count;i++ )
        {
            if (mNodeList[i].DispatchEvent(key, param1, param2)) return true;
        }
        return TriggerEvent(key, param1, param2);
    }

  
    /// <summary>
    /// Mensaje disparador
    /// </summary>
    /// <param name="key">idMensaje</param>
    /// <param name="param1"></param>
    /// <param name="param2"></param>
    /// <returns>si está disparado</returns>
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
