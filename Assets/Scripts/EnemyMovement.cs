using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {

    public float MoveSpeed = 3.0f;
    private GameObject player;
	
	private Animator anim;

	// Use this for initialization
	void Start () {
        player = ObstacleController.PLAYER;
		anim = gameObject.GetComponent<Animator>();
		anim.SetInteger("State",0);
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
}
