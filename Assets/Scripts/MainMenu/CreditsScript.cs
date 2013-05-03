using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GUINavigation))]
public class CreditsScript : MonoBehaviour {

    public GUISkin gSkin;
    public Texture2D backgroundScroll,textHiderScroll;
    public Vector2 scrollOffset;
    public EffectVolumeSetter cancelSound;
    [Multiline]
    public string credits;
    private int scroll;

    private Texture2D background;
    private Color activeColor, inactiveColor;
    private bool firstGUI;

    private GUINavigation guin;
    private MainMenuScript mms;
    void Menu_Credits()
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
        GUI.BeginGroup(new Rect(Screen.width / 2 - 395, Screen.height / 2 - 7.5f * 35+80, 790, 370));
        GUI.color = Color.black;
        GUI.SetNextControlName("title");
        GUI.skin.label.alignment = TextAnchor.UpperLeft;
        GUI.Label(new Rect(60, 400 - scroll, 790, 100000), credits);
        GUI.skin.label.alignment = TextAnchor.MiddleLeft;
        GUI.color = Color.white;
        GUI.EndGroup();
        if (textHiderScroll != null)
        {
            GUI.BeginGroup(new Rect(Screen.width / 2 - textHiderScroll.width / 2 + scrollOffset.x, Screen.height / 2 - textHiderScroll.height / 2 + scrollOffset.y, textHiderScroll.width, textHiderScroll.height));
            GUI.DrawTexture(new Rect(0, 0, textHiderScroll.width, textHiderScroll.height), textHiderScroll);

            GUI.EndGroup();
        }

        GUI.BeginGroup(new Rect(Screen.width / 2 - 395, Screen.height / 2 - 7.5f * 35, 790, 15 * 35));
        GUI.SetNextControlName("Return");
        if (GUI.Button(new Rect(60, 12 * 35, 640, 64), "Return")) 
            Return();
        GUI.Box(new Rect(60, 12 * 35, 640, 64), new GUIContent("", "0"));
        guin.mouseover = GUI.tooltip;

        GUI.EndGroup();

        if (!guin.usingMouse)
        {
            if (guin.keySelect == 0)
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
        firstGUI = true;
        scroll = 0;
        guin = GetComponent<GUINavigation>();
        mms = GetComponent<MainMenuScript>();
    }

    void Update()
    {
        scroll += 1;
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
            Menu_Credits();
    }

    public void Return()
    {
        cancelSound.Play();
        this.enabled = false;
        scroll = 0;
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
}
