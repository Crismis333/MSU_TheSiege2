using UnityEngine;
using System.Collections;
using System;

public class ObstacleBehaviour : MonoBehaviour {
	
	public float SlowTime = 2;
	public float SlowAmount = 5;
	
	bool destroyed = false;

    private float moveOffset = 1.0f;

    private bool snapped = false;
    private int fixedCounter = 0;

    public EffectVolumeSetter SoundObstacle;
    private GUIScript GUI;

    void Start()
    {
        GUI = Camera.mainCamera.GetComponent<GUIScript>();
    }
	
	// Update is called once per frame
	void Update () {
        if (transform.position.z < ObstacleController.PLAYER.transform.position.z - 64)
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

	void OnTriggerEnter(Collider other) {
		if (!destroyed && other.tag.Equals("Player")) {
			foreach(Rigidbody rb in this.GetComponentsInChildren<Rigidbody>())
			{
				rb.isKinematic = false;
                Vector3 exppos = ObstacleController.PLAYER.transform.position;
                exppos.y += 1;
				rb.AddExplosionForce(other.GetComponent<HeroMovement>().CurrentSpeed/4,exppos,0);

                if (rb.gameObject.collider != null)
                    Physics.IgnoreCollision(rb.gameObject.collider, ObstacleController.PLAYER.collider);
                else
                    Physics.IgnoreCollision(rb.gameObject.GetComponentInChildren<BoxCollider>(), ObstacleController.PLAYER.collider);
			}

            HeroMovement hm = other.GetComponent<HeroMovement>();

            if (hm.Charging)
            {
                GUI.DestroyObstacle();
            }
            else
            {
                hm.SlowHero(SlowTime, SlowAmount);
            }
            if (SoundObstacle != null)
            {
                SoundObstacle.Play();
            }
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
			destroyed = true;
            if (SoundObstacle != null)
                SoundObstacle.Play();
		}
	}
}
