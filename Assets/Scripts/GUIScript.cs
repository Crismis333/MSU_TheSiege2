using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIScript : MonoBehaviour {
	
	public static long SCORE;
	
    public GUISkin gSkin;
    public Texture2D runningSoldiers, runningGoal, damagebar, swordLeft, swordRight, bloodsplatter;

    public float engagePercent, releasePercent;
    public float fixedEngagePercent;
    public bool engageFixed, releaseFixed;
    public bool BarActive;
    public float ShowLingerTime = 1.0f;
    public List<HitAccuracy> HitList;

    private float lingerTimer;
    private bool showLinger;

	private float currentZ;
	private float armyZ;
	private float minZ = 0;
	private float maxZ;

    

	// Use this for initialization
	void Start () {
		maxZ = (LevelCreator.LengthConverter(LevelCreator.LEVEL_LENGTH)*64)-32;
        HitList = new List<HitAccuracy>();
	}
	
	// Update is called once per frame
	void Update () {
		currentZ = ObstacleController.PLAYER.transform.position.z;
		armyZ = ObstacleController.ARMY.transform.position.z;

        if (showLinger)
        {
            if (lingerTimer > ShowLingerTime)
            {
                engagePercent = 0;
                releasePercent = 0;
                fixedEngagePercent = 0;
                engageFixed = false;
                showLinger = false;
            }
            else
            {
                lingerTimer += Time.deltaTime;
            }
        }
	}

    void Level_Interface()
    {
        GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
        GUI.Box(new Rect(Screen.width-225, 25, 200, 75), "");
        GUI.Label(new Rect(Screen.width - 225 + 15, 25 + 15, 200, 75), "Score: "+ SCORE);
        GUI.Label(new Rect(Screen.width - 225 + 15, 25 + 15*2, 200, 75), "Multiplier: x2");

        GUI.Box(new Rect(15, Screen.height - 65, Screen.width - 30, 50), "");
        GUI.DrawTexture(new Rect(15+currentZ/(maxZ-minZ)*Screen.width-50, Screen.height - 64, 50, 50), runningSoldiers);
        GUI.DrawTexture(new Rect(15+armyZ/(maxZ-minZ)*Screen.width-50, Screen.height - 64, 50, 50), runningSoldiers);
        GUI.DrawTexture(new Rect(Screen.width-65, Screen.height - 64, 50, 50), runningSoldiers);

        GUI.DrawTexture(new Rect(Screen.width / 2 - 250, Screen.height - 90, 500, 12), damagebar);
     //   GUI.DrawTexture(new Rect(Screen.width / 2 - 250 - swordLeft.width / 2, Screen.height - 90 - swordLeft.height / 2 + 6, swordLeft.width, swordLeft.height), swordLeft);
    //    GUI.DrawTexture(new Rect(Screen.width / 2 + 250 - swordRight.width / 2, Screen.height - 90 - swordRight.height / 2 + 6, swordRight.width, swordRight.height), swordRight);
        GUI.DrawTexture(new Rect(Screen.width / 2 - bloodsplatter.width / 2, Screen.height - 90 - bloodsplatter.height / 2 + 6, bloodsplatter.width, bloodsplatter.height), bloodsplatter);
        GUI.EndGroup();
    }

    void OnGUI()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            this.enabled = false;
            Time.timeScale = 0;
            GetComponent<PauseMenuScript>().enabled = true;
        }
        else
        {

            GUI.Label(new Rect(10, 50, 80, 30), "" + Time.timeSinceLevelLoad);
            GUI.skin = gSkin;
            Level_Interface();

            EngageReleaseBar();
        }
    }

    void EngageReleaseBar()
    {

        handleEngagement();
        //  handleRelease(120, 55, 100, 20);
    }

    void handleEngagement()
    {
        int width = 500;
       
        if (BarActive)
        {
            if (engagePercent > 0 && engagePercent < 97)
            {
                float pos = engagePercent / 100 * width;
                GUI.DrawTexture(new Rect(Screen.width / 2 - 250 - swordLeft.width / 2 + pos, Screen.height - 90 - swordLeft.height / 2 + 6, swordLeft.width, swordLeft.height), swordLeft);
                //  print("Engagepercent: " + engagePercent);
            //    GUI.Box(new Rect(left + pos, top, 2, height), new GUIContent(""));
            }

            if (engageFixed)
            {
                float pos = fixedEngagePercent / 100 * width;
               // GUI.Box(new Rect(left + pos, top, 2, height), new GUIContent(""));
                GUI.DrawTexture(new Rect(Screen.width / 2 - 250 - swordLeft.width / 2 + pos, Screen.height - 90 - swordLeft.height / 2 + 6, swordLeft.width, swordLeft.height), swordLeft);
            }

            if (releasePercent > 0)
            {
                float pos = releasePercent / 100 * width;
            //    GUI.Box(new Rect(left + (width - pos), top, 2, height), new GUIContent(""));
                GUI.DrawTexture(new Rect(Screen.width / 2 + 250 - swordRight.width / 2 - pos, Screen.height - 90 - swordRight.height / 2 + 6, swordRight.width, swordRight.height), swordRight);
            }
        }
    }

    public void ResetBar()
    {
        showLinger = true;
        lingerTimer = 0;
    }
}

public class HitAccuracy
{
    // The charge percent when released
    public float Accuracy
    {
        get;
        set;
    }

    // How many enemies were hit with one attack
    public int NumberOfHits
    {
        get;
        set;
    }

    public override string ToString()
    {
        return "Accuracy: " + Accuracy + ", Number of hits: " + NumberOfHits;
    }
}
