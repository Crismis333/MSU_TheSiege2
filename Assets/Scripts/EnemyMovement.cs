using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {

    public float MoveSpeed = 3.0f;
    private GameObject player;
	
	private Animator anim;

    private bool snapped = false;
    private int fixedCounter = 0;

	// Use this for initialization
	void Start () {
        player = ObstacleController.PLAYER;
		anim = gameObject.GetComponent<Animator>();
		//anim.SetInteger("State",0);
	}
	
	// Update is called once per frame
	void Update () {
        Rigidbody body = gameObject.GetComponent<Rigidbody>();

        float bodyZ = body.position.z;
        float playerZ = player.transform.position.z;

        if (bodyZ < playerZ - 32)
        {
            Destroy(gameObject);
        }
	}

    void FixedUpdate()
    {
        if (!snapped)
        {
            if (fixedCounter == 2)
            {
                Vector3 d = transform.TransformDirection(Vector3.down);
                Vector3 newPos;
                RaycastHit rh;
                if (Physics.Raycast(transform.position, d, out rh, 6.0f))
                {
                    Vector3 offset = Vector3.zero;
                    newPos = rh.point;
                    if (rh.collider.tag.Equals("Soldier") || rh.collider.tag.Equals("Obstacle"))
                    {
                        RaycastHit newRayHit;
                        while(Physics.Raycast(transform.position + offset, d, out newRayHit, 6.0f)) {
                            if (!newRayHit.collider.tag.Equals("Soldier") && !newRayHit.collider.tag.Equals("Obstacle"))
                            {
                                newPos = newRayHit.point;
                                break;
                            }
                            offset.x += 1f;
                            if (offset.x >= 6)
                                offset.x = -6;
                        }
                    }

                    transform.position = newPos;
                }
                snapped = true;
                GetComponent<EnemyAttack>().RecalcParticlePosition();
            }

            fixedCounter++;
        }
    }
}
