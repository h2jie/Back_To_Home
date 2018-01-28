using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBehaviour {

    public virtual string Name { get; set; }

    public virtual void DoOnEntering(){
        GetComponent<Canvas>().worldCamera = Camera.main;
    }

    public virtual void DoOnPausing(){
        
    }
     
    public virtual void DoOnResuming(){
        
    }

    public virtual void DoOnExiting(){
        
    }
}
