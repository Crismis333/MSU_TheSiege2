using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GUINavigation))]
[RequireComponent(typeof(OptionsMenuScript))]
[RequireComponent(typeof(QuitAcceptMenu))]
[RequireComponent(typeof(HighScoreMenuScript))]
[RequireComponent(typeof(ControlsScript))]
[RequireComponent(typeof(CreditsScript))]
public class MainMenuScript : MonoBehaviour {
	
	public GUISkin gSkin;
    public Texture2D black;
    public MusicVolumeSetter music;
    public EffectVolumeSetter startGameSound;
    public EffectVolumeSetter selectSound;

    private float countdown, aftercountdown;
    private bool started, stopped, firstGUI;
    private Texture2D background;
    private Color activeColor, inactiveColor;
    private bool infiniteMode;

    private GUINavigation guin;

	void Menu_Main() {
        if (!guin.usingMouse)
            GUI.skin.button.hover.background = null;
        else
            GUI.skin.button.hover.background = background;
        if (guin.activated)
            GUI.skin.button.focused.textColor = activeColor;
        else
            GUI.skin.button.focused.textColor = inactiveColor;

        GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
        GUI.SetNextControlName("title");
        GUI.skin.label.fontSize = 40;
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUI.color = Color.black;
        GUI.Label(new Rect(Screen.width / 2 - 400-1, 40-1, 800, 100), "Rush of the Vanguard");
        GUI.Label(new Rect(Screen.width / 2 - 400-1, 40-2, 800, 100), "Rush of the Vanguard");
        GUI.Label(new Rect(Screen.width / 2 - 400-2, 40-2, 800, 100), "Rush of the Vanguard");
        GUI.Label(new Rect(Screen.width / 2 - 400-2, 40-1, 800, 100), "Rush of the Vanguard");
        GUI.color = new Color(206 / 256f, 206 / 256f, 206f / 256f);
        GUI.Label(new Rect(Screen.width / 2 - 400, 40, 800, 100), "Rush of the Vanguard");
        GUI.EndGroup();

        GUI.BeginGroup(new Rect(0, Screen.height / 2 - 140, Screen.width, Screen.height));
        GUI.SetNextControlName("Campaign");
        GUI.skin.label.fontSize = 24;
        GUI.skin.label.alignment = TextAnchor.MiddleLeft;
        GUI.color = Color.black;
        GUI.Label(new Rect(-1, 0 * 60 - 1, Screen.width - 30, 55), "Start Campaign");
        GUI.Label(new Rect(1, 0 * 60 + 1, Screen.width - 30, 55), "Start Campaign");
        GUI.Label(new Rect(-1, 0 * 60 + 1, Screen.width - 30, 55), "Start Campaign");
        GUI.Label(new Rect(1, 0 * 60 - 1, Screen.width - 30, 55), "Start Campaign");
        GUI.color = new Color(206 / 256f, 206 / 256f, 206f / 256f);
        if (GUI.Button(new Rect(0, 0 * 60, 600, 55), "Start Campaign"))
            Menu_Main_Start_Campaign();


        GUI.SetNextControlName("Endless"); 
        GUI.color = Color.black;
        GUI.Label(new Rect(-1, 1 * 60 - 1, Screen.width - 30, 55), "Start Eternal Rush");
        GUI.Label(new Rect(1, 1 * 60 + 1, Screen.width - 30, 55), "Start Eternal Rush");
        GUI.Label(new Rect(-1, 1 * 60 + 1, Screen.width - 30, 55), "Start Eternal Rush");
        GUI.Label(new Rect(1, 1 * 60 - 1, Screen.width - 30, 55), "Start Eternal Rush");
        GUI.color = new Color(206 / 256f, 206 / 256f, 206f / 256f);
        if (GUI.Button(new Rect(0, 1 * 60, 600, 55), "Start Eternal Rush")) 
            Menu_Main_Start_Endless();


        GUI.SetNextControlName("Controls");
        GUI.color = Color.black;
        GUI.Label(new Rect(-1, 2 * 60 - 1, Screen.width - 30, 55), "Instructions");
        GUI.Label(new Rect(1, 2 * 60 + 1, Screen.width - 30, 55), "Instructions");
        GUI.Label(new Rect(-1, 2 * 60 + 1, Screen.width - 30, 55), "Instructions");
        GUI.Label(new Rect(1, 2 * 60 - 1, Screen.width - 30, 55), "Instructions");
        GUI.color = new Color(206 / 256f, 206 / 256f, 206f / 256f);
        if (GUI.Button(new Rect(0, 2 * 60, 600, 55), "Instructions")) 
            Menu_Main_Controls();
        GUI.SetNextControlName("Options");
        GUI.color = Color.black;
        GUI.Label(new Rect(-1, 3 * 60 - 1, Screen.width - 30, 55), "Options");
        GUI.Label(new Rect(1, 3 * 60 + 1, Screen.width - 30, 55), "Options");
        GUI.Label(new Rect(-1, 3 * 60 + 1, Screen.width - 30, 55), "Options");
        GUI.Label(new Rect(1, 3 * 60 - 1, Screen.width - 30, 55), "Options");
        GUI.color = new Color(206 / 256f, 206 / 256f, 206f / 256f);
        if (GUI.Button(new Rect(0, 3 * 60, 600, 55), "Options")) 
            Menu_Main_Options();
        GUI.SetNextControlName("Highscores");
        GUI.color = Color.black;
        GUI.Label(new Rect(-1, 4 * 60 - 1, Screen.width - 30, 55), "Highscores");
        GUI.Label(new Rect(1, 4 * 60 + 1, Screen.width - 30, 55), "Highscores");
        GUI.Label(new Rect(-1, 4 * 60 + 1, Screen.width - 30, 55), "Highscores");
        GUI.Label(new Rect(1, 4 * 60 - 1, Screen.width - 30, 55), "Highscores");
        GUI.color = new Color(206 / 256f, 206 / 256f, 206f / 256f);
        if (GUI.Button(new Rect(0, 4 * 60, 600, 55), "Highscores")) 
            Menu_Main_Highscores();
        GUI.SetNextControlName("Credits");
        GUI.color = Color.black;
        GUI.Label(new Rect(-1, 5 * 60 - 1, Screen.width - 30, 55), "Credits");
        GUI.Label(new Rect(1, 5 * 60 + 1, Screen.width - 30, 55), "Credits");
        GUI.Label(new Rect(-1, 5 * 60 + 1, Screen.width - 30, 55), "Credits");
        GUI.Label(new Rect(1, 5 * 60 - 1, Screen.width - 30, 55), "Credits");
        GUI.color = new Color(206 / 256f, 206 / 256f, 206f / 256f);
        if (GUI.Button(new Rect(0, 5 * 60, 600, 55), "Credits")) 
            Menu_Main_Credits();
        GUI.SetNextControlName("Quit");
        GUI.color = Color.black;
        GUI.Label(new Rect(-1, 6 * 60 - 1, Screen.width - 30, 55), "Quit");
        GUI.Label(new Rect(1, 6 * 60 + 1, Screen.width - 30, 55), "Quit");
        GUI.Label(new Rect(-1, 6 * 60 + 1, Screen.width - 30, 55), "Quit");
        GUI.Label(new Rect(1, 6 * 60 - 1, Screen.width - 30, 55), "Quit");
        GUI.color = new Color(206 / 256f, 206 / 256f, 206f / 256f);
        if (GUI.Button(new Rect(0, 6 * 60, 600, 55), "Quit")) 
            Menu_Main_Quit();

        GUI.Box(new Rect(0, 0 * 60, 600, 55), new GUIContent("", "0"));
        GUI.Box(new Rect(0, 1 * 60, 600, 55), new GUIContent("", "1"));
        GUI.Box(new Rect(0, 2 * 60, 600, 55), new GUIContent("", "2"));
        GUI.Box(new Rect(0, 3 * 60, 600, 55), new GUIContent("", "3"));
        GUI.Box(new Rect(0, 4 * 60, 600, 55), new GUIContent("", "4"));
        GUI.Box(new Rect(0, 5 * 60, 600, 55), new GUIContent("", "5"));
        GUI.Box(new Rect(0, 6 * 60, 600, 55), new GUIContent("", "6"));
        guin.mouseover = GUI.tooltip;
		GUI.EndGroup();
        if (stopped)
        {
            GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
            GUI.color = new Color(1, 1, 1, Mathf.Lerp(0, 1, 1 - countdown));
            music.volume = Mathf.Lerp(OptionsValues.musicVolume, 0, 1 - countdown);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), black);
            GUI.EndGroup();
        }
        else if (started)
        {
            GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
            GUI.color = new Color(1, 1, 1, Mathf.Lerp(1, 0, 1 - countdown));
            music.volume = Mathf.Lerp(0, OptionsValues.musicVolume, 1 - countdown);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), black);
            GUI.EndGroup();
        }
        else
        {
            music.volume = OptionsValues.musicVolume;
            if (!guin.usingMouse)
                switch (guin.keySelect)
                {
                    case 0: GUI.FocusControl("Campaign"); break;
                    case 1: GUI.FocusControl("Endless"); break;
                    case 2: GUI.FocusControl("Controls"); break;
                    case 3: GUI.FocusControl("Options"); break;
                    case 4: GUI.FocusControl("Highscores"); break;
                    case 5: GUI.FocusControl("Credits"); break;
                    case 6: GUI.FocusControl("Quit"); break;
                    default: GUI.FocusControl("title"); break;
                }
            else
                GUI.FocusControl("title");
        }

        GUI.skin.button.hover.background = background;
        GUI.skin.button.focused.textColor = inactiveColor;
	}

    public void Menu_Main_Start_Campaign()
    {
        if (!started && !stopped)
        {
            infiniteMode = false;
            CurrentGameState.InfiniteMode = false;
            LevelCreator.INF_MODE = false;
            started = false;
            stopped = true;
            countdown = 1f;
            music.useGlobal = false;
            startGameSound.Play();
        }
    }

    public void Menu_Main_Start_Endless()
    {
        if (!started && !stopped)
        {
            infiniteMode = true;
            CurrentGameState.InfiniteMode = true;
            LevelCreator.INF_MODE = true;
            started = false;
            stopped = true;
            countdown = 1f;
            music.useGlobal = false;
            startGameSound.Play();
        }
    }

    public void Menu_Main_Controls()
    {
        if (!started && !stopped)
        {
            this.enabled = false;
            GetComponent<ControlsScript>().enabled = true;
            guin.ClearElements();
            guin.maxKeys = 1;
            guin.menuKey = 0;
            guin.AddElement(0, GetComponent<ControlsScript>().Return);
            selectSound.Play();
        }
    }

    public void Menu_Main_Options()
    {
        if (!started && !stopped)
        {
            this.enabled = false;
            GetComponent<OptionsMenuScript>().Menu_Options_Startup();
            guin.ClearElements();
            guin.maxKeys = 6;
            guin.menuKey = 5;
            guin.AddElement(5, GetComponent<OptionsMenuScript>().Menu_Options_Back);
            selectSound.Play();
        }
    }

    public void Menu_Main_Highscores()
    {
        if (!started && !stopped)
        {
            this.enabled = false;
            GetComponent<HighScoreMenuScript>().enabled = true;
            guin.ClearElements();
            guin.maxKeys = 1;
            guin.menuKey = 0;
            guin.AddElement(0, GetComponent<HighScoreMenuScript>().Accept);
            selectSound.Play();
        }
    }

    public void Menu_Main_Credits()
    {
        if (!started && !stopped)
        {
            this.enabled = false;
            GetComponent<CreditsScript>().enabled = true;
            guin.ClearElements();
            guin.maxKeys = 1;
            guin.menuKey = 0;
            guin.AddElement(0, GetComponent<CreditsScript>().Return);
            selectSound.Play();
        }
    }


    public void Menu_Main_Quit()
    {
        if (!started && !stopped)
        {
            this.enabled = false;
            GetComponent<QuitAcceptMenu>().enabled = true;
            guin.ClearElements();
            guin.maxKeys = 2;
            guin.AddElement(0, GetComponent<QuitAcceptMenu>().Menu_Quit_Yes);
            guin.AddElement(1, GetComponent<QuitAcceptMenu>().Menu_Quit_No);
            selectSound.Play();
        }
    }

	void OnGUI() {
		GUI.skin = gSkin;
        if (firstGUI)
        {
            guin.maxKeys = 7;
            guin.AddElement(0, Menu_Main_Start_Campaign);
            guin.AddElement(1, Menu_Main_Start_Endless);
            guin.AddElement(2, Menu_Main_Controls);
            guin.AddElement(3, Menu_Main_Options);
            guin.AddElement(4, Menu_Main_Highscores);
            guin.AddElement(5, Menu_Main_Credits);
            guin.AddElement(6, Menu_Main_Quit);
            background = GUI.skin.button.hover.background;
            activeColor = GUI.skin.button.active.textColor;
            inactiveColor = GUI.skin.button.focused.textColor;
            firstGUI = false;
        }
        if (guin.QuitPressed())
            Menu_Main_Quit();
        else
            Menu_Main();
	}

    void Start()
    {
        firstGUI = true;
        music.useGlobal = false;
        started = true;
        countdown = 1f;
        aftercountdown = 0.8f;
        guin = GetComponent<GUINavigation>();
    }

    void Update()
    {
        if (stopped || started)
        {
            if (started)
                countdown -= Time.deltaTime;
            else
                countdown -= Time.deltaTime / 2;
            if (started)
            {
                if (countdown < 0)
                {
                    countdown = 0;
                    started = false;
                    music.useGlobal = true;
                }

            }
            if (stopped && countdown < 0)
            {
                aftercountdown -= Time.deltaTime;
                if (aftercountdown < 0)
                    if (infiniteMode)
                        Application.LoadLevel(5);
                    else
                        Application.LoadLevel(1);
            }
        }
    }
}
