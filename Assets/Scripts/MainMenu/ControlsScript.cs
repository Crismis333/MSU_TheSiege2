using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GUINavigation))]
public class ControlsScript : MonoBehaviour {

    public GUISkin gSkin;
    public Texture2D backgroundScroll, xBoxController, keyboardController;
    public Texture2D attackBar, chargeOrb;
    public Vector2 scrollOffset;
    public bool mainMenu;
    public EffectVolumeSetter cancelSound, selectSound, switchSound;

    [Multiline]
    public string howToAttack, howToCharge, howToLose;

    private Texture2D background;
    private Color activeColor, inactiveColor;
    private bool firstGUI;
    private bool movedLeft, movedRight;
    private int screenNr, maxPages;

    private GUINavigation guin;

    void Menu_Controls()
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

        GUI.BeginGroup(new Rect(0,0, Screen.width, Screen.height));
        GUI.color = Color.white;


        if (screenNr == 0)
        {
            GUI.DrawTexture(new Rect(Screen.width / 2 - attackBar.width / 2 + scrollOffset.x, Screen.height / 2 - attackBar.height / 2 + scrollOffset.y + 100, attackBar.width, attackBar.height), attackBar);
        }
        else if (screenNr == 1)
        {
            GUI.DrawTexture(new Rect(Screen.width / 2 - chargeOrb.width / 2 + scrollOffset.x + 200, Screen.height / 2 - chargeOrb.height / 2 + scrollOffset.y + 75, chargeOrb.width, chargeOrb.height), chargeOrb);
        }
        else if (screenNr == 3)
        {
            GUI.DrawTexture(new Rect(Screen.width / 2 - xBoxController.width / 2 + scrollOffset.x, Screen.height / 2 - xBoxController.height / 2 + scrollOffset.y, xBoxController.width, xBoxController.height), xBoxController);
        }
        else if (screenNr == 4)
        {
            GUI.DrawTexture(new Rect(Screen.width / 2 - keyboardController.width / 2 + scrollOffset.x, Screen.height / 2 - keyboardController.height / 2 + scrollOffset.y, keyboardController.width, keyboardController.height), keyboardController);
        }

        GUI.EndGroup();

        GUI.BeginGroup(new Rect(Screen.width / 2 - 395, Screen.height / 2 - 7.5f * 35, 790, 15 * 35));
        GUI.color = Color.white;

        GUI.SetNextControlName("title");
        GUI.Label(new Rect(90, 0 * 30 + 10, 790, 64), "Instructions page " + (screenNr+1) + "/" + maxPages);

        TextAnchor t = GUI.skin.label.alignment;
        GUI.skin.label.alignment = TextAnchor.UpperLeft;
        GUI.skin.label.fontSize = 18;
        if (screenNr == 0)
        {
            GUI.Label(new Rect(15 + 30, 2 * 35 + 10, 640, 640), howToAttack);
        }
        else if (screenNr == 1)
        {
            GUI.Label(new Rect(15 + 30, 2 * 35 + 10, 640, 640), howToCharge);
        }
        else if (screenNr == 2)
        {
            GUI.Label(new Rect(15 + 30, 2 * 35 + 10, 640, 640), howToLose);
        }
        GUI.skin.label.alignment = t;
        GUI.skin.label.fontSize = 20;
        GUI.color = Color.white;
        GUI.SetNextControlName("Return");
        if (GUI.Button(new Rect(60, 12 * 35, 640, 64), "Return")) { Return(); }
        GUI.Box(new Rect(60, 12 * 35, 640, 64), new GUIContent("", "0"));
        guin.mouseover = GUI.tooltip;

        GUI.EndGroup();

        GUI.skin.button.fontSize = 40;
            //GUI.BeginGroup(new Rect(Screen.width / 2 - 395, Screen.height / 2 - 5 * 35, 790, 10 * 35));
        GUI.BeginGroup(new Rect(Screen.width / 2 - 435, Screen.height / 2 - 7.5f * 35, 155, 15 * 35));
        if (GUI.Button(new Rect(0, 6 * 30, 155, 100), "<"))
        {
            screenNr--;
            guin.SetNoPlay();
            switchSound.Play();
            if (screenNr < 0) screenNr = maxPages-1;
        }
        GUI.Box(new Rect(0, 6 * 30, 155, 100), new GUIContent("", "left"));
        GUI.EndGroup();

        GUI.BeginGroup(new Rect(Screen.width / 2 + 280, Screen.height / 2 - 7.5f * 35, 155, 15 * 35));
        if (GUI.Button(new Rect(0, 6 * 30, 155, 100), ">"))
        {
            screenNr++;
            guin.SetNoPlay();
            switchSound.Play();
            if (screenNr > maxPages-1) screenNr = 0;
        }
        GUI.Box(new Rect(0, 6 * 30, 155, 100), new GUIContent("", "right"));
        GUI.EndGroup();
        guin.mouseover = GUI.tooltip;
        GUI.skin.button.fontSize = 24;
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
        maxPages = 5;
        screenNr = 0;
        movedLeft = false;
        movedRight = false;
        firstGUI = true;
        guin = GetComponent<GUINavigation>();
    }

    void Update()
    {
        float kh = Input.GetAxisRaw("Horizontal");
        float kh2 = Input.GetAxisRaw("DPadHorizontal");

        if ((kh > 0.2 || kh2 > 0.2) && !movedRight)
        {
            guin.SetNoPlay();
            switchSound.Play();
            screenNr++;
        }
        else if ((kh < -0.2 || kh < -0.2) && !movedLeft)
        {
            guin.SetNoPlay();
            switchSound.Play();
            screenNr--;
        }
        if (screenNr < 0) screenNr = maxPages-1;
        else if (screenNr > maxPages-1) screenNr = 0;

        if (kh > 0.2 || kh2 > 0.2)
        {
            movedRight = true;
            movedLeft = false;
        }
        else if (kh < -0.2 || kh2 < -0.2)
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
        if (guin.QuitPressed())
            Return();
        else
            Menu_Controls();
    }

    public void Return()
    {
        this.enabled = false;
        cancelSound.Play();
        if (mainMenu)
        {
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
        else
        {
            PauseMenuScript pms = GetComponent<PauseMenuScript>();
            pms.enabled = true;
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
}
