using UnityEngine;
using System.Collections;
using System;

public class HeroCtrl : MonoBehaviour ,IEventListener
{
    [Header("Rigidbody del personaje")]
    public Rigidbody2D playerRigidbody2D;

    [Header("Controlador de animación de personaje")]
    public Animator playerAnimator;

    [Header("Velocidad de movimiento")]
    public float xSpeed = 40.0f;

    [Header("Fuerza de salta")]
    public float ySpeed = 8000;


    private bool isGround = true;


    private bool isJump = false;


    private bool isLeft = false;


    private bool isDie = false;

    [Header("Verifica si el personaje está en el suelo")]
    public Transform checkGroudPos;

    [Header("Capa de pisada de personaje")]
    public LayerMask GroudMask;

    [Header("Determine el área circular del piso Radio del círculo")]
    public float radius = 4.5f;
    
	void Awake()
	{
        if(AppMgr.Instance)
        {
            AppMgr.Instance.AttachEventListener((int)EventDef.LevelEvent.PlayerDie, this);
            AppMgr.Instance.AttachEventListener((int)EventDef.LevelEvent.GameOver, this);
            AppMgr.Instance.AttachEventListener((int)EventDef.LevelEvent.SaveGame, this);
        }
	}
	
    void OnDestroy()
    {
        if (AppMgr.Instance)
        {
            AppMgr.Instance.DetachEventListener((int)EventDef.LevelEvent.PlayerDie, this);
            AppMgr.Instance.DetachEventListener((int)EventDef.LevelEvent.GameOver, this);
            AppMgr.Instance.DetachEventListener((int)EventDef.LevelEvent.SaveGame, this);
        }
    }

	void Start () 
	{
	
	}
	
	void Update () 
	{
        if (isDie)
	    {
            return;
	    }


        if(Input.GetButtonDown("Jump"))
        {
            if (isGround && !isJump)
            {
                isJump = true;
                isGround = false;
                playerRigidbody2D.AddForce(new Vector2(0, ySpeed));
            }
        }

	}

    void FixedUpdate()
    {
        if (isDie)
        {
            return;
        }


        isGround = Physics2D.OverlapCircle(checkGroudPos.position, radius, GroudMask);
        float dir = Input.GetAxis("Horizontal");

        isLeft = false;

        if (dir < -0f)
        {
            isLeft = true;
        }

        playerAnimator.SetFloat("Speed", dir);
        playerAnimator.SetBool("Ground", isGround);
        playerAnimator.SetBool("IsLeft", isLeft);
        playerRigidbody2D.velocity = new UnityEngine.Vector2(dir * xSpeed, playerRigidbody2D.velocity.y);
        isJump = false;

    }

    private Vector3 mTragetPos = Vector3.zero;
    void LateUpdate()
    {
        mTragetPos = GetCameraMovePos();
        if (mTragetPos != Camera.main.transform.position)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, mTragetPos, 10f);
        }
    }

    [Header("Punto de esquina inferior izquierda")]
    public Transform LeftDown;

    [Header("Punto de esquina superior derecha")]
    public Transform RightUp;

    Vector3 GetCameraMovePos()
    {
        Vector3 pos = this.transform.position;
        float screenX = SceneToWorldSize(Screen.width * 0.5f, Camera.main,
                                                pos.z);

        pos.y = Camera.main.transform.position.y;
        pos.z = Camera.main.transform.position.z;

        float maxX = RightUp.position.x;
        float minX = LeftDown.position.x;
        if (pos.x - screenX < minX)
        {
            pos.x = minX + screenX;
        }
        else if (pos.x + screenX > maxX)
        {
            pos.x = maxX - screenX;
        }

        return pos;
    }


    public float SceneToWorldSize(float size, Camera ca, float Worldz)
    {
        if (ca.orthographic)
        {
            float height = Screen.height / 2;
            float px = (ca.orthographicSize / height);
            return px * size;
        }
        else
        {
            float halfFOV = (ca.fieldOfView * 0.5f);
            halfFOV *= Mathf.Deg2Rad;

            float height = Screen.height / 2;
            float px = height / Mathf.Tan(halfFOV);
            Worldz = Worldz - ca.transform.position.z;
            return (Worldz / px) * size;
        }
    }

    public bool HandleEvent(int id, object param1, object param2)
    {
        EventDef.LevelEvent evid = (EventDef.LevelEvent)id;
        switch(evid)
        {
            case EventDef.LevelEvent.PlayerDie:
                isDie = true;
                playerRigidbody2D.velocity = new UnityEngine.Vector2(0, playerRigidbody2D.velocity.y);
                playerAnimator.SetBool("Die", isDie);
                return false;
        }
        return false;
    }

    public int EventPriority()
    {
        return 1000;
    }
}
