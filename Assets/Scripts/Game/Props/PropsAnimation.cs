using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class PropsAnimation : MonoBehaviour
{
    public List<GameObject> actionList = new List<GameObject>();

    public Vector2 to;
    public float duration = 3f;

    public void DetectionPlayAnimation()
    {
        for (int i = 0; i < actionList.Count; i++)
        {
            if (!actionList[i].activeSelf)
            {
                return;
            }
        }

        this.gameObject.SetActive(true);

        (transform as RectTransform).DOAnchorPos(to, duration).OnComplete(() => { Destroy(this.gameObject); });
    }
}