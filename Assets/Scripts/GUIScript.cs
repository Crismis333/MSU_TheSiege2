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
    public Texture2D black, sideBar, progressBackground, progressForeground, campIcon, rageOrbBackground, rageOrbForeground, rageFull, rageEmpty, rageShadow;
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

	private float currentZ, maxZ, minZ = 0;

    private float soldierAnim;
    private float countdown;

    private string hitFeedback = "";

    private float scoreTimer = 0.3f;
    private float maxScore, prevScore = 0;

    public static bool PERFECT_RUN = true;
    public static float Multiplier = 1;
    public static int DIFFICULTY_INCREASE = 1;

    public static float MAX_TIMER = 15;
    public static float INF_TIMER = 0;

    public MusicVolumeSetter music;

    public bool started;
    private float screencountdown;
    private bool lost;
    private float prescreencountdown;
    private LevelCompleteScript lcs;
    private GUINavigation guin;

    private float yscale, score;
    private int scoreAdded;
    HeroMovement hm;

	// Use this for initialization
	void Start () {
        int moduleCount = (int)LevelCreator.LengthConverter(LevelCreator.LEVEL_LENGTH);
        maxScore = moduleCount * 5000;

        maxZ = (moduleCount * 64) - 32;
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
        countdown = 0;
        
        if (ObstacleController.PLAYER != null)
            hm = ObstacleController.PLAYER.GetComponent<HeroMovement>();
	}
	
	// Update is called once per frame
	void Update () {
        if (hm == null)
            hm = ObstacleController.PLAYER.GetComponent<HeroMovement>();

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
                    if (CurrentGameState.InfiniteMode)
                        CompleteLevel();
                    else
                        Application.LoadLevel(4);
                }
            }
        }

        if (!PERFECT_RUN && lcs.perfectRun)
        {
            lcs.perfectRun = false;
        }

        soldierAnim += Time.deltaTime;

        if (soldierAnim > 1)
            soldierAnim -= 1;

		currentZ = ObstacleController.PLAYER.transform.position.z;

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

    void FixedUpdate()
    {
        if (scoreTimer <= 0.0f)
        {
            if (CurrentGameState.InfiniteMode)
                score += 267;
            else
                score = (currentZ / maxZ) * maxScore;

            //scoreAdded = (int)(score - prevScore); //remove this to remove text popup

            if (CurrentGameState.InfiniteMode)
                SCORE += 267;
            else
                SCORE += (int)(score - prevScore);
            scoreTimer = 0.1f;
            //hitFeedback = " "; //remove this too

            prevScore = score;
        }

        scoreTimer -= Time.deltaTime;
    }

    string TrimFloat(float f)
    {
        return f.ToString(".0#");
    }

    void Level_Interface()
    {
        GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));

        yscale = Screen.height / 1080f;
        if (sideBar != null)
        {
            GUI.DrawTexture(new Rect(Screen.width - sideBar.width*yscale,0,sideBar.width*yscale,sideBar.height*yscale), sideBar);
        }

        if (progressBackground != null)
        {
            GUI.DrawTexture(new Rect(Screen.width - progressBackground.width * yscale, 166 * yscale, progressBackground.width * yscale, progressBackground.height * yscale), progressBackground);
        }

        if (campIcon != null && !LevelCreator.INF_MODE)
        {
            GUI.DrawTexture(new Rect(Screen.width - campIcon.width * yscale, 116* yscale, campIcon.width * yscale, campIcon.height * yscale),campIcon);
        }
        else if (LevelCreator.INF_MODE)
        {
            GUI.color = new Color(219f / 256f, 168f / 256f, 1f / 256f);
            gSkin.label.fontSize = (int)(20 * yscale);
            String s = ""+DIFFICULTY_INCREASE;
            GUI.skin.label.alignment = TextAnchor.MiddleLeft;
            GUI.Label(new Rect(Screen.width - (20+15*s.Length) * yscale, 100 * yscale, 100 * yscale, 100 * yscale), s);
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUI.color = Color.white;
            // Draw label with text: DIFFICULTY_INCREASE
        }

        if (rageFull != null)
        {
            Rect fullRect = new Rect(Screen.width - rageFull.width * yscale, Screen.height - rageFull.height * yscale, rageFull.width * yscale, rageFull.height * yscale);
            GUI.DrawTexture(fullRect, rageFull);
        }

        if (rageEmpty != null)
        {
            int bottomPadding = 16; // Magic number
            int orbHeight = 178; // Magic number
            float per = hm.Rage;
            float percent = 1 - (bottomPadding + orbHeight * per) / 256.0f;
            Rect clip = new Rect(Screen.width - rageEmpty.width * yscale, Screen.height - rageEmpty.height * yscale, rageEmpty.width * yscale, rageEmpty.height * yscale * percent);
            GUI.BeginGroup(clip);
            GUI.DrawTexture(new Rect(0, 0, clip.width, rageEmpty.height * yscale), rageEmpty);
            GUI.EndGroup();
        }

        if (rageShadow != null)
        {
            Rect fullRect = new Rect(Screen.width - rageShadow.width * yscale, Screen.height - rageShadow.height * yscale, rageShadow.width * yscale, rageShadow.height * yscale);
            GUI.DrawTexture(fullRect, rageShadow);
               
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

        gSkin.label.fontSize = (int)(30f*yscale);
        GUI.color = new Color(219f / 256f, 168f / 256f, 1f / 256f);
        GUI.Label(new Rect(Screen.width - (225 + 15 + 60) * yscale, (15 - 5) * yscale, 250 * yscale, 75 * yscale), ""+SCORE);
        GUI.color = Color.white;

        gSkin.label.fontSize = 24;

        float barHeight = 632f; // Magic number...

        GUI.EndGroup();
        
        Rect progRect = new Rect(Screen.width - progressForeground.width * yscale, 166 * yscale, progressForeground.width * yscale, progressForeground.height * yscale);
        GUI.BeginGroup(progRect);

        float p = currentZ / (maxZ - minZ);
        if (LevelCreator.INF_MODE)
        {
            p = 1 - (INF_TIMER / MAX_TIMER);
        }

        GUI.DrawTexture(new Rect(0, (int) (barHeight * yscale * (1-p)), progressForeground.width * yscale, (int)( progressForeground.height * yscale * p)), progressForeground);
        GUI.DrawTexture(new Rect(0, (int)(barHeight * yscale * 0.99f + 1), progressForeground.width * yscale, (int)(progressForeground.height * yscale * 0.01f)), progressForeground);

        GUI.EndGroup();

        if (started)
        {
            GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
            GUI.color = new Color(1, 1, 1, Mathf.Lerp(1, 0, 1 - screencountdown));
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), black);
            music.volume = Mathf.Lerp(0, OptionsValues.musicVolume, 1 - countdown);
            GUI.EndGroup();
        }
        else if (lost && !CurrentGameState.InfiniteMode)
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
            return 10;
        if (hitRate > 0.95)
            return 9;
        if (hitRate > 0.90)
            return 8;
        if (hitRate > 0.80)
            return 7;
        if (hitRate > 0.70)
            return 6;
        if (hitRate > 0.60)
            return 5;
        if (hitRate > 0.50)
            return 4;
        if (hitRate > 0.40)
            return 3;
        if (hitRate > 0.30)
            return 2;
        if (hitRate > 0)
            return 1;
        return (int)hitRate;
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
        scoreAdded = 0;
        if (hit.Accuracy > HeroAttack.MIN_CHARGE)
        {
            // Calculate score increase
            int baseScore;
            switch (ConvertHitRate(hit.Accuracy)) {
                case 4: case 5: baseScore = lcs.averageScore; break;
                case 6: case 7: baseScore = lcs.goodScore; break;
                case 8: case 9: baseScore = lcs.excellentScore; break;
                case 10: baseScore = lcs.perfectScore; break;
                default: baseScore = 0; break;
            }
            if (baseScore > 0)
            {
                baseScore *= hit.NumberOfHits;
                if (hit.NumberOfHits == 2)
                    baseScore += lcs.doubleScore;
                else if (hit.NumberOfHits == 3)
                    baseScore += lcs.tripleScore;
                SCORE += baseScore;
                scoreAdded = baseScore;
            }
        }
        AttackFeedback(hit);
    }

    public void AttackFeedback(HitAccuracy ha)
    {
        int accuracy = ConvertHitRate(ha.Accuracy);
        
        switch (accuracy)
        {
            case -2: hitFeedback = "Fail"; break;
            case -1: 
            case 0: hitFeedback = "Miss"; break;
            case 1:
            case 2:
            case 3: hitFeedback = "Bad"; break;
            case 4:
            case 5: hitFeedback = "Good"; lcs.averages+= ha.NumberOfHits; break;
            case 6:
            case 7: hitFeedback = "Great"; lcs.goods+= ha.NumberOfHits; break;
            case 8:
            case 9: hitFeedback = "Excellent"; lcs.excellents+= ha.NumberOfHits; break;
            case 10: hitFeedback = "Perfect"; lcs.perfects+= ha.NumberOfHits; break;
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
        if (!started && !lost && (guin.QuitPressed() || guin.usedMenu))
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
                if (scoreAdded != 0)
                {
                    dp = o.AddComponent<DisappearingTextScript>();
                    dp.text = "+" + scoreAdded;
                    dp.gSkin = gSkin;
                    dp.scoreText = true;
                    dp.x = Screen.width - (int)(230*yscale);
                    dp.y = (int)(250 * yscale);
                }
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
        if (CurrentGameState.InfiniteMode)
            lcs.distanceScore = (int)score;
        else
            lcs.distanceScore = (int)(maxScore);

        this.enabled = false;
        lcs.enabled = true;
        guin.ClearElements();
        guin.maxKeys = 1;
        guin.menuKey = 0;
        guin.AddElement(0, lcs.Accept);

    }

    void EngageReleaseBar()
    {
        handleEngagement();
    }

    void handleEngagement()
    {
        if (BarActive)
        {
            float yscale = Screen.height / 1080f;
            if (chargeBarBackground != null)
            {
                GUI.DrawTexture(new Rect(Screen.width / 2 - chargeBarBackground.width * yscale / 2, Screen.height - chargeBarBackground.height * yscale, chargeBarBackground.width * yscale, chargeBarBackground.height * yscale), chargeBarBackground);
            }

            if (chargeBarForeground != null)
            {
                float epsilon = 40.0f;
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
