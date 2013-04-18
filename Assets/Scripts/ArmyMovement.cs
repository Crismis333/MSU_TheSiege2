using UnityEngine;
using System.Collections;

public class ArmyMovement : MonoBehaviour {
	
	private float scoreCountdown = 0.1f;
	private float pZ = 0;
	private float aZ = 0;
	
	private float dZ = 0;
	
	private float speedMod = 1.0f;
	
	private HeroMovement hm;
	
	void Start () {
		hm = ObstacleController.PLAYER.GetComponent<HeroMovement>();
	}
	
	// Update is called once per frame
	void Update () {
		
		pZ = ObstacleController.PLAYER.transform.position.z;
		aZ = transform.position.z;
		
		dZ = pZ-aZ;
		
		if (dZ >= 50) {
			speedMod = (dZ/45);
			speedMod = Mathf.Min(((hm.MoveSpeed+hm.SpeedUp - 0.3f)/hm.MoveSpeed),speedMod);
		}
		else {
			speedMod = 1.05f;
			
			if (dZ <= 0)
			{
				//TODO fail level
			}
		}
		
		
		transform.position += (new Vector3(0,0,hm.MoveSpeed * speedMod) * Time.deltaTime);
	
		scoreCountdown -= Time.deltaTime;
		
		if (scoreCountdown <= 0) {
			scoreCountdown = 0.1f;
			
			GUIScript.SCORE += (long)((dZ / hm.MoveSpeed) * hm.CurrentSpeed);
		}
	}
}
