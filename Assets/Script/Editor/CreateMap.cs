using UnityEngine;
using System.Collections;
using UnityEditor;
using LitJson;
using UnityEngine.UI;
using System.Collections.Generic;
public class CreateMap : EditorWindow 
{
	[MenuItem("Window/创建地图")]
    public static void CreateMapWindow()
    {
        CreateMap Window = EditorWindow.GetWindow<CreateMap>("创建地图信息");

        if (Window != null)
        {
            Window.Show(true);
        }
    }

    private Texture2D mainTex;
    private TextAsset mapJson;
    private string mapStr;
    private GameObject CrateParent;
    void OnGUI()
    {
        mainTex = EditorGUILayout.ObjectField("地图图集信息", mainTex, typeof(Texture2D)) as Texture2D;
        mapJson = EditorGUILayout.ObjectField("地图配置文件", mapJson, typeof(TextAsset)) as TextAsset;
        CrateParent = EditorGUILayout.ObjectField("地图创建的父对象", CrateParent, typeof(GameObject)) as GameObject;
        mapStr = EditorGUILayout.TextField("地图名称开头",mapStr);
        if (mainTex != null && mapJson != null && CrateParent && !string.IsNullOrEmpty(mapStr))
        {
            if (GUILayout.Button("生成地图"))
            {
                CreateMapObj();
            }
        }
    }
	
    public void CreateMapObj()
    {
        List<Sprite>sprites = new List<Sprite>();
        Object[] objs = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(mainTex));
        foreach(Object obj in objs)
        {
            Sprite sp = obj as Sprite;
            if (sp != null)
            {
                sprites.Add(sp);
            }
        }

        Tilemap map = JsonMapper.ToObject<Tilemap>(mapJson.text);
        int sh = map.tileheight;
        int sw = map.tilewidth;//地图块大小

        int ah = map.height;//地图总大小
        int aw = map.width;

        foreach(TilemapLayer layer in map.layers)
        {
            
            if (layer.type == "tilelayer" && layer.name == "Platform")
            {
                int x = 0,y = 0;
                foreach (int id in layer.data)
                {
                    if (id != 0)
                    {
                        int iamgeId = id - 1;
                        GameObject go = new GameObject(iamgeId.ToString(),typeof(RectTransform));

                        RectTransform rectTran = go.transform as RectTransform;
                        rectTran.SetParent(CrateParent.transform, false);
                        rectTran.pivot = Vector2.zero;
                        rectTran.sizeDelta = new UnityEngine.Vector2(sw,sh);
                        
                        Image image = go.AddComponent<Image>();
                        image.sprite = sprites.Find(s =>
                        {
                            return s.name == mapStr + iamgeId.ToString();
                        });
                        rectTran.localPosition = new UnityEngine.Vector3(x *sw,(ah - y - 1) *sh);

                    }
                    if (++ x >= aw)
                    {
                        x = 0;
                        y++;
                    }
                }
            }
        }
    }
}
