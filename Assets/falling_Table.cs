using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class falling_Table : MonoBehaviour {
    public float fallDealy = 0.1f;
    private Rigidbody2D rb2d;
    private BoxCollider2D bc2d;
    public bool exite = false;

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        bc2d = GetComponent<BoxCollider2D>();
        if (exite)
        {
            Invoke("Fall", fallDealy);
        }
	}
	
	// Update is called once per frame
	void Update () {
        
		
	}


    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

    void Fall()
    {
        rb2d.isKinematic = false;
        rb2d.velocity = new Vector3(0, -5, 0);
    }
}
