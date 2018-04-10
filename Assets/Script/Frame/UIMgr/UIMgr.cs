using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class UIMgr : EventNode 
{
    private static UIMgr mInstance;
    public static UIMgr Instance
    {
        get
        {
            return mInstance;
        }
    }

    private Dictionary<string, BaseUI> mDicUI = new Dictionary<string, BaseUI>();


    public void AddUI(BaseUI ui)
    {
        if (ui != null)
        {
            mDicUI[ui.UIName] = ui;
            
        }
        
    }


    public void RemoveUI(BaseUI ui)
    {
        if (ui != null && mDicUI.ContainsKey(ui.UIName))
        {
            mDicUI.Remove(ui.UIName);
        }
    }


    public List<Command> cmdList = new List<Command>();

    internal Transform UIROOT = null;
	void Awake() 
	{
        UIROOT = this.transform.Find("UIRoot");
        mInstance = this;
        DontDestroyOnLoad(this.gameObject);
	}

    public List<string> GetCurrentOpenUI()
    {
        List<string> list = new List<string>();
       foreach(BaseUI ui in mDicUI.Values)
       {
           if (ui.CacheGameObject.activeInHierarchy)
           {
               list.Add(ui.UIName);
           }
       }
       return list;
    }

    #region CrearUI


    public void CreateUI(string uiName, Type type, ILoadUIListener listener)
    {
        cmdList.Add(Command.CreateCmd(type,uiName,listener));
    }


    private void _Create(Command cmd)
    {
       
        BaseUI ui= null;
        mDicUI.TryGetValue(cmd.uiName, out ui);
        if (ui != null)
        {
            if (cmd.listener != null) cmd.listener.FiniSh(ui);
        }
        else
        {

            ResMgr.Instance.Load(cmd.uiName,new LoadResFinish(cmd),typeof(GameObject),true);
        }
    }

    #endregion

    #region MostrarUI


    public void ShowUI(string uiName, Type type, ILoadUIListener listener,object param = null,bool createCanCall = false)
    {
        BaseUI ui = null;
        mDicUI.TryGetValue(uiName,out ui);
        if (ui == null)
        {
            cmdList.Add(Command.CreateAndShowCmd(uiName,type,listener, param, createCanCall));
        }
        else
        {
            cmdList.Add(Command.ShowCmd(uiName, listener, param, createCanCall));
        }
        
        
    }


    private void _ShowUI(Command cmd)
    {

        BaseUI ui = null;
        mDicUI.TryGetValue(cmd.uiName, out ui);
        if (ui != null)
        {
            if (cmd.listener != null)
            {
                cmd.listener.FiniSh(ui);
            }
            ui.Show(cmd.param);
            
        }
    }

   
    #endregion

    #region  OcultarUI


    public void HideUI(string uiName)
    {
        
        cmdList.Add(Command.HideCmd(uiName));
    }


    private void _HideUI(Command cmd)
    {
        BaseUI ui = null;
        mDicUI.TryGetValue(cmd.uiName, out ui);
        if (ui != null)
        {
            ui.Hide();
        }
    }
#endregion

    #region  EliminarUI

    public void DestroyUI(string uiName)
    {
        cmdList.Add(Command.DestroyCmd(uiName));
    }

    private void _DestroyUI(Command cmd)
    {
        BaseUI ui = null;
        mDicUI.TryGetValue(cmd.uiName, out ui);
        if (ui != null)
        {
            mDicUI.Remove(ui.UIName);
            Destroy(ui.CacheGameObject);
        }
    }

    #endregion

	// Update is called every frame, if the MonoBehaviour is enabled.
	void Update() 
	{

        if (cmdList.Count > 0)
        {
            Command tempCmd = null;
            tempCmd = cmdList[0];
            if (tempCmd == null)
            {
                cmdList.RemoveAt(0);
            }
            else
            {
                switch(tempCmd.cmdType)
                {
                    case Command.CmdType.CreateAndShow:
                        _Create(tempCmd);
                        break;
                    case Command.CmdType.Create:
                        _Create(tempCmd);
                        break;
                    case Command.CmdType.Destroy:
                        _DestroyUI(tempCmd);
                        break;
                    case Command.CmdType.Hide:
                        
                        _HideUI(tempCmd);
                        break;
                    case Command.CmdType.Show:
                        _ShowUI(tempCmd);
                        break;
                }
                cmdList.RemoveAt(0);
            }
        }
	}



    public class LoadResFinish : IResLoadListener
    {

        public Command cmd;
        public LoadResFinish(Command _cmd)
        {
            cmd = _cmd;
        }

        public void Finish(object asset)
        {

            if (cmd == null)
            {
                return;
            }
            GameObject go = Instantiate<GameObject>(asset as GameObject);
            go.SetActive(false);
            BaseUI ui =  go.AddComponent(cmd.type) as BaseUI;
            ui.UIName = cmd.uiName;
            go.gameObject.name = ui.UIName;
            ui.CacheTransform.SetParent(UIMgr.Instance.UIROOT, false);
            UIMgr.Instance.AddUI(ui);
            if (cmd.cmdType == Command.CmdType.CreateAndShow)
            {
                UIMgr.Instance.ShowUI(cmd.uiName, cmd.type, cmd.listener, cmd.param, cmd.createCanCall);
            }
            else if (cmd.createCanCall && cmd.listener != null)
            {
                cmd.listener.FiniSh(ui);
            }

            ui.UIInit();
        }

        public void Failure()
        {
            if (cmd.createCanCall && cmd.listener != null)
            {
                cmd.listener.Failure();
            }
        }
    }


    public interface ILoadUIListener
    {
        void FiniSh(BaseUI ui);
        void Failure();
    }


    public class Command
    {
   
        public enum CmdType
        {

            CreateAndShow,Create,Show,Hide,Destroy,
        }


        public string uiName;

        public Type type;

        public ILoadUIListener listener;

        public object param;

        public CmdType cmdType;

        public bool createCanCall = true;

        public static Command CreateAndShowCmd(string uiName, Type type, ILoadUIListener listener, object param , bool createCanCall)
        {
            Command cmd = new Command(CmdType.CreateAndShow, uiName, type);
            cmd.createCanCall = createCanCall;
            cmd.listener = listener;
            cmd.type = type;
            cmd.param = param;
            return cmd;
        }

        public static Command ShowCmd(string _uiName,ILoadUIListener listener ,object _param, bool _createCanCall)
        {
            Command cmd = new Command(CmdType.Show, _uiName, _param);
            cmd.createCanCall = _createCanCall;
            cmd.listener = listener;
            return cmd;
        }

        public static Command CreateCmd(Type _type,string _uiName, ILoadUIListener _listener)
        {
            return new Command(CmdType.Create, _uiName, _type, _listener);
        }

        public static Command HideCmd(string _uiName)
        {
            return new Command(CmdType.Hide, _uiName,null);
        }

        public static Command DestroyCmd(string _uiName)
        {
            return new Command(CmdType.Destroy, _uiName, null);
        }

        public Command(CmdType _cmdType, string _uiName, object _param)
        {
            uiName = _uiName;
            cmdType = _cmdType;
            param = _param;
        }

        public Command(CmdType _cmdType,string _uiName,Type _type,ILoadUIListener _listener)
        {
            cmdType = _cmdType;
            type = _type;
            listener = _listener;
            uiName = _uiName;
        }
    }
}
