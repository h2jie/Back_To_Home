using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_1 : MonoBehaviour {
    public Sprite whenItHits;
    public SpriteRenderer mySprite;
    public bool isHits;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag=="Player")
        {
            mySprite.sprite = whenItHits;
            isHits = true;
        }
    }
}
