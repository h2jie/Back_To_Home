using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temptation : MonoBehaviour {

    public AudioSource starAudio;
	// Use this for initialization
	void Start () {
        		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag =="Player")
        {
            if (starAudio !=null)
            {
                starAudio.Play();
            }
            Destroy(this.gameObject);
        }
    }
}
