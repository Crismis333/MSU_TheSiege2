using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GUIScript : MonoBehaviour {
	
	public static long SCORE;
	
    public GUISkin gSkin;
    public Texture2D runningSoldiers1, runningSoldiers2, runningHero1, runningHero2, runningGoal, damagebar, swordLeft, swordRight, bloodsplatter;
    public Texture2D backgroundScrollScore, backgroundScrollLeft, backgroundScrollMid, backgroundScrollRight, black;
    public Vector2 scrollScoreOffset, scrollLeftOffset, scrollMidOffset, scrollRightOffset;
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
	private float minZ = -50;
	private float maxZ;

    private float soldierAnim;
    private float countdown;

	// Use this for initialization
	void Start () {
		maxZ = (LevelCreator.LengthConverter(LevelCreator.LEVEL_LENGTH)*64)-32;
        HitList = new List<HitAccuracy>();
        soldierAnim = 0;
	}
	
	// Update is called once per frame
	void Update () {
        soldierAnim += Time.deltaTime;

        if (soldierAnim > 1)
            soldierAnim -= 1;

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
        if (backgroundScrollScore != null)
        {
            GUI.DrawTexture(new Rect(Screen.width - 225 + scrollScoreOffset.x, 25 + scrollScoreOffset.y, backgroundScrollScore.width, backgroundScrollScore.height), backgroundScrollScore);
        }
        //GUI.Box(new Rect(Screen.width-225, 25, 200, 75), "");
        GUI.color = Color.black;
        GUI.Label(new Rect(Screen.width - 225 + 15, 25 + 15-5, 200, 75), "Score:   "+ SCORE);
        GUI.Label(new Rect(Screen.width - 225 + 15, 25 + 15*2, 200, 75), "Multiplier:   x2");
        GUI.color = Color.white;
        if (backgroundScrollLeft != null)
        {
            GUI.DrawTexture(new Rect(15 + scrollLeftOffset.x, Screen.height - 65 + scrollLeftOffset.y, backgroundScrollLeft.width, backgroundScrollLeft.height), backgroundScrollLeft);
        }
        if (backgroundScrollRight != null)
        {
            GUI.DrawTexture(new Rect(Screen.width - 30 + scrollRightOffset.x, Screen.height - 65 + scrollRightOffset.y, backgroundScrollRight.width, backgroundScrollRight.height), backgroundScrollRight);
        }
        if (backgroundScrollMid != null)
        {
            GUI.DrawTexture(new Rect(15 + scrollMidOffset.x, Screen.height - 65 + scrollMidOffset.y, Screen.width - 30 - scrollMidOffset.x*2+1, backgroundScrollMid.height), backgroundScrollMid);
        }

        GUI.DrawTexture(new Rect(Screen.width / 2 - 250, Screen.height - 90, 500, 12), damagebar);
        //   GUI.DrawTexture(new Rect(Screen.width / 2 - 250 - swordLeft.width / 2, Screen.height - 90 - swordLeft.height / 2 + 6, swordLeft.width, swordLeft.height), swordLeft);
        //    GUI.DrawTexture(new Rect(Screen.width / 2 + 250 - swordRight.width / 2, Screen.height - 90 - swordRight.height / 2 + 6, swordRight.width, swordRight.height), swordRight);
        //GUI.DrawTexture(new Rect(Screen.width / 2 - bloodsplatter.width / 2, Screen.height - 90 - bloodsplatter.height / 2 + 6, bloodsplatter.width, bloodsplatter.height), bloodsplatter);
        GUI.EndGroup();

        GUI.BeginGroup(new Rect(15 + scrollMidOffset.x, Screen.height - 64 - 6, Screen.width - 30 - scrollMidOffset.x * 2 + 1, Screen.height));
        //GUI.Box(new Rect(15, Screen.height - 65, Screen.width - 30, 50), "");
        GUI.DrawTexture(new Rect(currentZ / (maxZ - minZ) * (Screen.width - 30 - scrollMidOffset.x * 2), 0, 128, 64), runningSoldiers1);
       
        if (soldierAnim > 0.5)
            GUI.DrawTexture(new Rect(armyZ / (maxZ - minZ) * (Screen.width - 30 - scrollMidOffset.x * 2), 0, 128, 64), runningSoldiers1);
        else
            GUI.DrawTexture(new Rect(armyZ / (maxZ - minZ) * (Screen.width - 30 - scrollMidOffset.x * 2), 0, 128, 64), runningSoldiers2);
        //GUI.DrawTexture(new Rect(Screen.width - 30 - scrollMidOffset.x * 2 + 1 - 128, 0, 128, 64), runningSoldiers1);

        GUI.EndGroup();
    }

    private int ConvertHitRate(float hitRate)
    {
        if (hitRate > 0.98)
        {
            return 10;
        }
        if (hitRate > 0.95)
        {
            return 9;
        }
        if (hitRate > 0.90)
        {
            return 8;
        }
        if (hitRate > 0.80)
        {
            return 7;
        }
        if (hitRate > 0.70)
        {
            return 6;
        }
        if (hitRate > 0.60)
        {
            return 5;
        }
        if (hitRate > 0.50)
        {
            return 4;
        }
        if (hitRate > 0.40)
        {
            return 3;
        }
        if (hitRate > 0.30)
        {
            return 2;
        }
        return 1;
    }

    public void AddHit(HitAccuracy hit)
    {
        HitList.Add(hit);
        float eff = CalculateEfficiency();
        print("Efficiency: " + eff);
    }

    float CalculateEfficiency()
    {
        int size = 5;
        size = Math.Min(size, HitList.Count);

        int total = 0;

        for (int i = 0; i < size; i++)
        {
            int rate = ConvertHitRate(HitList[HitList.Count - 1 - i].Accuracy);
            total += rate;
        }

        return (float)total / size;
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

            //GUI.Label(new Rect(10, 50, 80, 30), "" + Time.timeSinceLevelLoad);
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
            if (engagePercent > 0 && engagePercent < 110.1)
            {
                float pos = engagePercent / 95 * width / 2;
                GUI.DrawTexture(new Rect(Screen.width / 2 - 250 - swordLeft.width / 2 + pos, Screen.height - 90 - swordLeft.height / 2 + 6, swordLeft.width, swordLeft.height), swordLeft);
                GUI.DrawTexture(new Rect(Screen.width / 2 + 250 - swordLeft.width / 2 - pos, Screen.height - 90 - swordLeft.height / 2 + 6, swordLeft.width, swordLeft.height), swordRight);
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
