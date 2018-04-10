using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class AppMgr : EventNode
{
    private static AppMgr mInstance;

    public static AppMgr Instance
    {
        get
        {
            return mInstance;
        }
    }
    private Camera mMainCamera;
    public Camera MainCamera
    {
        get
        {
            if (mMainCamera == null)
            {
                mMainCamera = Camera.current;
            }
            return mMainCamera;
        }
    }

    private Vector2 mSceneSize = new Vector2(960, 640);

    public Vector2 SceneSize
    {
        get
        {
            return mSceneSize;
        }
    }

    void Awake()
    {
        mInstance = this;
    }

    private const string mSoundValueKey = "SoundValueKEY";
    public bool SoundValue
    {
        get
        {
            return PlayerPrefs.GetInt(mSoundValueKey, 0) == 0;
        }
        set
        {
            PlayerPrefs.SetInt(mSoundValueKey, value ? 0 : 1);
            PlayerPrefs.Save();
        }
    }

    private const string mMusicValueKey = "MusicValueKEY";
    public bool MusicValue
    {
        get
        {
            return PlayerPrefs.GetInt(mMusicValueKey, 0) == 0;
        }
        set
        {
            PlayerPrefs.SetInt(mMusicValueKey, value ? 0 : 1);
            PlayerPrefs.Save();
        }
    }


    #region GuardarModuloDeDatos

    private const string mOpenLevelsKey = "OpenLevelsKEY";

    /// <summary>
    /// Los Niveles que ya esta abierto
    /// </summary>
    public List<int> OpenLevels
    {
        get
        {
            List<int> list = new List<int>() { };
            list.Add(1);
            string s = PlayerPrefs.GetString(mOpenLevelsKey, "");
            if (s.Contains("-"))
            {
                string[] ss = s.Split('-');
                for (int i = 0; i < ss.Length; i++)
                {
                    int num = 0;
                    int.TryParse(ss[i], out num);
                    if (num != 0 && !list.Contains(num))
                    {
                        list.Add(num);
                    }
                }
            }
            return list;
        }
        set
        {
            StringBuilder sb = new StringBuilder();
            List<int> list = value;
            for (int i = 0; i < list.Count; i++)
            {
                sb.Append(list[i]);
                if (i < list.Count - 1)
                {
                    sb.Append("-");
                }
            }
            PlayerPrefs.SetString(mOpenLevelsKey, sb.ToString());
            PlayerPrefs.Save();
        }
    }
    #endregion
}
