using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public Rigidbody2D rb;
    public float moveSpeed;
    public float jumpSpeed;

    public Transform checkpoint;
    public float checkRadius;
    public LayerMask whatIsGround;
    public bool isGround;

    public Animator Anim;


	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        isGround = Physics2D.OverlapCircle(checkpoint.position, checkRadius, whatIsGround); 


        if (Input.GetAxisRaw("Horizontal")>0)
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            transform.localScale = new Vector2(1f, 1f);

        }else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
            transform.localScale = new Vector2(-1f, 1f);
        }else{
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        if (Input.GetButtonDown("Jump") && isGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }

        Anim.SetFloat("Speed",Mathf.Abs(rb.velocity.x));
        Anim.SetBool("Grounded", isGround);

    }
}
