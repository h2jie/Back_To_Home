using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TilemapLayer {

	public string name, type;
	public List<int> data;
	public int width, height, x, y;
	public bool visible;
	public float opacity;

	public TilemapLayer(){
	}
}
