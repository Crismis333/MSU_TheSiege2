using UnityEngine;
using System.Collections;

public class HeroMovement : MonoBehaviour {

    public float MoveSpeed = 5.0f;
    public float StrafeSpeed = 3.0f;
	
	private float currentSpeed;
	
	public float CurrentSpeed { get { return currentSpeed; } }

    public float SpeedDown = 2.0f;

    private CollisionFlags collisionFlag;

    private float verticalSpeed;
	
	public bool Slowed = false;
	private float slowTimer = 0.0f;
	private float slowMax = 1.0f;
	private float slowAmount = 0.0f;

    private bool isControllable = true;

    private Vector3 moveDirection = Vector3.zero;

    //Jumping
    public float JumpHeight = 0.5f;
    public float Gravity = 5.0f;

    private bool jumping = false;
    private bool jumpingReachedApex = false;
    private float lastJumpTime = -1.0f;
    //private float lastJumpStartHeight = 0.0f;
    private float lastJumpButtonTime = -10.0f;
    private float jumpRepeatTime = 0.05f;
    private float jumpTimeout = 0.15f;

    private float speedUp = 0;

    public float SpeedUp
    {
        get { return speedUp; }
        set { speedUp = Mathf.Min(4.0f, value); }
    }

    private bool charging = false;

    public bool Charging { get { return charging; } }

    private float chargeSpeed = 5;
    private float chargeTimeMax = 5;
    private float chargeTime = 0;
    [HideInInspector]
    public float Rage = 0;

    private Animator anim;

	// Use this for initialization
	void Start () {
        currentSpeed = MoveSpeed;

        anim = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Jump"))
        {
            lastJumpButtonTime = Time.time;
        }

        if (Input.GetButtonDown("Fire2") && Rage >= 1) {

            charging = true;
            chargeTime = chargeTimeMax;
            Rage = 0;
        }

		Run();

        ApplyGravity();

        ApplyJumping();

        moveDirection.y = verticalSpeed;

        if (jumping) {
            moveDirection.x += 0.289f;
            moveDirection.z -= 1.031f; //animation error fix
        }

        moveDirection *= Time.deltaTime;

        CharacterController cc = GetComponent<CharacterController>();
        collisionFlag = cc.Move(moveDirection); 

        if (IsGrounded() && jumping)
        {
            jumping = false;
            anim.SetBool("Jump", false);
            SendMessage("DidLand", SendMessageOptions.DontRequireReceiver);
        }
		
		if (Slowed) {			
			slowTimer -= Time.deltaTime;
			if (slowTimer <= 0.0f) {
				Slowed = false;
				slowTimer = 0.0f;
			}
		}

        if (Charging)
        {
            chargeTime -= Time.deltaTime;
            if (chargeTime <= 0.0f) {
                charging = false;
                chargeTime = 0.0f;
            }
        }

        if (Rage >= 0 && Rage < 1) {
            Rage = Mathf.Max(Rage - (0.05f * Time.deltaTime), 0);
        }
		
		if (transform.position.z >= LevelCreator.LengthConverter(LevelCreator.LEVEL_LENGTH)*64-32 && !LevelCreator.INF_MODE) {
            Camera.mainCamera.GetComponent<GUIScript>().CompleteLevel();
		}
	}
	
	private void Run() {
        moveDirection = Vector3.zero;

        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        if (v < -0.1 && !charging)
            moveDirection.z = MoveSpeed - Mathf.Max(slowAmount * (slowTimer / slowMax), SpeedDown * -v) + SpeedUp;
        else if (charging)
            moveDirection.z = MoveSpeed + SpeedUp + chargeSpeed;
        else
            moveDirection.z = MoveSpeed - slowAmount * (slowTimer / slowMax) + SpeedUp;

        if (SpeedUp <= 0)
            SpeedUp = 0;
        else
            SpeedUp -= 0.2f * Time.deltaTime;

		currentSpeed = moveDirection.z;
        
        moveDirection.x = StrafeSpeed * h;
	}

    public void ApplyGravity()
    {
        if (isControllable)
        {
            //bool jumpButton = Input.GetButton("Jump");

            if (jumping && !jumpingReachedApex && verticalSpeed <= 0.0)
            {
                jumpingReachedApex = true;
                SendMessage("DidJumpReachApex", SendMessageOptions.DontRequireReceiver);
            }

            if (IsGrounded())
                verticalSpeed = 0.0f;
            else
                verticalSpeed -= Gravity * Time.deltaTime;
        }
    }

    public void ApplyJumping()
    {
        if (lastJumpTime + jumpRepeatTime > Time.time)
            return;

        if (IsGrounded())
        {
            if (Time.time < lastJumpButtonTime + jumpTimeout)
            {
                verticalSpeed = CalculateJumpVerticalSpeed(JumpHeight);
                SendMessage("DidJump", SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    private float CalculateJumpVerticalSpeed(float targetJumpHeight)
    {
        return Mathf.Sqrt(2 * targetJumpHeight * Gravity);
    }
	
	public void SlowHero(float time, float amount) {
		if (Slowed) {
			if (amount > (slowAmount * (slowTimer / slowMax))) {
				slowTimer = time;
				slowMax = time;
				slowAmount = amount * (CurrentSpeed / MoveSpeed);
			}
		}
		else {
			Slowed = true;
			slowTimer = time;
			slowMax = time;
			slowAmount = amount * (CurrentSpeed / MoveSpeed);
		}

        SpeedUp = 0;
        Rage = 0;
        GUIScript.PERFECT_RUN = false;
    }

    private bool IsGrounded()
    {
        return (collisionFlag & CollisionFlags.CollidedBelow) != 0;
    }

    private void DidJump()
    {

        jumping = true;
        anim.SetBool("Jump", true);
        jumpingReachedApex = false;
        lastJumpTime = Time.time;
        //lastJumpStartHeight = transform.position.y;
        lastJumpButtonTime -= 10;

        //State = jumping;
    }
}
