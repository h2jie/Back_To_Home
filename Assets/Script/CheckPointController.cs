using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointController : MonoBehaviour {
    public Sprite CheckOpen;
    public Sprite CheckClose;
    public SpriteRenderer mySprite;
    public bool IsCheckPoint;
    public Enemy enemy;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag=="Player")
        {
            mySprite.sprite = CheckOpen;
            IsCheckPoint = true;

        }
    }
}
