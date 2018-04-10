using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;
public class PropsAnimation : MonoBehaviour 
{
    public List<GameObject> actionList = new List<GameObject>();
    
    public Vector2 to;
    public float duration = 3f;

    public void DetectionPlayAnimation()
    {
        for(int i = 0;i<actionList.Count;i++)
        {
            if (!actionList[i].activeSelf)
            {
                return;
            }
        }
        this.gameObject.SetActive(true);

        //(transform as RectTransform).anchoredPosition(to, duration).OnComplete(() => { Destroy(this.gameObject); });
    }
	
}
