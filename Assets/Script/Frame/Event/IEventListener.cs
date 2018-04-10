using UnityEngine;
using System.Collections;


public interface IEventListener   
{

    bool HandleEvent(int id, object param1, object param2);

    int EventPriority();
}
