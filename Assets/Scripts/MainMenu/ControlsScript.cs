using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GUINavigation))]
public class ControlsScript : MonoBehaviour {

    public GUISkin gSkin;
    public Texture2D backgroundScroll;
    public Vector2 scrollOffset;
    public bool mainMenu;
    public EffectVolumeSetter cancelSound;
    public EffectVolumeSetter selectSound;

    private Texture2D background;
    private Color activeColor, inactiveColor;
    private bool firstGUI;
    private bool movedLeft, movedRight;
    private int screenNr, maxPages;
    void Menu_Controls()
    {
        if (!GetComponent<GUINavigation>().usingMouse)
            GUI.skin.button.hover.background = null;
        else
            GUI.skin.button.hover.background = background;
        if (GetComponent<GUINavigation>().activated)
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
        GUI.color = Color.black;

        GUI.SetNextControlName("title");
        GUI.Label(new Rect(60, 1 * 30 + 10, 790, 64), "Controls page " + (screenNr+1) + "/" + maxPages);

        GUI.color = Color.white;
        GUI.SetNextControlName("Return");
        if (GUI.Button(new Rect(60, 12 * 35, 640, 64), "Return")) { Return(); }
        GUI.Box(new Rect(60, 12 * 35, 640, 64), new GUIContent("", "0"));
        GetComponent<GUINavigation>().mouseover = GUI.tooltip;

        GUI.EndGroup();

        GUI.skin.button.fontSize = 40;
            //GUI.BeginGroup(new Rect(Screen.width / 2 - 395, Screen.height / 2 - 5 * 35, 790, 10 * 35));
        GUI.BeginGroup(new Rect(Screen.width / 2 - 495, Screen.height / 2 - 7.5f * 35, 155, 15 * 35));
        if (GUI.Button(new Rect(0, 6 * 30, 155, 100), "<"))
        {
            screenNr--;
            GetComponent<GUINavigation>().SetNoPlay();
            selectSound.Play();
            if (screenNr < 0) screenNr = maxPages-1;
        }
        GUI.Box(new Rect(0, 6 * 30, 155, 100), new GUIContent("", "left"));
        GUI.EndGroup();

        GUI.BeginGroup(new Rect(Screen.width / 2 + 280, Screen.height / 2 - 7.5f * 35, 155, 15 * 35));
        if (GUI.Button(new Rect(0, 6 * 30, 155, 100), ">"))
        {
            screenNr++;
            GetComponent<GUINavigation>().SetNoPlay();
            selectSound.Play();
            if (screenNr > maxPages-1) screenNr = 0;
        }
        GUI.Box(new Rect(0, 6 * 30, 155, 100), new GUIContent("", "right"));
        GUI.EndGroup();
        GetComponent<GUINavigation>().mouseover = GUI.tooltip;
        GUI.skin.button.fontSize = 24;
        if (!GetComponent<GUINavigation>().usingMouse)
        {
            if (GetComponent<GUINavigation>().keySelect == 0)
                GUI.FocusControl("Return");
            else
                GUI.FocusControl("title");
        }
        else
            GUI.FocusControl("title");
        GUI.skin.button.hover.background = background;
        GUI.skin.button.focused.textColor = inactiveColor;
    }

    void Start()
    {
        maxPages = 3;
        screenNr = 0;
        movedLeft = false;
        movedRight = false;
        firstGUI = true;
    }

    void Update()
    {
        float kh = Input.GetAxisRaw("Horizontal");

        if (kh > 0.01 && !movedRight)
        {
            GetComponent<GUINavigation>().SetNoPlay();
            selectSound.Play();
            screenNr++;
        }
        else if (kh < -0.01 && !movedLeft)
        {
            GetComponent<GUINavigation>().SetNoPlay();
            selectSound.Play();
            screenNr--;
        }
        if (screenNr < 0) screenNr = maxPages-1;
        else if (screenNr > maxPages-1) screenNr = 0;

        if (kh > 0.01)
        {
            movedRight = true;
            movedLeft = false;
        }
        else if (kh < -0.01)
        {
            movedRight = false;
            movedLeft = true;
        }
        else
        {
            movedRight = false;
            movedLeft = false;
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
            Return();
        }
        else
            Menu_Controls();
    }

    public void Return()
    {
        this.enabled = false;
        cancelSound.Play();
        if (mainMenu)
        {
            GetComponent<MainMenuScript>().enabled = true;
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
        else
        {
            GetComponent<PauseMenuScript>().enabled = true;
            GetComponent<GUINavigation>().ClearElements();
            GetComponent<GUINavigation>().maxKeys = 5;
            GetComponent<GUINavigation>().menuKey = 4;
            GetComponent<GUINavigation>().AddElement(0, GetComponent<PauseMenuScript>().Pause_Options);
            GetComponent<GUINavigation>().AddElement(1, GetComponent<PauseMenuScript>().Pause_Controls);
            GetComponent<GUINavigation>().AddElement(2, GetComponent<PauseMenuScript>().Pause_GiveUp);
            GetComponent<GUINavigation>().AddElement(3, GetComponent<PauseMenuScript>().Pause_Quit);
            GetComponent<GUINavigation>().AddElement(4, GetComponent<PauseMenuScript>().Pause_Back);
        }
    }
}
