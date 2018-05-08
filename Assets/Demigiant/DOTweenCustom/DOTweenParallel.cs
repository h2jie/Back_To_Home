#region Header
/**
 * DOTweenParallel.cs
 * 星战
 *
 *   @author  lgn 
 *   @Email   linguinan@126.com
 *   Created on 5/15/2015.
 **/
#endregion

using UnityEngine;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;

/// <summary>
/// 注释：在DOTween中未找到可以很好的实现多个Tween同时播放和倒播的方法
/// 特此自建一个类处理这种功能需求
/// </summary>
public class DOTweenParallel {

    private List<Tween> tweenList = new List<Tween>();

	public void Append (Tween t) {
		tweenList.Add(t);
	}

	public void PlayForward () {
		for (int i = 0; i < tweenList.Count; i++) {
			Tween tween = tweenList[i];
			tween.PlayForward();
		}
	}
	
	public void PlayBackwards () {
        for (int i = 0; i < tweenList.Count; i++)
        {
			tweenList[i].PlayBackwards();
		}
	}

	public void Restart () {
        for (int i = 0; i < tweenList.Count; i++)
        {
			tweenList[i].Restart();
		}
	}

	public void Dispose () {
		tweenList.Clear();
		tweenList = null;
	}


    
}
