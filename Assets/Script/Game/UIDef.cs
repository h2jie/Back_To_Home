using UnityEngine;
using System.Collections;

public class UIDef
{
    public const string DieUI = "CanvasDie";

    public const string StartUI = "CanvasStart";

    public const string SettingsUI = "CanvasSettings";

    public const string AboutUI = "CanvasAbout";

    public const string SelectLevelUI = "CanvasSelectLevel";


    public static int GetUIOrderLayer(string uianme)
    {
        switch (uianme)
        {
            case UIDef.StartUI:
                return 1;
            case UIDef.SelectLevelUI:
            case UIDef.SettingsUI:
                return 1;
            case UIDef.AboutUI:
                return 2;

        }
        return 0;
    }
}
