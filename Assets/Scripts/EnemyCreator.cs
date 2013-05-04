using UnityEngine;
using System.Collections;

public class EnemyCreator : MonoBehaviour
{

    private float countDown;
    private float CountDownTime;
    public GameObject Enemy, SpaceChecker;

    // Use this for initialization
    void Start()
    {
		CountDownTime = RatioToSeconds(ObstacleController.SOLDIER_RATIO);
    }

    // Update is called once per frame
    void Update()
    {
        if (countDown <= 0)
        {
            float z = ObstacleController.PLAYER.transform.position.z;

            HeroMovement hm = ObstacleController.PLAYER.GetComponent<HeroMovement>();
            if (hm.CurrentSpeed > 0)
            {
                countDown = CountDownTime * (hm.MoveSpeed / hm.CurrentSpeed);
            }
            // Enemy.transform.RotateAround(new Vector3(0, 1, 0), 180);
            //    Vector3 rot = new Vector3(Enemy.transform.eulerAngles.x, Enemy.transform.eulerAngles.y + 180, Enemy.transform.eulerAngles.z);

            int num_soldiers = numberOfSoldiers(ObstacleController.SOLDIER_RATIO);
            float[] used_values = new float[num_soldiers];
            for (int i = 0; i < num_soldiers; i++)
            {
                float x_val = Random.Range(-6, 6);
                if (i > 0)
                {                    
                    while (true)
                    {
                        bool temp = false;
                        for (int j = 0; j < i; j++)
                        {
                            temp = temp || almostEqual(used_values[j], x_val, 1f);
                        }
                        if (!temp)
                        {
                            break;
                        }
                        else
                        {
                            x_val = Random.Range(-6, 6);
                        }
                    }
                }
               

                bool EnemyOK = false;
                if (z + 70 < ObstacleController.LEVEL_LENGTH_Z || LevelCreator.INF_MODE)
                {
                    //   Instantiate(SpaceChecker, new Vector3(x_val, 0.1f, z + 70), Quaternion.AngleAxis(180, Vector3.up));
                    EnemyOK = true;
                }
                if (EnemyOK)
                {
                    //Destroy(SpaceChecker);
                    Instantiate(Enemy, new Vector3(x_val, 4f, z + 70), Quaternion.AngleAxis(180, Vector3.up));
                }
            }
        }
        countDown -= Time.deltaTime;
    }

    bool almostEqual(float f1, float f2, float epsilon)
    {
        if ((f2 > (f1 - epsilon)) && (f2 < (f1 + epsilon)))
        {
            return true;
        }
        return false;
    }

    int numberOfSoldiers(int ratio)
    {
        if (ratio < 6)
        {
            return 1;
        }
        if (ratio < 9)
        {
            return 2;
        }
        return 3;
    }
	
	public float RatioToSeconds(int ratio) {
        if (ratio > 6)
            ratio = 6;
		return (5.333f * Mathf.Pow(10, -0.125076810788137f * ratio));
	}

    public void RecalcTimer()
    {
        CountDownTime = RatioToSeconds(ObstacleController.SOLDIER_RATIO);
    }
}
