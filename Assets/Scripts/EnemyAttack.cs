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

    private ParticleSystem ps;

    // Use this for initialization
    void Start()
    {
        player = ObstacleController.PLAYER;
        ps = gameObject.GetComponentInChildren<ParticleSystem>();
        //ps.Stop();
    }

    //void enterInRange()
    //{
    //    print("Is in range");
    //    selectedIndicator = (GameObject)Instantiate(Indicator, gameObject.transform.position + new Vector3(0, 3, 0), gameObject.transform.rotation);
    //    selectedIndicator.transform.parent = gameObject.transform;
    //}

    //public void SetChosen(bool value)
    //{
    //    if (inRange)
    //    {
    //        isChosen = value;
    //        if (value)
    //        {
    //            selectedIndicator.renderer.material.color = Color.green;
    //        }
    //        else
    //        {
    //            selectedIndicator.renderer.material.color = Color.white;
    //        }
    //    }
    //}

    void OnDestroy()
    {
	//	if (player != null && gameObject != null)
        //	player.GetComponent<HeroAttack>().RemoveFromList(gameObject, isChosen);
    }

    /// <summary>
    /// Initiates particle system
    /// </summary>
    /// <param name="power">The power that the enemy is hit with [0, 1]</param>
    public void KillSelf(float power)
    {
        //inRange = false;
        //isDone = true;
     //   player.GetComponent<HeroAttack>().RemoveFromList(gameObject, isChosen);
      //  Destroy(selectedIndicator);
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
        if (destroyed)
        {
            ps.transform.position = gameObject.transform.FindChild("Bip001").transform.position;
        }
        //if (!inRange)
        //{
        //    if (gameObject.transform.position.z < player.transform.position.z + AwareRange)
        //    {
        //        inRange = true;
        //      //  enterInRange();
        //     //   player.GetComponent<HeroAttack>().AddToList(gameObject);


        //    }
        //}
        //else
        //{
        //if (gameObject.transform.position.z < player.transform.position.z)
        //{
            //inRange = false;
            //isDone = true;
        //    player.GetComponent<HeroAttack>().RemoveFromList(gameObject, isChosen);
        //    Destroy(selectedIndicator);
            
        //}
    }
        
	
}
