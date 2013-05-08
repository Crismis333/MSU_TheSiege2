using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GUINavigation))]
public class PauseReturnToScript : MonoBehaviour {

    public GUISkin gSkin;
    public Texture2D backgroundScroll;
    public Texture2D black;
    public Vector2 scrollOffset;
    public EffectVolumeSetter cancelSound, selectSound, quitSound, startSound;
    public MusicVolumeSetter music;
    public MusicLevelVolumeSetter levelmusic;

    [HideInInspector]
    public bool onMap, quit, restart;
    private float countdown, aftercountdown;
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
        GUI.color = new Color(206 / 256f, 206 / 256f, 206f / 256f);
        GUI.SetNextControlName("title");
        if (quit)
            GUI.Label(new Rect(0, 1 * 70, 790, 64), "Do you really wish to quit? All progress will be lost.");
        else if (onMap)
            GUI.Label(new Rect(0, 1 * 70, 790, 64), "Give up? All progress will be lost.");
        else
            GUI.Label(new Rect(0, 1 * 70, 790, 64), "Give up on this level?");
        GUI.color = Color.white;
        GUI.SetNextControlName("Yes");
        if (GUI.Button(new Rect(60, 3 * 70, 730, 64), "Yes")) 
            Return_Yes();
        GUI.SetNextControlName("No");
        if (GUI.Button(new Rect(60, 4 * 70, 730, 64), "No")) 
            Return_No();

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
        if (guin.QuitPressed())
            Return_No();
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
        guin.SetNoPlay();
        if (quit)
            quitSound.Play();
        else
            startSound.Play();
        ended = true;
        countdown = 1.0f;

    }

    void Update()
    {
        if (ended)
        {
            countdown -= 0.01f;
            if (music != null)
            {
                music.useGlobal = false;
                music.volume = Mathf.Lerp(OptionsValues.musicVolume, 0, 1 - countdown);
            }
            else if (levelmusic != null)
            {
                levelmusic.useGlobal = false;
                levelmusic.volume = Mathf.Lerp(OptionsValues.musicVolume, 0, 1 - countdown);
            }
            if (countdown <= 0)
            {
                aftercountdown -= 0.02f;
                if (aftercountdown <= 0)
                {
                    Time.timeScale = 1;
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
                        if (CurrentGameState.InfiniteMode)
                        {
                            CurrentGameState.Restart();
                            Application.LoadLevel(0);
                        }
                        else
                        {
                            CurrentGameState.highscorecondition = EndState.GaveUp;
                            Application.LoadLevel(4);
                        }
                    }
                }
            }
        }
    }

    void Start()
    {

        aftercountdown = 1.2f;
        firstGUI = true;
        guin = GetComponent<GUINavigation>();
        pms = GetComponent<PauseMenuScript>();
    }
}
