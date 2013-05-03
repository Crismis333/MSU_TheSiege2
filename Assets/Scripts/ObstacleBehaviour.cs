using UnityEngine;
using System.Collections;
using System;

public class ObstacleBehaviour : MonoBehaviour {
	
	public float SlowTime = 2;
	public float SlowAmount = 5;
	
	bool destroyed = false;

    private ParticleSystem ps;

    private float moveOffset = 1.0f;
	
	// Update is called once per frame
	void Update () {
        if (transform.position.z < ObstacleController.PLAYER.transform.position.z - 64)
        {
            Destroy(gameObject);
        }
	}


    void Start()
    {
        ps = gameObject.GetComponentInChildren<ParticleSystem>();
        if (ps != null)
            ps.Pause();
    }

	void OnTriggerEnter(Collider other) {
		if (!destroyed && other.tag.Equals("Player")) {
			foreach(Rigidbody rb in this.GetComponentsInChildren<Rigidbody>())
			{
				rb.isKinematic = false;
                Vector3 exppos = ObstacleController.PLAYER.transform.position;
                exppos.y = 1;
				rb.AddExplosionForce(other.GetComponent<HeroMovement>().CurrentSpeed/4,exppos,0);

                if (rb.gameObject.collider != null)
                    Physics.IgnoreCollision(rb.gameObject.collider, ObstacleController.PLAYER.collider);
                else
                    Physics.IgnoreCollision(rb.gameObject.GetComponentInChildren<BoxCollider>(), ObstacleController.PLAYER.collider);
			}
			
			other.GetComponent<HeroMovement>().SlowHero(SlowTime,SlowAmount);
            if (ps != null)
                ps.Play();
			destroyed = true;
		}

        if (!destroyed && other.tag.Equals("Soldier"))
        {
         //   Instantiate(new GameObject(), other.transform.position + Vector3.up * 2, Quaternion.identity);

            Bounds bounds = gameObject.GetComponent<BoxCollider>().bounds;
            float left = bounds.min.x;
            float right = bounds.max.x;
         //   print("left: " + left + " right: " + right);
            if (!other.GetComponent<EnemyAttack>().GetDestroyed())
            {
                float diff = other.transform.position.z - ObstacleController.PLAYER.transform.position.z;
                if (diff > 20)
                {
                    if (Math.Abs(left) > Math.Abs(right))
                    {
                        // Move right
                        if (other.transform.position.x < 5)
                        {
                            other.transform.position = new Vector3(right + moveOffset, other.transform.position.y, other.transform.position.z);
                            moveOffset += 0.8f;
                        }
                        else
                        {
                            // Should never be called
                            other.transform.position = other.transform.position + new Vector3(other.transform.position.x - 8, 0, 0);
                        }
                    }
                    else
                    {
                        // Move left
                        if (other.transform.position.x > -5)
                        {
                            other.transform.position = new Vector3(left - moveOffset, other.transform.position.y, other.transform.position.z);
                            moveOffset += 0.8f;
                        }
                        else
                        {
                            // Should never be called
                            other.transform.position = other.transform.position + new Vector3(other.transform.position.x + 8, 0, 0);
                        }
                    }
                }
            }
        }
		
		if (!destroyed && other.tag.Equals("Boulder")) {
			foreach(Rigidbody rb in this.GetComponentsInChildren<Rigidbody>())
			{
				rb.isKinematic = false;
				rb.AddExplosionForce(3.5f,other.transform.position,0);

                if (rb.gameObject.collider != null)
                    Physics.IgnoreCollision(rb.gameObject.collider, ObstacleController.PLAYER.collider);
                else
                    Physics.IgnoreCollision(rb.gameObject.GetComponentInChildren<BoxCollider>(), ObstacleController.PLAYER.collider);

				Physics.IgnoreCollision(other.collider, ObstacleController.PLAYER.collider);
			}
            if (ps != null)
                ps.Play();
			destroyed = true;
		}
	}
}
