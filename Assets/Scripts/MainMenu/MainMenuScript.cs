using UnityEngine;
using System.Collections;

public class MainMenuScript : MonoBehaviour {
	
	
	public GUISkin gSkin;
    public Texture2D black;
    public MusicVolumeSetter music;

    private float countdown;
    private bool stopped, started, usingMouse, movedUp, movedDown, firstGUI, activated;
    private int keySelect;
    private Texture2D background;
    private Color activeColor, inactiveColor;
    private string mouseover;


	void Menu_Main() {
        if (!usingMouse)
        {
            GUI.skin.button.hover.background = null;
        }
        else
        {
            GUI.skin.button.hover.background = background;
        }
        if (activated)
        {
            GUI.skin.button.focused.textColor = activeColor;
        }
        else
        {
            GUI.skin.button.focused.textColor = inactiveColor;
        }

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
        if (GUI.Button(new Rect(0, 3 * 70, Screen.width - 30, 64), "Quit")) { Menu_Main_Quit(); }

        GUI.Box(new Rect(0, 0 * 70, Screen.width - 30, 64), new GUIContent("", "0"));
        GUI.Box(new Rect(0, 1 * 70, Screen.width - 30, 64), new GUIContent("", "1"));
        GUI.Box(new Rect(0, 2 * 70, Screen.width - 30, 64), new GUIContent("", "2"));
        GUI.Box(new Rect(0, 3 * 70, Screen.width - 30, 64), new GUIContent("", "3"));
        mouseover = GUI.tooltip;
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
            if (!usingMouse)
            {
                switch (keySelect)
                {
                    case 0: GUI.FocusControl("Start"); break;
                    case 1: GUI.FocusControl("Options"); break;
                    case 2: GUI.FocusControl("Highscores"); break;
                    case 3: GUI.FocusControl("Quit"); break;
                }
            }
            else
                GUI.FocusControl("title");
        }

        GUI.skin.button.hover.background = background;
        GUI.skin.button.focused.textColor = inactiveColor;
        //if (!usingMouse)
        //{
        //    GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
        //    GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");
        //    GUI.EndGroup();
        //}
        //if (!usingMouse)
        //{
        //    GUI.skin.button.hover = st;
        //    GUI.skin.button.onHover = st2;
        //}
	}

    void Menu_Main_Start_Game() {
        if (!started && !stopped)
        {
            //Application.LoadLevel(1);
            started = false;
            stopped = true;
            countdown = 1f;
            music.useGlobal = false;
        }
    }

    void Menu_Main_Options() {
        if (!started && !stopped)
        {
            this.enabled = false;
            GetComponent<OptionsMenuScript>().Menu_Options_Startup();
        }
    }

    void Menu_Main_Highscores()
    {
        if (!started && !stopped)
        {
            this.enabled = false;
            GetComponent<HighScoreMenuScript>().enabled = true;
        }
    }

    void Menu_Main_Quit() {
        if (!started && !stopped)
        {
            this.enabled = false;
            GetComponent<QuitAcceptMenu>().enabled = true;
        }
    }

	void OnGUI() {
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
            Menu_Main_Quit();
        }
        else
            Menu_Main();
	}

    void Start()
    {
        activated = false;
        firstGUI = true;
        usingMouse = false;
        movedUp = false;
        movedDown = false;
        Screen.showCursor = false;
        music.useGlobal = false;
        started = true;
        countdown = 1f;
        keySelect = -1;
    }

    void Update()
    {
        print(mouseover);
        if (!activated)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                activated = true;
            }
        }
        else
        {
            if (Input.GetKeyUp(KeyCode.Return))
            {
                activated = false;
                switch (keySelect) {
                    case 0: Menu_Main_Start_Game(); break;
                    case 1: Menu_Main_Options(); break;
                    case 2: Menu_Main_Highscores(); break;
                    case 3: Menu_Main_Quit(); break;
                }
            }
        }
        if (!usingMouse && KeyOrMouse.MouseUsed())
        {
            usingMouse = true;
            Screen.showCursor = true;
            print("mouse used!");
        }
        if (usingMouse && KeyOrMouse.KeyUsed())
        {
            usingMouse = false;
            Screen.showCursor = false;
            print("key used!");
        }
        if (!usingMouse)
        {
            if (keySelect == -1)
            {
                if (Input.GetAxisRaw("Vertical") > 0)
                {
                    keySelect = 3;
                    movedDown = true;
                    movedUp = false;
                }
                else if (Input.GetAxisRaw("Vertical") < 0)
                {
                    keySelect = 0;
                    movedUp = true;
                    movedDown = false;
                }
                else
                {
                    movedDown = false;
                    movedUp = false;
                }
            }
            else
            {
                if (Input.GetAxisRaw("Vertical") > 0)
                    if (keySelect == 0)
                    {
                        if (!movedDown)
                        {
                            keySelect = 3;
                            movedDown = true;
                            movedUp = false;
                        }
                    }
                    else
                    {
                        if (!movedDown)
                        {
                            keySelect--;
                            movedDown = true;
                            movedUp = false;
                        }
                    }
                else if (Input.GetAxisRaw("Vertical") < 0)
                    if (keySelect == 3)
                    {
                        if (!movedUp)
                        {
                            keySelect = 0;
                            movedUp = true;
                            movedDown = false;
                        }
                    }
                    else
                    {
                        if (!movedUp)
                        {
                            keySelect++;
                            movedUp = true;
                            movedDown = false;
                        }
                    }
                else
                {
                    movedDown = false;
                    movedUp = false;
                }
            }
        }
        else
        {
            if (mouseover != null) {
                if (mouseover.Equals("0"))
                    keySelect = 0;
                else if (mouseover.Equals("1"))
                    keySelect = 1;
                else if (mouseover.Equals("2"))
                    keySelect = 2;
                else if (mouseover.Equals("3"))
                    keySelect = 3;
            }
        }

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
