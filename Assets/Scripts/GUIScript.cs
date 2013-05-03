using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(GUINavigation))]
[RequireComponent(typeof(PauseMenuScript))]
[RequireComponent(typeof(LevelCompleteScript))]
public class GUIScript : MonoBehaviour {
	
	public static long SCORE;
	
    public GUISkin gSkin;
    public Texture2D runningSoldiers1, runningSoldiers2, runningHero1, runningHero2, runningGoal, damagebar, swordLeft, swordRight;
    public Texture2D backgroundScrollScore, backgroundScrollLeft, backgroundScrollMid, backgroundScrollRight, black;
    public Texture2D sideBar, progressBackground, progressForeground, campIcon, rageOrbBackground, rageOrbForeground, rageFull, rageEmpty, rageShadow;
    public Texture2D slowDownBackground, slowDownForeground, chargeBarBackground, chargeBarForeground;

    public Vector2 scrollScoreOffset, scrollLeftOffset, scrollMidOffset, scrollRightOffset;
    public float engagePercent, lastEngagePercent, releasePercent;
    public float fixedEngagePercent;
    public bool engageFixed, releaseFixed;
    public bool BarActive;
    public float ShowLingerTime = 1.0f;
    public List<HitAccuracy> HitList;

    private float lingerTimer;
    private bool showLinger;

	private float currentZ;
	private float armyZ;
	//private float minZ = -50;
    private float minZ = 0;
	private float maxZ;

    private float soldierAnim;
    private float countdown;

    private float hitFeedbackTimer = 0;
    private string hitFeedback = "";

    public static bool PERFECT_RUN = true;
    public static float Multiplier = 1;
    public MusicVolumeSetter music;

    public bool started;
    private float screencountdown;
    private bool lost;
    private float prescreencountdown;
    private LevelCompleteScript lcs;
    private GUINavigation guin;

    private float yscale;

    HeroMovement hm;

	// Use this for initialization
	void Start () {
		maxZ = (LevelCreator.LengthConverter(LevelCreator.LEVEL_LENGTH)*64)-32;
        HitList = new List<HitAccuracy>();
        soldierAnim = 0;
        Screen.lockCursor = false;
        music.useGlobal = false;
        started = true;
        screencountdown = 1f;
        PERFECT_RUN = true;
        guin = GetComponent<GUINavigation>();
        guin.maxKeys = 0;
        lcs = GetComponent<LevelCompleteScript>();
        lost = false;
        prescreencountdown = 0f;
        hm = ObstacleController.PLAYER.GetComponent<HeroMovement>();
	}
	
	// Update is called once per frame
	void Update () {
        if (started)
        {
            screencountdown -= Time.deltaTime;
            if (screencountdown < 0)
            {
                screencountdown = 0;
                started = false;
            }
        }
        else if (lost)
        {
            prescreencountdown -= Time.deltaTime;
            if (prescreencountdown <= 0)
            {
                screencountdown -= Time.deltaTime;
                if (screencountdown < 0)
                {
                    screencountdown = 0;
                    CurrentGameState.highscorecondition = EndState.Lost;
                    Application.LoadLevel(4);
                }
            }
        }

        if (Input.GetKey(KeyCode.Z))
        {
            GameObject o = new GameObject();
            DisappearingTextScript dp = o.AddComponent<DisappearingTextScript>();
            dp.text = "Perfect!";
            dp.gSkin = gSkin;
            dp.x = Screen.width / 2;
            dp.y = 200;

            o = new GameObject();
            dp = o.AddComponent<DisappearingTextScript>();
            dp.text = "+5000";
            dp.gSkin = gSkin;
            dp.x = Screen.width -300;
            dp.y = 300;
        }

        if (!PERFECT_RUN && lcs.perfectRun)
        {
            lcs.perfectRun = false;
        }

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

        hitFeedbackTimer = Mathf.Max(hitFeedbackTimer - Time.deltaTime, 0);
	}

    string TrimFloat(float f)
    {
        return f.ToString(".0#");
    }

    void Level_Interface()
    {
        GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));

        if (backgroundScrollScore != null)
        {
            //GUI.DrawTexture(new Rect(Screen.width - 225 + scrollScoreOffset.x, 25 + scrollScoreOffset.y, backgroundScrollScore.width, backgroundScrollScore.height), backgroundScrollScore);
        }
        //GUI.Box(new Rect(Screen.width-225, 25, 200, 75), "");
        /*
        if (sideBarTop != null)
        {
            GUI.DrawTexture(new Rect(Screen.width - sideBarTop.width, 0, sideBarTop.width, sideBarTop.height), sideBarTop);
        }

        if (sideBarBot != null)
        {
            GUI.DrawTexture(new Rect(Screen.width - sideBarBot.width, Screen.height - sideBarBot.height, sideBarBot.width, sideBarBot.height), sideBarBot);
        }

        if (sideBarMid != null)
        {
            GUI.DrawTexture(new Rect(Screen.width - sideBarMid.width, 233, sideBarMid.width, Screen.height - 233 - 344), sideBarMid);
        }
         */
        yscale = Screen.height / 1080f;
        if (sideBar != null)
        {
            GUI.DrawTexture(new Rect(Screen.width - sideBar.width*yscale,0,sideBar.width*yscale,sideBar.height*yscale), sideBar);
        }

        if (progressBackground != null)
        {
            GUI.DrawTexture(new Rect(Screen.width - progressBackground.width * yscale, 166 * yscale, progressBackground.width * yscale, progressBackground.height * yscale), progressBackground);
        }

        if (progressForeground != null)
        {
            //Rect progRect = new Rect(Screen.width - progressForeground.width * yscale, 166 * yscale, progressForeground.width * yscale, progressForeground.height * yscale);
            //print(progRect);
            //Rect texRect = new Rect(0,1, 1.0f, 0.39f);
            //GUI.DrawTextureWithTexCoords(progRect, progressForeground, texRect);
         //   GUI.DrawTexture(new Rect(Screen.width - progressForeground.width * yscale, 166 * yscale, progressForeground.width * yscale, progressForeground.height * yscale), progressForeground);
        }

        if (campIcon != null)
        {
            GUI.DrawTexture(new Rect(Screen.width - campIcon.width * yscale, 116* yscale, campIcon.width * yscale, campIcon.height * yscale),campIcon);
        }

        if (false)
        {
            if (rageOrbBackground != null)
            {
                int bottomPadding = 16; // Magic number
                int orbHeight = 178; // Magic number
                int topPadding = 256 - bottomPadding - orbHeight; // Magic number
                float per = hm.Rage;
                float percent = (topPadding + orbHeight * per) / 256.0f;
                Rect clip = new Rect(Screen.width - rageOrbBackground.width * yscale, Screen.height - rageOrbBackground.height * yscale, rageOrbBackground.width * yscale, rageOrbBackground.height * yscale * percent);
                //    clip = new Rect(Screen.width - rageOrbBackground.width * yscale, (Screen.height - orbHeight - bottomPadding) * yscale, rageOrbBackground.width * yscale, orbHeight * yscale); 
                GUI.BeginGroup(clip);
                //  GUI.DrawTexture(new Rect(Screen.width - rageOrbBackground.width * yscale, Screen.height - rageOrbBackground.height * yscale, rageOrbBackground.width * yscale, rageOrbBackground.height * yscale), rageOrbBackground);
                GUI.DrawTexture(new Rect(0, 0, clip.width, rageOrbBackground.height * yscale), rageOrbBackground);
                // GUI.DrawTexture(new Rect(0, topPadding,  rageOrbBackground.width * yscale, orbHeight * yscale), rageOrbBackground);
                GUI.EndGroup();
            }

            if (rageOrbForeground != null)
            {
                Rect rageRect = new Rect(Screen.width - rageOrbForeground.width * yscale, Screen.height - rageOrbForeground.height * yscale, rageOrbForeground.width * yscale, rageOrbForeground.height * yscale);
                GUI.DrawTexture(rageRect, rageOrbForeground);
            }
        }
        else
        {
            if (rageFull != null)
            {
                Rect fullRect = new Rect(Screen.width - rageFull.width * yscale, Screen.height - rageFull.height * yscale, rageFull.width * yscale, rageFull.height * yscale);
                GUI.DrawTexture(fullRect, rageFull);
            }

            if (rageEmpty != null)
            {
                int bottomPadding = 16; // Magic number
                int orbHeight = 178; // Magic number
                int topPadding = 256 - bottomPadding - orbHeight; // Magic number
                float per = hm.Rage;
                float percent = 1 - (bottomPadding + orbHeight * per) / 256.0f;
                Rect clip = new Rect(Screen.width - rageEmpty.width * yscale, Screen.height - rageEmpty.height * yscale, rageEmpty.width * yscale, rageEmpty.height * yscale * percent);
                //    clip = new Rect(Screen.width - rageOrbBackground.width * yscale, (Screen.height - orbHeight - bottomPadding) * yscale, rageOrbBackground.width * yscale, orbHeight * yscale); 
                GUI.BeginGroup(clip);
                //  GUI.DrawTexture(new Rect(Screen.width - rageOrbBackground.width * yscale, Screen.height - rageOrbBackground.height * yscale, rageOrbBackground.width * yscale, rageOrbBackground.height * yscale), rageOrbBackground);
                GUI.DrawTexture(new Rect(0, 0, clip.width, rageEmpty.height * yscale), rageEmpty);
                // GUI.DrawTexture(new Rect(0, topPadding,  rageOrbBackground.width * yscale, orbHeight * yscale), rageOrbBackground);
                GUI.EndGroup();
            }

            if (rageShadow != null)
            {
                Rect fullRect = new Rect(Screen.width - rageShadow.width * yscale, Screen.height - rageShadow.height * yscale, rageShadow.width * yscale, rageShadow.height * yscale);
                GUI.DrawTexture(fullRect, rageShadow);
               
            }
        }

        if (slowDownBackground != null)
        {
            GUI.DrawTexture(new Rect(Screen.width - slowDownBackground.width * yscale, 830 * yscale, slowDownBackground.width * yscale, slowDownBackground.height * yscale), slowDownBackground);
        }

        if (slowDownForeground != null)
        {
            if (hm.Slowed)
            {
                Color c = new Color(1, 1, 1, hm.GetSlowed());
                GUI.color = c;
                GUI.DrawTexture(new Rect(Screen.width - slowDownForeground.width * yscale, 830 * yscale, slowDownForeground.width * yscale, slowDownForeground.height * yscale), slowDownForeground);
                c.a = 1;
                GUI.color = c;
            }
        }
/*
        if (chargeBarBackground != null)
        {
            GUI.DrawTexture(new Rect(Screen.width / 2 - chargeBarBackground.width * yscale / 2, Screen.height - chargeBarBackground.height * yscale, chargeBarBackground.width * yscale, chargeBarBackground.height * yscale), chargeBarBackground);
        }

        if (chargeBarForeground != null)
        {
            GUI.DrawTexture(new Rect(Screen.width / 2 - chargeBarForeground.width * yscale / 2, Screen.height - chargeBarForeground.height * yscale, chargeBarForeground.width * yscale, chargeBarForeground.height * yscale), chargeBarForeground);
        }
        */
        gSkin.label.fontSize = (int)(30f*yscale);
        GUI.color = new Color(219f / 256f, 168f / 256f, 1f / 256f);
        GUI.Label(new Rect(Screen.width - (225 + 15 + 60) * yscale, (15 - 5) * yscale, 250 * yscale, 75 * yscale), "99999999");
        GUI.color = Color.white;

        gSkin.label.fontSize = 24;
        /*
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

        //   GUI.DrawTexture(new Rect(Screen.width / 2 - 250 - swordLeft.width / 2, Screen.height - 90 - swordLeft.height / 2 + 6, swordLeft.width, swordLeft.height), swordLeft);
        //    GUI.DrawTexture(new Rect(Screen.width / 2 + 250 - swordRight.width / 2, Screen.height - 90 - swordRight.height / 2 + 6, swordRight.width, swordRight.height), swordRight);
        //GUI.DrawTexture(new Rect(Screen.width / 2 - bloodsplatter.width / 2, Screen.height - 90 - bloodsplatter.height / 2 + 6, bloodsplatter.width, bloodsplatter.height), bloodsplatter);
        */

        float barHeight = 632f; // Magic number...

        GUI.EndGroup();
        
        Rect progRect = new Rect(Screen.width - progressForeground.width * yscale, 166 * yscale, progressForeground.width * yscale, progressForeground.height * yscale);
        GUI.BeginGroup(progRect);
     //   Rect progRect = new Rect(Screen.width - progressForeground.width * yscale, 166 * yscale, progressForeground.width * yscale, progressForeground.height * yscale);
        
        Rect texRect = new Rect(0, 1, 1.0f, 0.39f);
     //   GUI.DrawTextureWithTexCoords(progRect, progressForeground, texRect);
        float p = currentZ / (maxZ - minZ);

        GUI.DrawTexture(new Rect(0, (int) (barHeight * yscale * (1-p)), progressForeground.width * yscale, (int)( progressForeground.height * yscale * p)), progressForeground);
   //     GUI.DrawTexture(new Rect(0, barPos, progressForeground.width * yscale, height), progressForeground);

        GUI.DrawTexture(new Rect(0, (int)(barHeight * yscale * 0.99f + 1), progressForeground.width * yscale, (int)(progressForeground.height * yscale * 0.01f)), progressForeground);

        GUI.EndGroup();


        if (runningSoldiers1 != null && runningSoldiers2 != null)
        {
            GUI.BeginGroup(new Rect(15 + scrollMidOffset.x, Screen.height - 64 - 6, Screen.width - 30 - scrollMidOffset.x * 2 + 1, Screen.height));
            //GUI.Box(new Rect(15, Screen.height - 65, Screen.width - 30, 50), "");
            /*
            GUI.DrawTexture(new Rect(Screen.width - 30 - scrollMidOffset.x * 2 - 64, 0, 64, 64), runningGoal);
            if (soldierAnim > 0.5)
                GUI.DrawTexture(new Rect(currentZ / (maxZ - minZ) * (Screen.width - 30 - scrollMidOffset.x * 2), 0, 128, 64), runningHero1);
            else
                GUI.DrawTexture(new Rect(currentZ / (maxZ - minZ) * (Screen.width- 30 - scrollMidOffset.x * 2), 0, 128, 64), runningHero2);
            if (soldierAnim > 0.5)
                GUI.DrawTexture(new Rect(armyZ / (maxZ - minZ) * (Screen.width - 30 - scrollMidOffset.x * 2) - 64, 0, 128, 64), runningSoldiers1);
            else
                GUI.DrawTexture(new Rect(armyZ / (maxZ - minZ) * (Screen.width - 30 - scrollMidOffset.x * 2) - 64, 0, 128, 64), runningSoldiers2);
            //GUI.DrawTexture(new Rect(Screen.width - 30 - scrollMidOffset.x * 2 + 1 - 128, 0, 128, 64), runningSoldiers1);
            */
            GUI.EndGroup();
        }

        if (started)
        {
            GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
            GUI.color = new Color(1, 1, 1, Mathf.Lerp(1, 0, 1 - screencountdown));
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), black);
            music.volume = Mathf.Lerp(0, OptionsValues.musicVolume, 1 - countdown);
            GUI.EndGroup();
        }
        else if (lost)
        {
            GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
            GUI.color = new Color(1, 1, 1, Mathf.Lerp(0, 1, 1 - screencountdown));
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), black);
            music.volume = Mathf.Lerp(OptionsValues.musicVolume,0, 1 - countdown);
            GUI.EndGroup();
        }

        music.useGlobal = false;
        music.volume = OptionsValues.musicVolume;
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
        if (hitRate > 0)
        {
            return 1;
        }
        return -1;
    }

    public void LoseLevel()
    {
        if (!lost)
        {
            lost = true;
            prescreencountdown = 0.5f;
            screencountdown = 1f;
        }
    }

    public void AddHit(HitAccuracy hit)
    {
        HitList.Add(hit);
        Multiplier = CalculateEfficiency() / 2;

        if (hit.Accuracy > HeroAttack.MIN_CHARGE)
        {
            // Calculate score increase
            float baseScore = 100;
            float hitMult = 1;
            if (hit.NumberOfHits > 1)
            {
                hitMult = hit.NumberOfHits * 1.5f;
            }
            float score = baseScore * hitMult * hit.CurrentSpeed * Multiplier;
            SCORE += (long)score;
            //print("Score increase: " + (long)score);
        }
        AttackFeedback(hit);
     //   print("Efficiency: " + eff);
    }

    public void AttackFeedback(HitAccuracy ha)
    {
        int accuracy = ConvertHitRate(ha.Accuracy);
        
        switch (accuracy)
        {
            case -1: hitFeedback = "Miss"; break;
            case 1:
            case 2:
            case 3: hitFeedback = "Bad"; break;
            case 4:
            case 5: hitFeedback = "Average"; lcs.averages++; break;
            case 6:
            case 7: hitFeedback = "Good"; lcs.goods++; break;
            case 8:
            case 9: hitFeedback = "Excellent"; lcs.excellents++; break;
            case 10: hitFeedback = "Perfect"; lcs.perfects++; break;
        }
        if (ha.NumberOfHits == 2)
        {
            hitFeedback += " Double Kill";
            lcs.doubleKills++;
        }
        else if (ha.NumberOfHits == 3)
        {
            hitFeedback += " Triple Kill";
            lcs.tripleKills++;
        }
        hitFeedback += "!";
        hitFeedbackTimer = 2.0f;
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
        if (!started && !lost && (Input.GetKeyDown(KeyCode.Escape) || guin.usedMenu))
        {
            music.useGlobal = true;
            this.enabled = false;
            Time.timeScale = 0;
            PauseMenuScript pms = GetComponent<PauseMenuScript>();
            pms.enabled = true;
            guin.ClearElements();
            guin.maxKeys = 5;
            guin.menuKey = 4;
            guin.AddElement(0, pms.Pause_Options);
            guin.AddElement(1, pms.Pause_Controls);
            guin.AddElement(2, pms.Pause_GiveUp);
            guin.AddElement(3, pms.Pause_Quit);
            guin.AddElement(4, pms.Pause_Back);
        }
        else
        {

            //GUI.Label(new Rect(10, 50, 80, 30), "" + Time.timeSinceLevelLoad);
            GUI.skin = gSkin;
            Level_Interface();

            
            if (!hitFeedback.Equals(""))
            {
                GameObject o = new GameObject();
                DisappearingTextScript dp = o.AddComponent<DisappearingTextScript>();
                dp.text = hitFeedback;
                dp.gSkin = gSkin;
                dp.x = Screen.width / 2;
                dp.y = (int)(400 * yscale);
                hitFeedback = "";
            }
            gSkin.label.alignment = TextAnchor.MiddleCenter;
            gSkin.label.fontSize = 40;
            if (ObstacleController.PLAYER.GetComponent<HeroMovement>().Rage >= 1)
            {
                GUI.color = new Color(0, 0, 0, 1);
                GUI.Label(new Rect(Screen.width / 2 - 301, Screen.height / 2 - 201, 600, 400), "CHARGE!!");
                GUI.Label(new Rect(Screen.width / 2 - 301, Screen.height / 2 - 199, 600, 400), "CHARGE!!");
                GUI.Label(new Rect(Screen.width / 2 - 299, Screen.height / 2 - 201, 600, 400), "CHARGE!!");
                GUI.Label(new Rect(Screen.width / 2 - 299, Screen.height / 2 - 199, 600, 400), "CHARGE!!");

                GUI.color = new Color(200f / 256f, 0, 0, 1);
                GUI.Label(new Rect(Screen.width / 2 - 300, Screen.height / 2 - 200, 600, 400), "CHARGE!!");

                GUI.color = new Color(1, 1, 1, 1);
            }

            gSkin.label.fontSize = 24;
            gSkin.label.alignment = TextAnchor.UpperRight;

            EngageReleaseBar();
        }
    }

    public void CompleteLevel()
    {
        this.enabled = false;
        GetComponent<LevelCompleteScript>().enabled = true;
        GetComponent<GUINavigation>().ClearElements();
        GetComponent<GUINavigation>().maxKeys = 1;
        GetComponent<GUINavigation>().menuKey = 0;
        GetComponent<GUINavigation>().AddElement(0, GetComponent<LevelCompleteScript>().Accept);

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
            float yscale = Screen.height / 1080f;
            if (chargeBarBackground != null)
            {
                GUI.DrawTexture(new Rect(Screen.width / 2 - chargeBarBackground.width * yscale / 2, Screen.height - chargeBarBackground.height * yscale, chargeBarBackground.width * yscale, chargeBarBackground.height * yscale), chargeBarBackground);
            }

            if (chargeBarForeground != null)
            {
                float epsilon = 20.0f;
                if (engagePercent > lastEngagePercent + epsilon || engagePercent < lastEngagePercent - epsilon)
                {
                    engagePercent = lastEngagePercent;
                }

                int right = 252; // Magic number
                int swordlength = 389; // Magic number
                int left = 1024 - right - swordlength;
                float percent = engagePercent > 100 ? 200 - engagePercent : engagePercent;
               
                

                float per = (left + (percent / 100) * swordlength) / 1024;

                Rect r = new Rect(Screen.width / 2 - chargeBarForeground.width * yscale / 2, Screen.height - chargeBarForeground.height * yscale, chargeBarForeground.width * yscale * per, chargeBarForeground.height * yscale);
                GUI.BeginGroup(r);
                GUI.DrawTexture(new Rect(0,0, chargeBarForeground.width * yscale, chargeBarForeground.height * yscale), chargeBarForeground);
                GUI.EndGroup();
                lastEngagePercent = engagePercent;
            }
            /*
            GUI.DrawTexture(new Rect(Screen.width / 2 - 250, Screen.height - 90, 500, 12), damagebar);


            if (engagePercent > 0 && engagePercent < 200)
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
             */
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

    // The speed the hero had when hitting
    public float CurrentSpeed
    {
        get;
        set;
    }

    public override string ToString()
    {
        return "Accuracy: " + Accuracy + ", Number of hits: " + NumberOfHits;
    }
}
