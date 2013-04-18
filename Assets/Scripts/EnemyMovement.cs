using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {

    public float MoveSpeed = 3.0f;
    private GameObject player;

	// Use this for initialization
	void Start () {
        player = ObstacleController.PLAYER;
	}
	
	// Update is called once per frame
	void Update () {
        Rigidbody body = gameObject.GetComponent<Rigidbody>();

        //Vector3 pos = body.position;
        //pos.z -= MoveSpeed * Time.deltaTime;

        body.MovePosition(body.position + new Vector3(0,0,-MoveSpeed * Time.deltaTime));


        float bodyZ = body.position.z;
        float playerZ = player.transform.position.z;

      //  Debug.Log("Body: " + bodyZ.ToString());
      //  Debug.Log("Player: " + playerZ.ToString());   

        if (bodyZ < playerZ - 64)
        {
            Destroy(gameObject);
        }
	}
}
