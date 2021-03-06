using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GUINavigation))]
public class LevelCompleteScript : MonoBehaviour {

    public GUISkin gSkin;
    public Texture2D backgroundScroll;
    public Vector2 scrollOffset;
    public Texture2D black;
    public EffectVolumeSetter selectSound;

    [HideInInspector]
    public int doubleKills, tripleKills, chargeKills, excellents, averages, goods, perfects;

    [HideInInspector]
    public bool perfectRun = true;

    [HideInInspector]
    public long endScore;

    public int doubleScore, tripleScore, chargeKillScore, excellentScore, averageScore, goodScore, perfectScore, perfectRunScore, distanceScore;

    private Texture2D background;
    private Color activeColor, inactiveColor;
    private bool firstGUI, returned;
    private float countdown,beforenextdecrease;
    private long calculatedScore, nextIncrement;
    private int excellentMultiplier, averageMultiplier, goodMultiplier, perfectMultiplier, chargeKillMultiplier, doubleMultiplier, tripleMultiplier, perfectRunMultiplier;
    private int decreaser, todecrease;
    private bool newtarget, completed;
    private long displayedScore;
    private bool timeout;

    private GUINavigation guin;

    void LevelComplete()
    {
        if (!guin.usingMouse)
            GUI.skin.button.hover.background = null;
        else
            GUI.skin.button.hover.background = background;
        if (guin.activated)
            GUI.skin.button.focused.textColor = activeColor;
        else
            GUI.skin.button.focused.textColor = inactiveColor;

        if (backgroundScroll != null)
        {
            GUI.BeginGroup(new Rect(Screen.width / 2 - backgroundScroll.width / 2 + scrollOffset.x, Screen.height / 2 - backgroundScroll.height / 2 + scrollOffset.y, backgroundScroll.width, backgroundScroll.height));
            GUI.DrawTexture(new Rect(0, 0, backgroundScroll.width, backgroundScroll.height), backgroundScroll);
            GUI.EndGroup();
        }

        GUI.BeginGroup(new Rect(Screen.width / 2 - 395, Screen.height / 2 - 7.5f * 35, 790, 15 * 35));
        GUI.color = new Color(206 / 256f, 206 / 256f, 206f / 256f);
        GUI.SetNextControlName("title");
        GUI.Label(new Rect(60, 0 * 35, 640, 64), "Distance traveled:");
        GUI.Label(new Rect(60, 1 * 35, 640, 64), averages + "x Good kills:");
        GUI.Label(new Rect(60, 2 * 35, 640, 64), goods + "x Great kills:");
        GUI.Label(new Rect(60, 3 * 35, 640, 64), excellents + "x Excellent kills:");
        GUI.Label(new Rect(60, 4 * 35, 640, 64), perfects + "x Perfect kills:");
        GUI.Label(new Rect(60, 5 * 35, 640, 64), doubleKills + "x Double kills:");
        GUI.Label(new Rect(60, 6 * 35, 640, 64), tripleKills + "x Triple kills:");
        GUI.Label(new Rect(60, 7 * 35, 640, 64), chargeKills + "x Destruction bonus:");
        if (!CurrentGameState.InfiniteMode)
            GUI.Label(new Rect(60, 8 * 35, 640, 64), "Perfect run bonus:");
        GUI.Label(new Rect(60, 10 * 35, 640, 64), "Final score:");

        GUI.color = new Color(219f / 256f, 168f / 256f, 1f / 256f);
        GUI.skin.label.alignment = TextAnchor.MiddleRight;
        GUI.Label(new Rect(400, 0 * 35, 250, 64), "" + distanceScore);
        GUI.Label(new Rect(400, 1 * 35, 250, 64), "" + averageMultiplier);
        GUI.Label(new Rect(400, 2 * 35, 250, 64), "" + goodMultiplier);
        GUI.Label(new Rect(400, 3 * 35, 250, 64), "" + excellentMultiplier);
        GUI.Label(new Rect(400, 4 * 35, 250, 64), "" + perfectMultiplier);
        GUI.Label(new Rect(400, 5 * 35, 250, 64), "" + doubleMultiplier);
        GUI.Label(new Rect(400, 6 * 35, 250, 64), "" + tripleMultiplier);
        GUI.Label(new Rect(400, 7 * 35, 250, 64), "" + chargeKillMultiplier);
        if (!CurrentGameState.InfiniteMode)
            GUI.Label(new Rect(400, 8 * 35, 250, 64), "" + perfectRunMultiplier);
        GUI.Label(new Rect(400, 10 * 35, 250, 64), "" + displayedScore);
        GUI.skin.label.alignment = TextAnchor.MiddleLeft;


        GUI.color = Color.white;

        GUI.SetNextControlName("Return");
        if (GUI.Button(new Rect(60, 12 * 35, 640, 64), "Continue"))
            Accept();
        GUI.Box(new Rect(60, 12 * 35, 640, 64), new GUIContent("", "0"));
        guin.mouseover = GUI.tooltip;

        GUI.EndGroup();


        if (returned)
        {
            GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
            GUI.color = new Color(1, 1, 1, Mathf.Lerp(0, 1, 1 - countdown));
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), black);
            GUI.EndGroup();
        }
        GUI.skin.button.hover.background = background;
        GUI.skin.button.focused.textColor = inactiveColor;

        if (!guin.usingMouse)
        {
            if (guin.keySelect == 0)
                GUI.FocusControl("Return");
            else
                GUI.FocusControl("title");
        }
        else
            GUI.FocusControl("title");
    }

	// Use this for initialization
	void Start () {
        guin = GetComponent<GUINavigation>();
        beforenextdecrease = 1.5f;
        todecrease = -1;
        firstGUI = true;
        timeout = false;
        returned = false;
        newtarget = true;
        completed = false;
        nextIncrement = 0;
        endScore = CurrentGameState.currentScore;
        excellentMultiplier = excellents * excellentScore;
        averageMultiplier = averages * averageScore;
        goodMultiplier = goods * goodScore;
        perfectMultiplier = perfects * perfectScore;
        doubleMultiplier = doubleKills * doubleScore;
        tripleMultiplier = tripleKills * tripleScore;
        chargeKillMultiplier = chargeKills * chargeKillScore;
        if (CurrentGameState.InfiniteMode)
            perfectRunMultiplier = 0;
        else
            perfectRunMultiplier = perfectRun ? perfectRunScore * LevelCreator.LEVEL_LENGTH : 0;
        displayedScore = endScore;
        //print("run: " + perfectRun + " score: " + perfectRunScore);
        calculatedScore = endScore + distanceScore + excellentMultiplier + averageMultiplier + goodMultiplier + perfectMultiplier + doubleMultiplier + tripleMultiplier + chargeKillMultiplier + perfectRunMultiplier;
	}
	
	// Update is called once per frame
	void Update () {
        if (returned)
        {
            countdown -= 0.02f;
            if (countdown <= 0)
            {
                CurrentGameState.SetWin();
                CurrentGameState.currentScore = calculatedScore;
                //Debug.Log("End Score: "+ GUIScript.SCORE);
                if (CurrentGameState.InfiniteMode)
                {
                    CurrentGameState.highscorecondition = EndState.Infinite;
                    Application.LoadLevel(3);
                }
                else if (CurrentGameState.locID == 39)
                {

                    CurrentGameState.highscorecondition = EndState.Won;
                    Application.LoadLevel(3);
                }
                else
                    Application.LoadLevel(1);
            }
        }
        else
        {
            if (!completed)
            {
                if (newtarget)
                {
                    if (beforenextdecrease > 0)
                        beforenextdecrease -= Time.deltaTime;
                    else
                    {
                        newtarget = false;
                        beforenextdecrease = 0f;
                        do
                        {
                            todecrease++;
                            switch (todecrease)
                            {
                                case 0: nextIncrement = distanceScore; break;
                                case 1: nextIncrement = averageMultiplier; break;
                                case 2: nextIncrement = goodMultiplier; break;
                                case 3: nextIncrement = excellentMultiplier; break;
                                case 4: nextIncrement = perfectMultiplier; break;
                                case 5: nextIncrement = doubleMultiplier; break;
                                case 6: nextIncrement = tripleMultiplier; break;
                                case 7: nextIncrement = chargeKillMultiplier; break;
                                case 8: nextIncrement = perfectRunMultiplier; break;
                                case 9: completed = true; beforenextdecrease = 5f; return;
                            }
                        }
                        while (nextIncrement == 0);
                        decreaser = (int)(nextIncrement / 23);
                        nextIncrement += displayedScore;
                    }
                }
                else
                {
                    switch (todecrease)
                    {
                        case 0: distanceScore -= decreaser; break;
                        case 1: averageMultiplier -= decreaser; break;
                        case 2: goodMultiplier -= decreaser; break;
                        case 3: excellentMultiplier -= decreaser; break; 
                        case 4: perfectMultiplier -= decreaser; break;
                        case 5: doubleMultiplier -= decreaser; break;
                        case 6: tripleMultiplier -= decreaser; break;
                        case 7: chargeKillMultiplier -= decreaser; break;
                        case 8: perfectRunMultiplier -= decreaser; break;
                    }
                    displayedScore += decreaser;
                    if (displayedScore >= nextIncrement)
                    {
                        displayedScore = nextIncrement;
                        switch (todecrease)
                        {
                            case 0: distanceScore = 0; break;
                            case 1: averageMultiplier = 0; break;
                            case 2: goodMultiplier = 0; break;
                            case 3: excellentMultiplier = 0; break;
                            case 4: perfectMultiplier = 0; break;
                            case 5: doubleMultiplier = 0; break;
                            case 6: tripleMultiplier = 0; break;
                            case 7: chargeKillMultiplier = 0; break;
                            case 8: perfectRunMultiplier = 0; break;
                        }
                        newtarget = true;
                        beforenextdecrease = 0.5f;
                    }
                }
            }
            else
            {
                if (beforenextdecrease > 0)
                    beforenextdecrease -= Time.deltaTime;
                else
                {
                    timeout = true;
                    Accept();
                }
            }
        }
	}

    void OnGUI()
    {
        GUI.skin = gSkin;
        if (firstGUI)
        {
            background = GUI.skin.button.hover.background;
            activeColor = GUI.skin.button.active.textColor;
            inactiveColor = GUI.skin.button.focused.textColor;
            firstGUI = false;
        }
        if (guin.QuitPressed())
        {
            Accept();
        }
        else
            LevelComplete();
    }

    public void Accept()
    {
        if (!returned)
        {
            if (!timeout)
            {
                selectSound.Play();
                guin.SetNoPlay();
            }
            displayedScore = calculatedScore;
            distanceScore = averageMultiplier = goodMultiplier = excellentMultiplier = perfectMultiplier = doubleMultiplier = tripleMultiplier = chargeKillMultiplier = perfectRunMultiplier = 0;
            returned = true;
            countdown = 1f;
        }
    }
}
