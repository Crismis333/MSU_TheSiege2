using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GUINavigation))]
public class DeathScreenScript : MonoBehaviour {

    public GUISkin gSkin;
    public Texture2D backgroundScroll2;
    public Vector2 scrollOffset2;
    public EffectVolumeSetter selectSound;
    public MusicVolumeSetter music;

    public string[] overrunText, gaveUpText;

    public Texture2D black;
    private long score;
    private long newscore;
    private float countdown, aftercountdown;
    private bool returned, gaveup, decreaseComplete, started;
    private long pointsDecreaser;

    private Texture2D background;
    private Color activeColor, inactiveColor;
    private bool firstGUI;
    private int textselect;

    private GUINavigation guin;

    void Menu_HighScore()
    {
        if (!guin.usingMouse)
            GUI.skin.button.hover.background = null;
        else
            GUI.skin.button.hover.background = background;

        if (guin.activated)
            GUI.skin.button.focused.textColor = activeColor;
        else
            GUI.skin.button.focused.textColor = inactiveColor;
        if (backgroundScroll2 != null)
        {
            GUI.BeginGroup(new Rect(Screen.width / 2 - backgroundScroll2.width / 2 + scrollOffset2.x, Screen.height / 2 - backgroundScroll2.height / 2 + scrollOffset2.y, backgroundScroll2.width, backgroundScroll2.height));
            GUI.DrawTexture(new Rect(0, 0, backgroundScroll2.width, backgroundScroll2.height), backgroundScroll2);
            GUI.EndGroup();
        }
        GUI.BeginGroup(new Rect(Screen.width / 2 - 395, Screen.height / 2 - 5 * 35, 790, 10 * 35));
        //GUI.Box(new Rect(0, 0, 790, 15 * 35), "");
        GUI.color = new Color(206 / 256f, 206 / 256f, 206f / 256f);
        GUI.SetNextControlName("title");
        TextAnchor t = GUI.skin.label.alignment;
        GUI.skin.label.alignment = TextAnchor.UpperLeft;
        if (CurrentGameState.highscorecondition == EndState.GaveUp)
            GUI.Label(new Rect(0, 0, 790, 790), gaveUpText[textselect]);
        else
            GUI.Label(new Rect(0, 0, 790, 790), overrunText[textselect]);
        GUI.skin.label.alignment = t;

        GUI.Label(new Rect(0, 4 * 35, 790, 64), "Current score      ");
        GUI.color = new Color(219f / 256f, 168f / 256f, 1f / 256f);
        GUI.Label(new Rect(0, 4 * 35, 790, 64), "                      " + score);
        GUI.color = Color.white;
        GUI.SetNextControlName("Return");
        if (GUI.Button(new Rect(60, 6 * 35, 700, 64), "Return to camp (cut points)")) 
            ReturnToCamp();
        GUI.SetNextControlName("GiveUp");
        if (GUI.Button(new Rect(60, 8 * 35, 700, 64), "Give up"))
            GiveUp();

        GUI.Box(new Rect(60, 6 * 35, 700, 64), new GUIContent("", "0"));
        GUI.Box(new Rect(60, 8 * 35, 700, 64), new GUIContent("", "1"));
        guin.mouseover = GUI.tooltip;

        GUI.EndGroup();
        if (started)
        {
            GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
            GUI.color = new Color(1, 1, 1, Mathf.Lerp(1, 0, 1 - countdown));
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), black);
            music.volume = Mathf.Lerp(0, OptionsValues.musicVolume, 1 - countdown);
            GUI.EndGroup();
        }
        else if (gaveup || returned)
        {
            GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
            GUI.color = new Color(1, 1, 1, Mathf.Lerp(0, 1, 1 - countdown));
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), black);
            music.volume = Mathf.Lerp(OptionsValues.musicVolume, 0, 1 - countdown);
            GUI.EndGroup();
        }
        else
            music.volume = OptionsValues.musicVolume;

        GUI.skin.button.hover.background = background;
        GUI.skin.button.focused.textColor = inactiveColor;

        if (!guin.usingMouse)
        {
            switch (guin.keySelect)
            {
                case 0: GUI.FocusControl("Return"); break;
                case 1: GUI.FocusControl("GiveUp"); break;
                default: GUI.FocusControl("title"); break;
            }
        }
        else
            GUI.FocusControl("title");
    }

    void Start()
    {

        if (CurrentGameState.highscorecondition == EndState.GaveUp)
            textselect = Random.Range(0, gaveUpText.Length);
        else
            textselect = Random.Range(0, overrunText.Length);
        firstGUI = true;
        started = true;
        countdown = 1.2f;
        aftercountdown = 1.0f;
        returned = false;
        gaveup = false;
        decreaseComplete = false;
        music.useGlobal = false;

        score = CurrentGameState.currentScore;
        newscore = score / 2;
        pointsDecreaser = newscore / 59L;
        guin = GetComponent<GUINavigation>();
        
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
            guin.ClearElements();
            guin.maxKeys = 2;
            guin.AddElement(0, ReturnToCamp);
            guin.AddElement(1, GiveUp);
        }
        Menu_HighScore();
    }

    public void GiveUp()
    {
        if (!started && !gaveup && !returned)
        {
            selectSound.Play();
            guin.SetNoPlay();
            gaveup = true;
            countdown = 1.2f;
        }
    }

    public void ReturnToCamp()
    {
        if (!started && !gaveup && !returned)
        {
            selectSound.Play();
            guin.SetNoPlay();
            returned = true;
            countdown = 2f;
        }
    }

    void Update()
    {
        if (started)
        {
            countdown -= Time.deltaTime;
            if (countdown <= 0)
                started = false;
        }
        if (returned) {
            if (!decreaseComplete)
            {
                score -= pointsDecreaser;
                if (score <= newscore)
                {
                    score = newscore;
                    decreaseComplete = true;
                }
            }
            else
            {
                countdown -=Time.deltaTime/2;
                if (countdown <= 0)
                {
                    aftercountdown -= Time.deltaTime;
                    if (aftercountdown < 0)
                    {
                        CurrentGameState.previousPosition = CurrentGameState.previousPreviousPosition;
                        CurrentGameState.currentScore = newscore;
                        CurrentGameState.failedlast = true;
                        Application.LoadLevel(1);
                    }
                }
            }
        }
        else if (gaveup)
        {
            countdown -= Time.deltaTime/2;
            if (countdown <= 0)
            {
                aftercountdown -= Time.deltaTime;
                if (aftercountdown < 0)
                {
                    CurrentGameState.highscorecondition = EndState.Lost;
                    Application.LoadLevel(3);
                }
            }
        }
    }
}
