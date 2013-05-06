using UnityEngine;
using System.Collections;

public class BoulderBehaviour : MonoBehaviour {

    public float SlowTime = 2;
    public float SlowAmount = 5;


    public EffectVolumeSetter CrashSound;
	
	private ParticleSystem ps;
	
	bool hitGround = false;
	Vector3 hitPos;

    void Start()
    {
		ps = gameObject.GetComponentInChildren<ParticleSystem>();
		//ps.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z < ObstacleController.PLAYER.transform.position.z - 64)
        {
            Destroy(gameObject);
        }
		ps.transform.rotation = Quaternion.Euler(0,0,0);
        if (!hitGround)
            ps.transform.position = transform.position + new Vector3(0, -0.9f, 0);
        else
            ps.transform.position = hitPos;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            HeroMovement hm = other.GetComponent<HeroMovement>();
            Vector3 exppos = ObstacleController.PLAYER.transform.position;
            exppos.y += 1;
            exppos.z -= 1;
            gameObject.GetComponent<Rigidbody>().AddExplosionForce((hm.CurrentSpeed / 4)*10, exppos, 0);
            Physics.IgnoreCollision(gameObject.collider, other);
            hm.SlowHero(SlowTime, SlowAmount);
        }
		
		if (other.tag.Equals("Soldier"))
		{
			other.GetComponent<EnemyAttack>().KillSelf(0.7f);
			other.GetComponent<EnemyAttack>().AddExplosion(600,this.transform.position);
            Physics.IgnoreCollision(gameObject.collider, ObstacleController.PLAYER.collider);
		}
		
		if (other.tag.Equals("Road") && !hitGround) {
			ps.Play();
			hitGround = true;
            CrashSound.Play();
			hitPos = ps.transform.position;
		}
    }
}
