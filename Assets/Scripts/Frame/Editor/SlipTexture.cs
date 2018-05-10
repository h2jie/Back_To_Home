using UnityEngine;
using UnityEditor;
using System.IO;

public class SlipTexture : EditorWindow
{
    public Texture2D MainTex = null;

    //Atlas de segmentación
    [MenuItem("Window/Atlas de segmentación")]
    public static void CreateWindows()
    {
        SlipTexture sp = EditorWindow.CreateInstance<SlipTexture>();
        sp.titleContent = new GUIContent("Atlas de segmentación");
        sp.Show(true);
    }

    void OnGUI()
    {
        MainTex = EditorGUILayout.ObjectField("mainText", MainTex, typeof(Texture2D)) as Texture2D;
        if (MainTex != null)
        {

            if (GUILayout.Button("Export imagenes pequeñas"))
            {
                string path = AssetDatabase.GetAssetPath(MainTex);
                TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
                if (importer.textureType != TextureImporterType.Sprite ||
                    importer.spriteImportMode != SpriteImportMode.Multiple ||
                    importer.spritesheet.Length == 0
                    )
                {
                    Debug.LogError("La imagen no esta formato de 'Sprite Multiple'");
                    return;
                }
                importer.isReadable = true;
                AssetDatabase.ImportAsset(path);
                AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);

                string savepath = EditorUtility.OpenFolderPanel("Carpeta para guardar", Application.dataPath, "");
                if (!string.IsNullOrEmpty(savepath))
                {
                    foreach (SpriteMetaData metaData in importer.spritesheet)//pasar los sprite
                    {
                        Texture2D myimage = new Texture2D((int)metaData.rect.width, (int)metaData.rect.height);
                        for (int y = (int)metaData.rect.y; y < metaData.rect.y + metaData.rect.height; y++)
                        {
                            for (int x = (int)metaData.rect.x; x < metaData.rect.x + metaData.rect.width; x++)
                                myimage.SetPixel(x - (int)metaData.rect.x, y - (int)metaData.rect.y, MainTex.GetPixel(x, y));
                        }


                        //Convertir a PNG
                        if (myimage.format != TextureFormat.ARGB32 && myimage.format != TextureFormat.RGB24)
                        {
                            Texture2D newTexture = new Texture2D(myimage.width, myimage.height);
                            newTexture.SetPixels(myimage.GetPixels(0), 0);
                            myimage = newTexture;
                        }
                        byte[] pngData = myimage.EncodeToPNG();


                        //AssetDatabase.CreateAsset(myimage, rootPath + "/" + image.name + "/" + metaData.name + ".PNG");
                        File.WriteAllBytes(savepath + "/" + metaData.name + ".PNG", pngData);
                    }
                }
            }
        }
    }
}
