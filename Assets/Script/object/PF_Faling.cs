using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PF_Faling : MonoBehaviour {
    public float fallDealy = 0.1f;
    public float respawnDealy = 1f;
    private Rigidbody2D rb2d;
    private PolygonCollider2D pc2d;

    private Vector3 start;
	// Use this for initialization
	void Start () {

        rb2d = GetComponent<Rigidbody2D>();
        pc2d = GetComponent<PolygonCollider2D>();
        start = transform.position;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Invoke("Fall",fallDealy);
            Invoke("Respawn",fallDealy +respawnDealy);
        }
    }

    void Fall(){
        rb2d.isKinematic = false;
        rb2d.velocity = new Vector3(0,-5,0);
        pc2d.isTrigger = true;
    }

    void Respawn(){
        transform.position = start;
        rb2d.isKinematic = true;
        rb2d.velocity = Vector3.zero;
        pc2d.isTrigger = false;
    }
}
