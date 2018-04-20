using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tilemap
{
    public int width, height, tileheight, tilewidth, version;
	public List<TilemapLayer> layers;
    public string orientation;
    //public TilemapTileset[] tilesets;
}
