using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour {

    private HeroCtrl player;

    private bool EnemyAwake = false;
    private bool EnemyDead = false;
    //private float AwakeDistance = 5f;
    public float attackDistance;
    public Animator anim;
    public float speed;
    public AudioSource EnemyDiesAudio;
    public GameObject MySpriteOBJ;
    private Vector3 MySpriteOriginalScale;
    public GameObject enemyExplosion;
    public Rigidbody2D enemyRigidbody2d;
        

	// Use this for initialization
	void Start () {
        //player = GameObject.FindGameObjectWithTag("Player").GetComponent<HeroCtrl>();

        //Start the distance checks. (When player gets close enough, Wake up. When he gets far enough, Go back to sleep.
        //InvokeRepeating("CheckPlayerDistance", 0.5f, 0.5f);

        MySpriteOriginalScale = MySpriteOBJ.transform.localScale;
        MySpriteOBJ.transform.localScale = new Vector3(-MySpriteOriginalScale.x, MySpriteOBJ.transform.localScale.y, 1f);


	}


    void FixedUpdate()
    {
        this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        bool attack = false;

        //Log.Debug(HeroCtrl.Instance.heroPosition);
        //If you are awake. Move towards the player
        
        if (Vector3.Distance(this.transform.position, HeroCtrl.Instance.heroPosition) <= attackDistance)
        {
            attack = true;
            
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(this.transform.position, HeroCtrl.Instance.heroPosition, step);

            if (transform.position.x > HeroCtrl.Instance.heroPosition.x)
            {
                MySpriteOBJ.transform.localScale = new Vector3(-MySpriteOriginalScale.x, MySpriteOBJ.transform.localScale.y, 0.6f);
            }

            if (transform.position.x < HeroCtrl.Instance.heroPosition.x)
            {
                MySpriteOBJ.transform.localScale = new Vector3(MySpriteOriginalScale.x, MySpriteOBJ.transform.localScale.y, 0.6f);
            }
            
            
            anim.SetBool("Awake",attack);

        }


        //anim.SetBool("Dead", EnemyDead);
    }
	

    void OnCollisionEnter2D(Collision2D coll)
    {
        Debug.Log(coll.contacts[0].normal.ToString());


        if (coll.gameObject.name=="Hero")
        {
            //Check who killed who. If contact happend from the top player killed the enemy. Else player died.
            if (coll.contacts[0].normal.x > -1f && coll.contacts[0].normal.x < 1f && coll.contacts[0].normal.y < -0.8f && coll.contacts[0].normal.y > -1.8f)
            {
                this.enemyRigidbody2d.velocity = new UnityEngine.Vector2(0, enemyRigidbody2d.velocity.y);

                coll.rigidbody.AddForce(new Vector2(0f, 4900f));
                this.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, -200f));
                /*EnemyDead = true;*/
                SoundManager.Instance.PlaySound("enemyDeadAudio");
                Debug.Log("Monster died");


                
                this.gameObject.SetActive(false);
                Instantiate(enemyExplosion,this.transform.position,this.transform.rotation);

                //Invoke("iDied",1f);
                
                //Destroy(this.gameObject);
                
            }
            else
            {

                Debug.Log("GG");
                EventDef.LevelEvent eventId = EventDef.LevelEvent.PlayerDie;
                AppMgr.Instance.SendEvent((int)eventId,this.gameObject);

            }

        }

    }
    
    void iDied()
    {
        Destroy(this.gameObject);
    }


   /* void CheckPlayerDistance()
    {

        if (Vector3.Distance(this.transform.position, HeroCtrl.Instance.heroPosition) <= AwakeDistance)
        {
            //          Debug.Log("Close enough to wake up");
            EnemyAwake = true;


        }


    }*/


}
