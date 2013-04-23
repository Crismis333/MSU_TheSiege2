using UnityEngine;
using System.Collections;

public class EnemyCreator : MonoBehaviour
{

    private float countDown;
    private float CountDownTime;
    public GameObject Enemy, SpaceChecker;

    private float moduleCount;

    // Use this for initialization
    void Start()
    {
		CountDownTime = RatioToSeconds(ObstacleController.SOLDIER_RATIO);
        moduleCount = LevelCreator.LengthConverter(LevelCreator.LEVEL_LENGTH);
    }

    // Update is called once per frame
    void Update()
    {
        if (countDown <= 0)
        {
            float z = ObstacleController.PLAYER.transform.position.z;

            HeroMovement hm = ObstacleController.PLAYER.GetComponent<HeroMovement>();
            if (hm.CurrentSpeed > 0)
                countDown = CountDownTime * (hm.MoveSpeed / hm.CurrentSpeed);

            // Enemy.transform.RotateAround(new Vector3(0, 1, 0), 180);
            //    Vector3 rot = new Vector3(Enemy.transform.eulerAngles.x, Enemy.transform.eulerAngles.y + 180, Enemy.transform.eulerAngles.z);
            float x_val = Random.Range(-6, 6);


            bool EnemyOK = false;
            if (z + 70 < moduleCount * 64 - 32 || LevelCreator.INF_MODE)
            {
             //   Instantiate(SpaceChecker, new Vector3(x_val, 0.1f, z + 70), Quaternion.AngleAxis(180, Vector3.up));
                EnemyOK = true;
            }
            if (EnemyOK)
            {
                //Destroy(SpaceChecker);
                Instantiate(Enemy, new Vector3(x_val, 0.1f, z + 70), Quaternion.AngleAxis(180, Vector3.up));
            }
        }
        countDown -= Time.deltaTime;
    }
	
	public float RatioToSeconds(int ratio) {
		return (5.333f * Mathf.Pow(10, -0.125076810788137f * ratio));
	}
}
