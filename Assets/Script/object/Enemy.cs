using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour {

    private PlayerController player;

    private bool EnemyAwake = false;
    private bool EnemyDead = false;
    private float AwakeDistance = 5f;
    public float attackDistance;
    public Animator anim;
    public float speed;
    public LevelManager level;
    public AudioSource EnemyDiesAudio;
    public GameObject MySpriteOBJ;
    private Vector3 MySpriteOriginalScale;
    public GameObject enemyExplosion;
    public UnityEvent onCollision;
  

    public Vector3 originPosition;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        //Start the distance checks. (When player gets close enough, Wake up. When he gets far enough, Go back to sleep.
        InvokeRepeating("CheckPlayerDistance", 0.5f, 0.5f);

        MySpriteOriginalScale = MySpriteOBJ.transform.localScale;
        MySpriteOBJ.transform.localScale = new Vector3(-MySpriteOriginalScale.x, MySpriteOBJ.transform.localScale.y, 1f);


	}


    void FixedUpdate()
    {
        this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        //If you are awake. Move towards the player
        if (EnemyAwake == true && EnemyDead == false)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(this.transform.position, player.transform.position, step);

            if (MySpriteOBJ.transform.localScale.x > 0 && transform.position.x > player.transform.position.x)
            {
                MySpriteOBJ.transform.localScale = new Vector3(-MySpriteOriginalScale.x, MySpriteOBJ.transform.localScale.y, 1f);
            }

            if (MySpriteOBJ.transform.localScale.x < 0 && transform.position.x < player.transform.position.x)
            {
                MySpriteOBJ.transform.localScale = new Vector3(MySpriteOriginalScale.x, MySpriteOBJ.transform.localScale.y, 1f);
            }
        }


        anim.SetBool("Awake", EnemyAwake);
        anim.SetBool("Dead", EnemyDead);
    }
	
	// Update is called once per frame
	/*void Update () {
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
    }*/




    void OnCollisionEnter2D(Collision2D coll)
    {
        Debug.Log(coll.contacts[0].normal.ToString());

        if (coll.gameObject.tag == "Player")
        {

            //Check who killed who. If contact happend from the top player killed the enemy. Else player died.
            if (coll.contacts[0].normal.x > -1f && coll.contacts[0].normal.x < 1f && coll.contacts[0].normal.y < -0.8f && coll.contacts[0].normal.y > -1.8f)
            {
                if (EnemyDiesAudio != null)
                {
                    EnemyDiesAudio.Play();
                }
                coll.rigidbody.AddForce(new Vector2(0f, 1500f));
                this.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, -200f));
                EnemyDead = true;
                Debug.Log("Monster died");
                Instantiate(enemyExplosion,this.transform.position,this.transform.rotation);
                Invoke("iDied", 0.15f);
                onCollision.Invoke();
            }
            else
            {
                level.Respawn();
            }

        }
    }


    void iDied()
    {
        Destroy(this.gameObject);
    }


    void CheckPlayerDistance()
    {

        if (Vector3.Distance(this.transform.position, player.transform.position) <= AwakeDistance && EnemyAwake == false)
        {
            //          Debug.Log("Close enough to wake up");
            EnemyAwake = true;


        }

        if (Vector3.Distance(this.transform.position, player.transform.position) > AwakeDistance && EnemyAwake == true)
        {
            //          Debug.Log("Far enough to fall back sleep");
            EnemyAwake = false;


        }

    }


}
