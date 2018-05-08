using UnityEngine;
using System.Collections.Generic;

public class TableDataMgr : MonoBehaviour , UIMgr.ILoadUIListener
{
    public static TableDataMgr Instance
    {
        private set;
        get;
    }
    private List<TableDateBase> mLoadTables = new List<TableDateBase>();

    

	void Awake()
	{
        Instance = this;

        UIMgr.Instance.ShowUI(UIDef.StartUI, typeof(StartCtrl), this);
        return;
        for (int i = 0; i < mLoadTables.Count;i++ )
        {
            ResMgr.Instance.Load(mLoadTables[i].tableName,mLoadTables[i]);
        }

	}
	
	void Start () 
	{
	
	}
	
	void Update () 
	{
	
	}
	
    #region Lea la clase base de la tabla de configuración

    public class TableDateBase : IResLoadListener
    {
        public string tableName;

        public TableDateBase(string tableName)
        {
            this.tableName = tableName;
            Instance.mLoadTables.Add(this);
        }

        public void Finish(object asset)
        {
            Log.Debug("tableName" + tableName + "/Instance.mLoadTables =" + Instance.mLoadTables.Count);
            TextAsset text = asset as TextAsset;
            if (text == null)
            {
                Log.Error("Fallo de leer la clase base, tableName：" + tableName);
            }
            else
            {
                ExtractJson(text.text);
            }

            Instance.mLoadTables.Remove(this);

            if (Instance.mLoadTables.Count == 0)
            {
                //EntranceSceneCtrl.Instance.SendEvent(EventDef.TableDataFinish);
            }
        }

        public void Failure()
        {
        }

        protected virtual void ExtractJson(string json)
        {

        }
    }
#endregion


    public void FiniSh(BaseUI ui)
    {
    }

    public void Failure()
    {
    }
}
