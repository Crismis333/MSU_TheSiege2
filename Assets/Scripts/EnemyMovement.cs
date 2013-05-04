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
                RaycastHit rh;
                if (Physics.Raycast(transform.position, d, out rh, 6.0f))
                {
                    transform.position = rh.point;
                }
                snapped = true;
            }

            fixedCounter++;
        }
    }
}
