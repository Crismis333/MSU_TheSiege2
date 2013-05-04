using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstacleCreator : MonoBehaviour {

    private float countDown;
    private float countDownTime;

    public List<GameObject> ObstacleList;

	// Use this for initialization
	void Start () {
        countDownTime = RatioToSeconds(ObstacleController.OBSTACLE_RATIO);
	}
	
	// Update is called once per frame
	void Update () {
        if (countDown <= 0)
        {
            float z = ObstacleController.PLAYER.transform.position.z;
            
            HeroMovement hm = ObstacleController.PLAYER.GetComponent<HeroMovement>();
            if (hm.CurrentSpeed > 0)
                countDown = countDownTime * (hm.MoveSpeed / hm.CurrentSpeed);

            int rIndex = Random.Range(0, ObstacleList.Count);

            if (z + 70 < ObstacleController.LEVEL_LENGTH_Z || LevelCreator.INF_MODE)
    	    	Instantiate(ObstacleList[rIndex], new Vector3(Random.Range(-6, 7), 4f, z + 70), Quaternion.AngleAxis(180, Vector3.up));
        }
        countDown -= Time.deltaTime;
	}

    public float RatioToSeconds(int ratio)
    {
        return (5.333f * Mathf.Pow(10, -0.1f * ratio));
    }

    public void RecalcTimer() 
    {
        countDownTime = RatioToSeconds(ObstacleController.OBSTACLE_RATIO);
    }
}
