using UnityEngine;
using System.Collections;

public class HeroMovement : MonoBehaviour {

    public float MoveSpeed = 5.0f;
    public float StrafeSpeed = 3.0f;
    public float SpeedDown = 2.0f;

	private float currentSpeed;
	public float CurrentSpeed { get { return currentSpeed; } }
       
    private CollisionFlags collisionFlag;
    private float verticalSpeed;

	public bool Slowed = false;
	private float slowTimer = 0.0f;
	private float slowMax = 1.0f;
	private float slowAmount = 0.0f;

    private bool isControllable = true;
    public bool complete;
    public bool dead;

    private Vector3 moveDirection = Vector3.zero;
    private Quaternion defaultRot;

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

    private GUIScript GUI;
    private HeroAttack ha;

    private float speedUp = 0;

    public float SpeedUp
    {
        get { return speedUp; }
        set { speedUp = Mathf.Min(4.0f, value); }
    }

    private bool charging = false;

    public bool Charging { get { return charging; } }

    public EffectVolumeSetter SoundCharge;

    private float chargeSpeed = 5;
    private float chargeTimeMax = 5;
    private float chargeTime = 0;
    [HideInInspector]
    public float Rage = 0;

    private Animator anim;

    private ArmyMovement am;

    public float GetSlowed()
    {
        if (slowMax != 0)
        {
            return slowTimer / slowMax;
        }
        return 0;
    }

    public void Kill()
    {
        dead = true;
        isControllable = false;
        complete = true;
        Camera.mainCamera.GetComponent<GUIScript>().LoseLevel();
        Vector3 pos = transform.position;
        pos.z -= 5;
        gameObject.GetComponent<Animator>().enabled = false;
        //GetComponent<CharacterController>().
        foreach (CapsuleCollider rs in this.gameObject.GetComponentsInChildren<CapsuleCollider>())
        {
            rs.isTrigger = false;
        }
        foreach (BoxCollider rs in this.gameObject.GetComponentsInChildren<BoxCollider>())
        {
            rs.isTrigger = false;
        }
        foreach (SphereCollider rs in this.gameObject.GetComponentsInChildren<SphereCollider>())
        {
            rs.isTrigger = false;
        }
        foreach (Rigidbody rs in this.gameObject.GetComponentsInChildren<Rigidbody>())
        {
            rs.isKinematic = false;
            rs.WakeUp();
            rs.AddExplosionForce(1f, pos, 0);
        }
    }

	// Use this for initialization
	void Start () {
        foreach (CapsuleCollider rs in this.gameObject.GetComponentsInChildren<CapsuleCollider>())
        {
            rs.isTrigger = true;
        }
        foreach (BoxCollider rs in this.gameObject.GetComponentsInChildren<BoxCollider>())
        {
            rs.isTrigger = true;
        }
        foreach (SphereCollider rs in this.gameObject.GetComponentsInChildren<SphereCollider>())
        {
            rs.isTrigger = true;
        }
        foreach (Rigidbody rs in this.gameObject.GetComponentsInChildren<Rigidbody>())
        {
            rs.Sleep();
        }
        currentSpeed = MoveSpeed;
        dead = false;
        defaultRot = transform.rotation;
        complete = false;
        anim = gameObject.GetComponent<Animator>();
        GUI = Camera.mainCamera.GetComponent<GUIScript>();
        am = GameObject.Find("FellowHeroes").GetComponent<ArmyMovement>();
        ha = gameObject.GetComponent<HeroAttack>();
	}
	
	// Update is called once per frame
	void Update () {
        transform.rotation = defaultRot;
        float v = 0;
        float h = 0;

        if (isControllable)
        {
            v = Input.GetAxisRaw("Vertical");
            h = Input.GetAxisRaw("Horizontal");

            if (Input.GetButtonDown("Jump") || v > 0.3f)
            {
                lastJumpButtonTime = Time.time;
            }

            if (Input.GetButtonDown("Fire2") && Rage >= 1 && !charging)
            {

                charging = true;
                chargeTime = chargeTimeMax;
                SoundCharge.Play();
                anim.SetBool("Charge", true);
            }
        }

        Run(v,h);

        ApplyGravity();

        ApplyJumping();

        moveDirection.y = verticalSpeed;

        if (jumping)
        {
            moveDirection.x += 0.003f;
            moveDirection.z -= 2.0f; //animation error fix
        }

        moveDirection *= Time.deltaTime;

        if (!dead)
        {
            CharacterController cc = GetComponent<CharacterController>();
            collisionFlag = cc.Move(moveDirection);
        }

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
                anim.SetBool("Charge", false);
            }
            Rage = chargeTime / chargeTimeMax;
        }

        if (Rage >= 0 && Rage < 1) {
            Rage = Mathf.Max(Rage - (0.03f * Time.deltaTime), 0);
        }

        if (transform.position.z >= LevelCreator.LengthConverter(LevelCreator.LEVEL_LENGTH) * 64 - 32 && !LevelCreator.INF_MODE && !complete)
        {
            complete = true;
            Camera.mainCamera.GetComponent<GUIScript>().CompleteLevel();
            isControllable = false;
		}
	}
	
	private void Run(float v, float h) {
        moveDirection = Vector3.zero;

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

        // Change this to true to use non-smoothed input
        if (UseSmoothedInput)
        {
            float change = StrafeSpeed * h; // in [-9, 9]
            moveDirection.x = InputSmoothing(change);
        }
        else
        {
            moveDirection.x = StrafeSpeed * h;
        }
	}

    private float curSpeed = 0, interval = 50;
    public bool UseSmoothedInput;

    float InputSmoothing(float change)
    {       
        if (curSpeed < change)
        {
            curSpeed += interval * Time.deltaTime;
        }
        else if (curSpeed > change)
        {
            curSpeed -= interval * Time.deltaTime;            
        }
        if (change == 0 && (curSpeed < 1.5f * interval * Time.deltaTime || curSpeed > -1.5f * interval * Time.deltaTime))
        {
            curSpeed = 0;
        }       
        return curSpeed;
    }

    public void ApplyGravity()
    {
        if (jumping && !jumpingReachedApex && verticalSpeed <= 0.0)
        {
            jumpingReachedApex = true;
            SendMessage("DidJumpReachApex", SendMessageOptions.DontRequireReceiver);
        }

        if (IsGrounded() && !jumping)
            verticalSpeed = 0.0f;
        else
            verticalSpeed -= Gravity * Time.deltaTime;
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
        if (!charging)
        {
            SpeedUp = 0;
            Rage = 0;
            GUIScript.PERFECT_RUN = false;
            
            am.Trample();
        }
        else
            return;

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
        GUI.lastEngagePercent = 0;
        GUI.BarActive = false;
        ha.SetCharging(false);
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
