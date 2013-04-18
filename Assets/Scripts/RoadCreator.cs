using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoadCreator : MonoBehaviour {

    public List<GameObject> Roads;
	
	private GameObject player;
	private int moduleLength = 64;
	
	private GameObject thisRoad = null;

	// Use this for initialization
	void Start () {
        player = ObstacleController.PLAYER;
		
		int rIndex = Random.Range(0,Roads.Count);
		
		thisRoad = Instantiate(Roads[rIndex],transform.position,Roads[rIndex].transform.rotation) as GameObject;
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.position.z <= player.transform.position.z - moduleLength){
			Destroy(thisRoad.gameObject);
			Destroy(gameObject);
        }
	}
}
