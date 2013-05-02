using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GUINavigation))]
public class PauseReturnToScript : MonoBehaviour {

    public GUISkin gSkin;
    public Texture2D backgroundScroll;
    public Texture2D black;
    public Vector2 scrollOffset;
    public EffectVolumeSetter cancelSound;
    public EffectVolumeSetter selectSound;

    [HideInInspector]
    public bool onMap, quit, restart;
    private float countdown;
    private bool ended;

    private Texture2D background;
    private Color activeColor, inactiveColor;
    private bool firstGUI;

    private GUINavigation guin;
    private PauseMenuScript pms;

    void Return_Accept()
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
        GUI.BeginGroup(new Rect(Screen.width / 2 - 395, Screen.height / 2 - 2.5f * 70, 790, 5 * 70));

        //GUI.Box(new Rect(0, 0, 790, 5 * 70), "");
        GUI.color = Color.black;
        GUI.SetNextControlName("title");
        if (quit)
            GUI.Label(new Rect(0, 1 * 70, 790, 64), "Do you really wish to quit? All progress will be lost.");
        else if (onMap)
            GUI.Label(new Rect(0, 1 * 70, 790, 64), "Give up? All progress will be lost.");
        else
            GUI.Label(new Rect(0, 1 * 70, 790, 64), "Give up on this level?");
        GUI.color = Color.white;
        GUI.SetNextControlName("Yes");
        if (GUI.Button(new Rect(60, 3 * 70, 730, 64), "Yes")) { Return_Yes(); }
        GUI.SetNextControlName("No");
        if (GUI.Button(new Rect(60, 4 * 70, 730, 64), "No")) { Return_No();  }

        GUI.Box(new Rect(60, 3 * 70, 730, 64), new GUIContent("", "0"));
        GUI.Box(new Rect(60, 4 * 70, 730, 64), new GUIContent("", "1"));
        guin.mouseover = GUI.tooltip;

        GUI.EndGroup();
        if (ended)
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
            switch (guin.keySelect)
            {
                case 0: GUI.FocusControl("Yes"); break;
                case 1: GUI.FocusControl("No"); break;
                default: GUI.FocusControl("title"); break;
            }
        }
        else
            GUI.FocusControl("title");
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Return_No();
        }
        else
            Return_Accept();
    }

    public void Return_No()
    {
        if (!ended)
        {
            quit = onMap = restart = false;
            this.enabled = false;
            pms.enabled = true;
            cancelSound.Play();
            guin.ClearElements();
            guin.maxKeys = 5;
            guin.menuKey = 4;
            guin.AddElement(0, pms.Pause_Options);
            guin.AddElement(1, pms.Pause_Controls);
            guin.AddElement(2, pms.Pause_GiveUp);
            guin.AddElement(3, pms.Pause_Quit);
            guin.AddElement(4, pms.Pause_Back);
        }
    }

    public void Return_Yes()
    {
        //this.enabled = false;

        selectSound.Play();
        guin.SetNoPlay();
        
        ended = true;
        countdown = 1.0f;

    }

    void Update()
    {
        if (ended)
        {
            countdown -= 0.02f;
            if (countdown <= 0)
            {
                Time.timeScale = 1;
                //if (restart)
                //{
                //    Application.LoadLevel(Application.loadedLevel);
                //}
                if (quit)
                {
                    CurrentGameState.Restart();
                    Application.Quit();
                }
                else if (onMap)
                {
                    CurrentGameState.highscorecondition = EndState.GaveUp;
                    Application.LoadLevel(3);
                }
                else
                {
                    // Wins, doesn't reset. Change when game is final
                    //CurrentGameState.previousPosition = CurrentGameState.previousPreviousPosition;
                    //CurrentGameState.currentScore = CurrentGameState.previousScore;
                    //CurrentGameState.SetWin();
                    //Application.LoadLevel(1);
                    Application.LoadLevel(4);
                    CurrentGameState.highscorecondition = EndState.GaveUp;
                }
            }
        }
    }

    void Start()
    {
        firstGUI = true;
        guin = GetComponent<GUINavigation>();
        pms = GetComponent<PauseMenuScript>();
    }
}
