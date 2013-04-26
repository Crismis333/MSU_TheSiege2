using UnityEngine;
using System.Collections;

public class MainMenuScript : MonoBehaviour {
	
	
	public GUISkin gSkin;
    public Texture2D black;
    public MusicVolumeSetter music;

    private float countdown;
    private bool started, stopped, firstGUI;
    private Texture2D background;
    private Color activeColor, inactiveColor;

	void Menu_Main() {
        if (!GetComponent<GUINavigation>().usingMouse)
            GUI.skin.button.hover.background = null;
        else
            GUI.skin.button.hover.background = background;
        if (GetComponent<GUINavigation>().activated)
            GUI.skin.button.focused.textColor = activeColor;
        else
            GUI.skin.button.focused.textColor = inactiveColor;

        GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
        GUI.SetNextControlName("title");
        GUI.Label(new Rect(Screen.width / 2 - 400, 40, 800, 100), "The assault on East Castle");
        GUI.EndGroup();
        GUI.BeginGroup(new Rect(0, Screen.height / 2 - 100, Screen.width, Screen.height));
        GUI.SetNextControlName("Start");
        if (GUI.Button(new Rect(0, 0 * 70, Screen.width - 30, 64), "Start Game")) { Menu_Main_Start_Game(); }
        GUI.SetNextControlName("Options");
        if (GUI.Button(new Rect(0, 1 * 70, Screen.width - 30, 64), "Options")) { Menu_Main_Options(); }
        GUI.SetNextControlName("Highscores");
        if (GUI.Button(new Rect(0, 2 * 70, Screen.width - 30, 64), "Highscores")) { Menu_Main_Highscores(); }
        GUI.SetNextControlName("Quit");
        if (GUI.Button(new Rect(0, 3 * 70, Screen.width - 30, 64), "Quit")) { Menu_Main_Quit();  }

        GUI.Box(new Rect(0, 0 * 70, Screen.width - 30, 64), new GUIContent("", "0"));
        GUI.Box(new Rect(0, 1 * 70, Screen.width - 30, 64), new GUIContent("", "1"));
        GUI.Box(new Rect(0, 2 * 70, Screen.width - 30, 64), new GUIContent("", "2"));
        GUI.Box(new Rect(0, 3 * 70, Screen.width - 30, 64), new GUIContent("", "3"));
        Camera.mainCamera.GetComponent<GUINavigation>().mouseover = GUI.tooltip;
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
            if (!GetComponent<GUINavigation>().usingMouse)
            {
                switch (GetComponent<GUINavigation>().keySelect)
                {
                    case 0: GUI.FocusControl("Start"); break;
                    case 1: GUI.FocusControl("Options"); break;
                    case 2: GUI.FocusControl("Highscores"); break;
                    case 3: GUI.FocusControl("Quit"); break;
                    default: GUI.FocusControl("title"); break;
                }
            }
            else
                GUI.FocusControl("title");
        }

        GUI.skin.button.hover.background = background;
        GUI.skin.button.focused.textColor = inactiveColor;
	}

    public void Menu_Main_Start_Game()
    {
        if (!started && !stopped)
        {
            //Application.LoadLevel(1);
            CurrentGameState.InfiniteMode = false;
            started = false;
            stopped = true;
            countdown = 1f;
            music.useGlobal = false;
        }
    }

    public void Menu_Main_Options()
    {
        if (!started && !stopped)
        {
            this.enabled = false;
            GetComponent<OptionsMenuScript>().Menu_Options_Startup();
            GetComponent<GUINavigation>().ClearElements();
            GetComponent<GUINavigation>().maxKeys = 6;
            GetComponent<GUINavigation>().menuKey = 5;
            GetComponent<GUINavigation>().AddElement(5, GetComponent<OptionsMenuScript>().Menu_Options_Back);
        }
    }

    public void Menu_Main_Highscores()
    {
        if (!started && !stopped)
        {
            this.enabled = false;
            GetComponent<HighScoreMenuScript>().enabled = true;
            GetComponent<GUINavigation>().ClearElements();
            GetComponent<GUINavigation>().maxKeys = 1;
            GetComponent<GUINavigation>().menuKey = 0;
            GetComponent<GUINavigation>().AddElement(0, GetComponent<HighScoreMenuScript>().Accept);
        }
    }

    public void Menu_Main_Quit()
    {
        if (!started && !stopped)
        {
            this.enabled = false;
            GetComponent<QuitAcceptMenu>().enabled = true;
            GetComponent<GUINavigation>().ClearElements();
            GetComponent<GUINavigation>().maxKeys = 2;
            GetComponent<GUINavigation>().AddElement(0, GetComponent<QuitAcceptMenu>().Menu_Quit_Yes);
            GetComponent<GUINavigation>().AddElement(1, GetComponent<QuitAcceptMenu>().Menu_Quit_No);
        }
    }

	void OnGUI() {
		GUI.skin = gSkin;
        if (firstGUI)
        {
            GetComponent<GUINavigation>().maxKeys = 4;
            GetComponent<GUINavigation>().AddElement(0, Menu_Main_Start_Game);
            GetComponent<GUINavigation>().AddElement(1, Menu_Main_Options);
            GetComponent<GUINavigation>().AddElement(2, Menu_Main_Highscores);
            GetComponent<GUINavigation>().AddElement(3, Menu_Main_Quit);
            background = GUI.skin.button.hover.background;
            activeColor = GUI.skin.button.active.textColor;
            inactiveColor = GUI.skin.button.focused.textColor;
            firstGUI = false;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Menu_Main_Quit();
        }
        else
            Menu_Main();
	}

    void Start()
    {
        firstGUI = true;
        music.useGlobal = false;
        started = true;
        countdown = 1f;
    }

    void Update()
    {
        //print("skin is null: " + (gSkin.button.hover.background == null) + ", mouseactivated: " + Camera.mainCamera.GetComponent<GUINavigation>().usingMouse);
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
                Application.LoadLevel(1);
            }
        }
    }
}
