using UnityEngine;
using System.Collections;

public class ObstacleController : MonoBehaviour {
	
	public static int SOLDIER_RATIO = 1;
    public static int CATAPULT_RATIO = 1;
    public static int OBSTACLE_RATIO = 1;

    public static float LEVEL_LENGTH_Z = 1;

	public static GameObject PLAYER = null;
	public static GameObject ARMY = null;
	
    public float DifficultyIncreaseTimer = 15;
    private bool maxDifficulty = false;
    private float timer;

    private ObstacleCreator oc;
    private EnemyCreator ec;
    private BoulderCreator bc;

    void Start()
    {
        PLAYER = GameObject.Find("Hero");
		ARMY = GameObject.Find("FellowHeroes");

        oc = GetComponentInChildren<ObstacleCreator>();
        ec = GetComponentInChildren<EnemyCreator>();
        bc = GetComponentInChildren<BoulderCreator>();

        timer = DifficultyIncreaseTimer;

        LEVEL_LENGTH_Z = LevelCreator.LengthConverter(LevelCreator.LEVEL_LENGTH) * 64 - 32;

        if (LevelCreator.INF_MODE)
        {
            SOLDIER_RATIO = 1;
            CATAPULT_RATIO = 1;
            OBSTACLE_RATIO = 1;
            GUIScript.DIFFICULTY_INCREASE = 1;

            GUIScript.MAX_TIMER = DifficultyIncreaseTimer;
            GUIScript.INF_TIMER = 0;
        }
    }

    void Update()
    {
        if (LevelCreator.INF_MODE)
        {
            if (!maxDifficulty && SOLDIER_RATIO == 10 && CATAPULT_RATIO == 10 && OBSTACLE_RATIO == 10)
            {
                maxDifficulty = true;
            }

            if (timer <= 0)
            {
                GUIScript.DIFFICULTY_INCREASE++;
                timer = DifficultyIncreaseTimer;

                if (maxDifficulty)
                {
                    ARMY.GetComponent<ArmyMovement>().InfSpeedMod += 0.1f;
                    return;
                }

                bool done = false;
                while (!done)
                {
                    int r = Random.Range(0, 3);

                    switch (r)
                    {
                        case 0:
                            if (!(SOLDIER_RATIO - 1 > CATAPULT_RATIO || SOLDIER_RATIO - 1 > OBSTACLE_RATIO) && SOLDIER_RATIO <= 9)
                            {
                                SOLDIER_RATIO++;
                                ec.RecalcTimer();
                                done = true;
                            }
                            break;
                        case 1:
                            if (!(CATAPULT_RATIO - 1 > SOLDIER_RATIO || CATAPULT_RATIO - 1 > OBSTACLE_RATIO) && CATAPULT_RATIO <= 9)
                            {
                                CATAPULT_RATIO++;
                                bc.RecalcTimer();
                                done = true;
                            }
                            break;
                        case 2:
                            if (!(OBSTACLE_RATIO - 1 > SOLDIER_RATIO || OBSTACLE_RATIO - 1 > CATAPULT_RATIO) && OBSTACLE_RATIO <= 9)
                            {
                                OBSTACLE_RATIO++;
                                oc.RecalcTimer();
                                done = true;
                            }
                            break;
                    }
                }
            }
            else
            {
                timer -= Time.deltaTime;
            }

            GUIScript.INF_TIMER = timer;
        }
    }
}
