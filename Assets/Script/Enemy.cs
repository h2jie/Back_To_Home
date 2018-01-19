using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Enemy : MonoBehaviour {

    private Transform player;
    public float attackDistance;
    private Animator anim;
    public float speed;
    public LevelManager level;
    public AudioSource EnemyDiesAudio;
  

    public Vector3 originPosition;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = this.GetComponent<Animator>();
        level = FindObjectOfType<LevelManager>();
        originPosition = transform.position;


	}
	
	// Update is called once per frame
	void Update () {
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance<attackDistance)
        {
            anim.SetBool("Run",true);

            if (player.position.x<transform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }else{
                transform.localScale = new Vector3(1, 1, 1);

            }

            Vector3 dir = player.position - transform.position;
            transform.position = dir.normalized * speed * Time.deltaTime + transform.position;
        }else{
            anim.SetBool("Run",false);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            if ( collision.contacts[0].normal.y < -0.1f)
            {
                if (EnemyDiesAudio != null)
                {
                    EnemyDiesAudio.Play();
                }

                collision.rigidbody.AddForce(new Vector2(0f, 500f));
                this.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -150f));
                anim.SetBool("Dead", true);

                Destroy(this.gameObject);

            }
            else
            {
                level.Respawn();
                transform.position = originPosition;

            }
        }
    }




}
