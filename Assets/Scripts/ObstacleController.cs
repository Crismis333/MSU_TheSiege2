using UnityEngine;
using System.Collections;

public class ObstacleController : MonoBehaviour {
	
	public static int SOLDIER_RATIO = 1;
    public static int CATAPULT_RATIO = 1;
    public static int PIT_RATIO = 1;
    public static int OBSTACLE_RATIO = 1;
	
	public static GameObject PLAYER = null;
	public static GameObject ARMY = null;
	
	public static float MOVEMENT_MODIFIER = 1;
	public static float JUMP_MODIFIER = 1;
	public static float CHARGE_MODIFIER = 1;

    void Start()
    {
        PLAYER = GameObject.Find("Hero");
		ARMY = GameObject.Find("FellowHeroes");
		
		PLAYER.GetComponent<HeroMovement>().MoveSpeed *= MOVEMENT_MODIFIER;
		PLAYER.GetComponent<HeroMovement>().StrafeSpeed *= MOVEMENT_MODIFIER;
		
		//PLAYER.GetComponent<HeroMovement>().Gravity *= (2 - JUMP_MODIFIER);
    }
}
