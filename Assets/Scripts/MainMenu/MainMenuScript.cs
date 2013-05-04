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

    private float countdown;
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
        GUI.Label(new Rect(Screen.width / 2 - 400, 40, 800, 100), "The assault on East Castle");
        GUI.EndGroup();
        GUI.BeginGroup(new Rect(0, Screen.height / 2 - 140, Screen.width, Screen.height));
        GUI.SetNextControlName("Campaign");
        if (GUI.Button(new Rect(0, 0 * 60, Screen.width - 30, 55), "Start Campaign")) 
            Menu_Main_Start_Campaign();
        GUI.SetNextControlName("Endless");
        if (GUI.Button(new Rect(0, 1 * 60, Screen.width - 30, 55), "Start Eternal Rush")) 
            Menu_Main_Start_Endless();
        GUI.SetNextControlName("Controls");
        if (GUI.Button(new Rect(0, 2 * 60, Screen.width - 30, 55), "Controls")) 
            Menu_Main_Controls();
        GUI.SetNextControlName("Options");
        if (GUI.Button(new Rect(0, 3 * 60, Screen.width - 30, 55), "Options")) 
            Menu_Main_Options();
        GUI.SetNextControlName("Highscores");
        if (GUI.Button(new Rect(0, 4 * 60, Screen.width - 30, 55), "Highscores")) 
            Menu_Main_Highscores();
        GUI.SetNextControlName("Credits");
        if (GUI.Button(new Rect(0, 5 * 60, Screen.width - 30, 55), "Credits")) 
            Menu_Main_Credits();
        GUI.SetNextControlName("Quit");
        if (GUI.Button(new Rect(0, 6 * 60, Screen.width - 30, 55), "Quit")) 
            Menu_Main_Quit();

        GUI.Box(new Rect(0, 0 * 60, Screen.width - 30, 55), new GUIContent("", "0"));
        GUI.Box(new Rect(0, 1 * 60, Screen.width - 30, 55), new GUIContent("", "1"));
        GUI.Box(new Rect(0, 2 * 60, Screen.width - 30, 55), new GUIContent("", "2"));
        GUI.Box(new Rect(0, 3 * 60, Screen.width - 30, 55), new GUIContent("", "3"));
        GUI.Box(new Rect(0, 4 * 60, Screen.width - 30, 55), new GUIContent("", "4"));
        GUI.Box(new Rect(0, 5 * 60, Screen.width - 30, 55), new GUIContent("", "5"));
        GUI.Box(new Rect(0, 6 * 60, Screen.width - 30, 55), new GUIContent("", "6"));
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
        guin = GetComponent<GUINavigation>();
    }

    void Update()
    {
        if (stopped || started)
        {
            countdown -= 0.02f;
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
                if (infiniteMode)
                    Application.LoadLevel(5);
                else
                    Application.LoadLevel(1);
            }
        }
    }
}
