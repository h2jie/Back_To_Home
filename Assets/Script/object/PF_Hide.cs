using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PF_Hide : MonoBehaviour {

    public UnityEvent onCollision;


    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.contacts[0].normal.y > -1f)
        {
            onCollision.Invoke();

        }
    }
}
