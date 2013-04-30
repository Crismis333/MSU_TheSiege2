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
    private float countdown;

    void Menu_Quit()
    {
        if (!GetComponent<GUINavigation>().usingMouse)
            GUI.skin.button.hover.background = null;
        else
            GUI.skin.button.hover.background = background;
        if (GetComponent<GUINavigation>().activated)
            GUI.skin.button.focused.textColor = activeColor;
        else
            GUI.skin.button.focused.textColor = inactiveColor;
        GUI.BeginGroup(new Rect(0, Screen.height / 2 - 100, Screen.width, Screen.height));

        GUI.SetNextControlName("title");
        GUI.Label(new Rect(0, 0 * 70, Screen.width, 64), "Do you really wish to quit?");
        GUI.SetNextControlName("Yes");
        if (GUI.Button(new Rect(0, 1 * 70, Screen.width, 64), "Yes")) { Menu_Quit_Yes(); }
        GUI.SetNextControlName("No");
        if (GUI.Button(new Rect(0, 2 * 70, Screen.width, 64), "No")) { Menu_Quit_No(); }

        GUI.Box(new Rect(0, 1 * 70, Screen.width, 64), new GUIContent("", "0"));
        GUI.Box(new Rect(0, 2 * 70, Screen.width, 64), new GUIContent("", "1"));
        GetComponent<GUINavigation>().mouseover = GUI.tooltip;

        GUI.EndGroup();

        GUI.skin.button.hover.background = background;
        GUI.skin.button.focused.textColor = inactiveColor;

        if (!GetComponent<GUINavigation>().usingMouse)
        {
            switch (GetComponent<GUINavigation>().keySelect)
            {
                case 0: GUI.FocusControl("Yes"); break;
                case 1: GUI.FocusControl("No"); break;
                default: GUI.FocusControl("title"); break;
            }
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Menu_Quit_No();
        }
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
        GetComponent<MainMenuScript>().enabled = true;
        cancelSound.Play();
        GetComponent<GUINavigation>().ClearElements();
        GetComponent<GUINavigation>().maxKeys = 7;
        GetComponent<GUINavigation>().AddElement(0, GetComponent<MainMenuScript>().Menu_Main_Start_Campaign);
        GetComponent<GUINavigation>().AddElement(1, GetComponent<MainMenuScript>().Menu_Main_Start_Endless);
        GetComponent<GUINavigation>().AddElement(2, GetComponent<MainMenuScript>().Menu_Main_Controls);
        GetComponent<GUINavigation>().AddElement(3, GetComponent<MainMenuScript>().Menu_Main_Options);
        GetComponent<GUINavigation>().AddElement(4, GetComponent<MainMenuScript>().Menu_Main_Highscores);
        GetComponent<GUINavigation>().AddElement(5, GetComponent<MainMenuScript>().Menu_Main_Credits);
        GetComponent<GUINavigation>().AddElement(6, GetComponent<MainMenuScript>().Menu_Main_Quit);
    }

    void Start()
    {
        countdown = 1f;
        firstGUI = true;
        stopped = false;
    }

    void Update()
    {
        if (stopped)
        {
            if (countdown > 0)
                countdown -= Time.deltaTime;
            else
            {
                Application.Quit();
            }
        }
    }
}
