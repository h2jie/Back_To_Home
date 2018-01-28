using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UIManager : MonoBehaviour {

    private static UIManager _instance;
    public static UIManager Instance{ get { return _instance; }}

    //Guardar el stack de los panel que tenemos
    private Stack<UIBase> UIStack = new Stack<UIBase>();

    //Guardar el panel actual
    private Dictionary<string, UIBase> currentUIDict = new Dictionary<string, UIBase>();
    //prefab de interface
    private Dictionary<string, GameObject> UIObjectDict = new Dictionary<string, GameObject>();

    public string ResourceDir = "UI";

    void Awake()
    {
        _instance = this;
        LoadALLUIObject();
    }

    //Entra stack y muestra panel
    public void PushUIPanel(string UIName){
        if (UIStack.Count>0)
        {
            UIBase old_topUI = UIStack.Peek();
            old_topUI.DoOnPausing();
        }

        UIBase new_topUI = GetUIBase(UIName);
        new_topUI.DoOnEntering();
        UIStack.Push(new_topUI);
    }


    private UIBase GetUIBase(string UIName){
        foreach (var name in currentUIDict.Keys)
        {
            if (name==UIName)
            {
                UIBase u = currentUIDict[UIName];
                return u;
            }
        }

        //Si no coger la panel en prefab
        GameObject UIPrefab = UIObjectDict[UIName];
        GameObject UIObject = GameObject.Instantiate<GameObject>(UIPrefab);
        UIObject.name = UIName;
        //Crear panel
        UIBase uibase = UIObject.GetComponent<UIBase>();
        currentUIDict.Add(UIName, uibase);
        return uibase;
    }


    //Salir de stack, ocultar panel
    public void PopUIPanel(){
        if (UIStack.Count ==0)
        {
            return;
        }

        UIBase old_topUI = UIStack.Pop();
        old_topUI.DoOnExiting(); 

        if (UIStack.Count > 0)
        {
            UIBase new_topUI = UIStack.Peek();
            new_topUI.DoOnResuming();
        }
    }

    private void LoadALLUIObject(){
        string path = Application.dataPath + "Assets/Resources/" + ResourceDir;
        DirectoryInfo folder = new DirectoryInfo(path);

        foreach (FileInfo file in folder.GetFiles("*.prefab"))
        {
            int index = file.Name.LastIndexOf('.');
            string UIName = file.Name.Substring(0, index);
            string UIPath = ResourceDir + "/" + UIName;
            GameObject UIObject = Resources.Load<GameObject>(UIPath);
            UIObjectDict.Add(UIName, UIObject);
        }

    }

}
