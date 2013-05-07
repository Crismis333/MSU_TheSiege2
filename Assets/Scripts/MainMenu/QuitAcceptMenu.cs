using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GUINavigation))]
public class QuitAcceptMenu : MonoBehaviour {

    public GUISkin gSkin;
    public EffectVolumeSetter cancelSound;
    public EffectVolumeSetter quitSound;
    public Texture2D black;
    public MusicVolumeSetter music;

    private Texture2D background;
    private Color activeColor, inactiveColor;
    private bool firstGUI, stopped;
    private float countdown, aftercountdown;

    private GUINavigation guin;

    void Menu_Quit()
    {
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
        GUI.Label(new Rect(Screen.width / 2 - 400 - 1, 40 - 1, 800, 100), "The Rush of the Vanguard");
        GUI.Label(new Rect(Screen.width / 2 - 400 - 1, 40 - 2, 800, 100), "The Rush of the Vanguard");
        GUI.Label(new Rect(Screen.width / 2 - 400 - 2, 40 - 2, 800, 100), "The Rush of the Vanguard");
        GUI.Label(new Rect(Screen.width / 2 - 400 - 2, 40 - 1, 800, 100), "The Rush of the Vanguard");
        GUI.color = new Color(206 / 256f, 206 / 256f, 206f / 256f);
        GUI.Label(new Rect(Screen.width / 2 - 400, 40, 800, 100), "The Rush of the Vanguard");
        GUI.EndGroup();

        GUI.BeginGroup(new Rect(0, Screen.height / 2 - 100, Screen.width, Screen.height));
        GUI.skin.label.fontSize = 24;
        GUI.skin.label.alignment = TextAnchor.MiddleLeft;
        GUI.SetNextControlName("title");
        GUI.color = Color.black;
        GUI.Label(new Rect(-1, 0 * 70-1, Screen.width, 64), "Do you really wish to quit?");
        GUI.Label(new Rect(1, 0 * 70-1, Screen.width, 64), "Do you really wish to quit?");
        GUI.Label(new Rect(-1, 0 * 70+1, Screen.width, 64), "Do you really wish to quit?");
        GUI.Label(new Rect(1, 0 * 70+1, Screen.width, 64), "Do you really wish to quit?");
        GUI.color = new Color(206 / 256f, 206 / 256f, 206f / 256f);
        GUI.Label(new Rect(0, 0 * 70, Screen.width, 64), "Do you really wish to quit?");
        GUI.SetNextControlName("Yes");
        GUI.color = Color.black;
        GUI.Label(new Rect(-1, 1 * 70-1, Screen.width, 64), "Yes");
        GUI.Label(new Rect(1, 1 * 70-1, Screen.width, 64), "Yes");
        GUI.Label(new Rect(-1, 1 * 70+1, Screen.width, 64), "Yes");
        GUI.Label(new Rect(1, 1 * 70+1, Screen.width, 64), "Yes");
        GUI.color = new Color(206 / 256f, 206 / 256f, 206f / 256f);
        if (GUI.Button(new Rect(0, 1 * 70, Screen.width, 64), "Yes")) 
            Menu_Quit_Yes();
        GUI.SetNextControlName("No");
        GUI.color = Color.black;
        GUI.Label(new Rect(-1, 2 * 70-1, Screen.width, 64), "No");
        GUI.Label(new Rect(1, 2 * 70-1, Screen.width, 64), "No");
        GUI.Label(new Rect(-1, 2 * 70+1, Screen.width, 64), "No");
        GUI.Label(new Rect(1, 2 * 70+1, Screen.width, 64), "No");
        GUI.color = new Color(206 / 256f, 206 / 256f, 206f / 256f);
        if (GUI.Button(new Rect(0, 2 * 70, Screen.width, 64), "No")) 
            Menu_Quit_No();

        GUI.Box(new Rect(0, 1 * 70, Screen.width, 64), new GUIContent("", "0"));
        GUI.Box(new Rect(0, 2 * 70, Screen.width, 64), new GUIContent("", "1"));
        guin.mouseover = GUI.tooltip;

        GUI.EndGroup();

        GUI.skin.button.hover.background = background;
        GUI.skin.button.focused.textColor = inactiveColor;

        if (!guin.usingMouse)
            switch (guin.keySelect)
            {
                case 0: GUI.FocusControl("Yes"); break;
                case 1: GUI.FocusControl("No"); break;
                default: GUI.FocusControl("title"); break;
            }
        else
            GUI.FocusControl("title");

        if (stopped)
        {
            GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
            GUI.color = new Color(1, 1, 1, Mathf.Lerp(0, 1, 1 - countdown));
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), black);
            music.volume = Mathf.Lerp(OptionsValues.musicVolume, 0, 1 - countdown);
            GUI.EndGroup();
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
            Menu_Quit_No();
        else
            Menu_Quit();
    }

    public void Menu_Quit_Yes()
    {
        quitSound.Play();
        stopped = true;
        music.useGlobal = false;
    }

    public void Menu_Quit_No()
    {
        this.enabled = false;
        cancelSound.Play();
        MainMenuScript mms = GetComponent<MainMenuScript>();
        mms.enabled = true;
        guin.ClearElements();
        guin.maxKeys = 7;
        guin.AddElement(0, mms.Menu_Main_Start_Campaign);
        guin.AddElement(1, mms.Menu_Main_Start_Endless);
        guin.AddElement(2, mms.Menu_Main_Controls);
        guin.AddElement(3, mms.Menu_Main_Options);
        guin.AddElement(4, mms.Menu_Main_Highscores);
        guin.AddElement(5, mms.Menu_Main_Credits);
        guin.AddElement(6, mms.Menu_Main_Quit);
    }

    void Start()
    {
        aftercountdown = 1.2f;
        countdown = 1f;
        firstGUI = true;
        stopped = false;
        guin = GetComponent<GUINavigation>(); 
    }

    void Update()
    {
        if (stopped)
        {
            if (countdown > 0)
                countdown -= Time.deltaTime/2;
            else
            {
                if (aftercountdown > 0)
                    aftercountdown -= Time.deltaTime;
                else
                    Application.Quit();
            }
        }
    }
}
