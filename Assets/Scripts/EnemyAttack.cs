using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAttack : MonoBehaviour {

    private GameObject player;
    private bool isChosen; //inRange, isDone

    public float AwareRange;
    public GameObject Indicator;
    private GameObject selectedIndicator;
    public bool AttackDone;
	
	private bool destroyed = false;

    private Transform bip;

    private ParticleSystem ps;

    // Use this for initialization
    void Start()
    {
        player = ObstacleController.PLAYER;
        bip = gameObject.transform.FindChild("Bip001");
        ps = gameObject.GetComponentInChildren<ParticleSystem>();
        RecalcParticlePosition();
    }

    /// <summary>
    /// Initiates particle system
    /// </summary>
    /// <param name="power">The power that the enemy is hit with [0, 1]</param>
    public void KillSelf(float power)
    {
        RecalcParticlePosition();
        float basePower = 1f;
        float interval = 4f;
        ps.startSpeed = basePower + power * interval;
        ps.Play();
    }

    public void AddExplosion(float power, Vector3 pos)
    {
		gameObject.GetComponent<Animator>().enabled = false;
		if (!destroyed) {
           
        	foreach (Rigidbody rs in this.gameObject.GetComponentsInChildren<Rigidbody>())
        	{
            	rs.isKinematic = false;
            	rs.WakeUp();
	            rs.AddExplosionForce(power, pos, 0);
        	}
			
			foreach (Collider c in gameObject.GetComponentsInChildren<Collider>())
			{
				if (c.enabled && player.collider.enabled)
					Physics.IgnoreCollision(c,player.collider);
			}
		}
		
		destroyed = true;
    }

    public bool GetDestroyed()
    {
        return destroyed;
    }

	// Update is called once per frame
    void Update()
    {
        RecalcParticlePosition();
    }

    public void RecalcParticlePosition()
    {
        ps.transform.position = bip.position;
    }
}
