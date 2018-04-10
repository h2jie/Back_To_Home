using UnityEngine;
using System.Collections;


public interface IResLoadListener  
{
    void Finish(object asset);

    void Failure();
}
