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

    // Use this for initialization
    void Start()
    {
        player = ObstacleController.PLAYER;
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

    public void KillSelf()
    {
        //inRange = false;
        //isDone = true;
     //   player.GetComponent<HeroAttack>().RemoveFromList(gameObject, isChosen);
        Destroy(selectedIndicator);
    }

    public void AddExplosion(float power, Vector3 pos)
    {
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
	
	// Update is called once per frame
    void Update()
    {

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
