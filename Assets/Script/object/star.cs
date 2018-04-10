using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class star : MonoBehaviour {

    public UnityEvent onCollision;
    public AudioSource music;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        onCollision.Invoke();

        if (collision.gameObject.tag == "Player")
        {
            if (music != null)
            {
                music.Play();
            }
          
        }


    }


}
