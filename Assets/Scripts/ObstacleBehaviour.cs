using UnityEngine;
using System.Collections;

public class ObstacleBehaviour : MonoBehaviour {
	
	public float SlowTime = 2;
	public float SlowAmount = 5;
	
	bool destroyed = false;

    private ParticleSystem ps;
	
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
				rb.AddExplosionForce(other.GetComponent<HeroMovement>().CurrentSpeed/4,other.transform.position + Vector3.up,0);
				
				Physics.IgnoreCollision(rb.gameObject.collider, other);
			}
			
			other.GetComponent<HeroMovement>().SlowHero(SlowTime,SlowAmount);
            if (ps != null)
                ps.Play();
			destroyed = true;
		}

        if (!destroyed && other.tag.Equals("Soldier"))
        {
            print("Moving soldier");
            float x = other.transform.position.x;
            if (x < 0)
            {
                // Move right
                other.transform.position = other.transform.position + new Vector3(1, 0, 0);
            }
            else
            {
                // Move left
                other.transform.position = other.transform.position + new Vector3(-1, 0, 0);
            }
        }
		
		if (!destroyed && other.tag.Equals("Boulder")) {
			foreach(Rigidbody rb in this.GetComponentsInChildren<Rigidbody>())
			{
				rb.isKinematic = false;
				rb.AddExplosionForce(3.5f,other.transform.position,0);
				
				Physics.IgnoreCollision(rb.gameObject.collider, ObstacleController.PLAYER.collider);
				Physics.IgnoreCollision(other.collider, ObstacleController.PLAYER.collider);
			}
            if (ps != null)
                ps.Play();
			destroyed = true;
		}
	}
}
