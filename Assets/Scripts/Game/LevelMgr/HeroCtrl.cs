using UnityEngine;

public class HeroCtrl : MonoBehaviour, IEventListener
{
    [Header("Rigidbody of character")] public Rigidbody2D playerRigidbody2D;

    [Header("Animator")] public Animator playerAnimator;

    [Header("chaeacter's moving speed")] public float xSpeed = 40.0f;

    [Header("character's jumping force")] public float ySpeed = 7000;

    private bool isGround = true;

    private bool isJump = false;

    private bool isLeft = false;

    private bool isDie = false;

    [Header("Check gound plane")] public Transform checkGroudPos;

    [Header("Character treading layer")] public LayerMask GroudMask;

    [Header("Determine the circular area of ​​the floor, the radius of the circle")]
    public float radius = 4.5f;

    private bool isGameOver = false;


    public GameObject DeadExplosion;


    void Awake()
    {
        
        if (AppMgr.Instance.HeroPos == Vector3.zero)
        {
            AppMgr.Instance.HeroPos = transform.position;
        }
        else
        {
            transform.position = AppMgr.Instance.HeroPos;
        }

        if (AppMgr.Instance)
        {
            AppMgr.Instance.AttachEventListener((int) EventDef.LevelEvent.PlayerDie, this);
            AppMgr.Instance.AttachEventListener((int) EventDef.LevelEvent.GameOver, this);
            AppMgr.Instance.AttachEventListener((int) EventDef.LevelEvent.SaveGame, this);
        }
    }

    void OnDestroy()
    {
        if (AppMgr.Instance)
        {
            AppMgr.Instance.DetachEventListener((int) EventDef.LevelEvent.PlayerDie, this);
            AppMgr.Instance.DetachEventListener((int) EventDef.LevelEvent.GameOver, this);
            AppMgr.Instance.DetachEventListener((int) EventDef.LevelEvent.SaveGame, this);
        }
    }

    void Start()
    {
    }

    void Update()
    {
        if (isDie || isGameOver)
        {
            return;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (isGround && !isJump)
            {
                isJump = true;
                isGround = false;
                playerRigidbody2D.AddForce(new Vector2(0, ySpeed));


                SoundManager.Instance.PlaySound("jump");
            }
        }
    }

    void FixedUpdate()
    {
        if (isDie || isGameOver)
        {
            return;
        }

        isGround = Physics2D.OverlapCircle(checkGroudPos.position, radius, GroudMask);
        float dir = Input.GetAxis("Horizontal");

        isLeft = false;

        if (dir < -0.01f)
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

    [Header("Lower left point")] public Transform LeftDown;

    [Header("Top right cpoint")] public Transform RightUp;

    Vector3 GetCameraMovePos()
    {
        Vector3 pos = this.transform.position;
        float screenX = SceneToWorldSize(Screen.width * 0.5f, Camera.main, pos.z);

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
            halfFOV *= Mathf.Deg2Rad; //Arc angle//

            float height = Screen.height / 2;
            float px = height / Mathf.Tan(halfFOV); //Get the Z axis that should be on//
            Worldz = Worldz - ca.transform.position.z;
            return (Worldz / px) * size;
        }
    }

    public bool HandleEvent(int id, object param1, object param2)
    {
        EventDef.LevelEvent evid = (EventDef.LevelEvent) id;
        switch (evid)
        {
            case EventDef.LevelEvent.PlayerDie:
                isDie = true;
                playerRigidbody2D.velocity = new UnityEngine.Vector2(0, playerRigidbody2D.velocity.y);
                playerAnimator.SetBool("Die", isDie);
                this.gameObject.SetActive(false);
                Instantiate(DeadExplosion, transform.position, transform.rotation);
                
                return false;
            case EventDef.LevelEvent.GameOver:
                isGameOver = true;
                playerRigidbody2D.velocity = new UnityEngine.Vector2(0, playerRigidbody2D.velocity.y);
                playerAnimator.SetFloat("Speed", 0);
                return false;
        }

        return false;
    }

    public int EventPriority()
    {
        return 1000;
    }
}