using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class EventDef
{

    public const int ResLoadFinish = 1;


    [Serializable]
    public enum LevelEvent : int
    {
        None = 2000,
        PlayerDie,
        SaveGame,
        GameOver,
    }




}
