public class UIDef
{

    public const string DieUI = "CanvasDie";

    public const string WinUI = "CanvasWin";

    public const string PauseUI = "CanvasPause";

    public const string AboutUI = "CanvasAbout";

    public const string StartUI = "CanvasStart";

    public const string SettingsUI = "CanvasSettings";

    public const string SelectLevelUI = "CanvasSelectLevel";

    /// <summary>
    /// Orden para get pantalla
    /// </summary>
    /// <param name="uianme"></param>
    /// <returns></returns>
    public static int GetUIOrderLayer(string uianme)
    {
        switch (uianme)
        {
            case UIDef.StartUI:
                return 1;

            case UIDef.SettingsUI:
            case UIDef.SelectLevelUI:
            case UIDef.AboutUI:
            case UIDef.DieUI:
            case UIDef.WinUI:
                return 2;
        }
        return 0;
    }

    /// <summary>
    /// get numero del nivel
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public static string GetLevelName(int level)
    {
        return string.Format("Level_{0}", level);
    }
}
