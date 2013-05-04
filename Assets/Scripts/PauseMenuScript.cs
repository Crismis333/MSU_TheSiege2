using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GUINavigation))]
[RequireComponent(typeof(PauseReturnToScript))]
[RequireComponent(typeof(OptionsMenuScript))]
[RequireComponent(typeof(ControlsScript))]
public class PauseMenuScript : MonoBehaviour {

    public GUISkin gSkin;
    public Texture2D backgroundScroll;
    public Vector2 scrollOffset;
    public bool onMap;
    public EffectVolumeSetter cancelSound;
    public EffectVolumeSetter selectSound;

    private Texture2D background;
    private Color activeColor, inactiveColor;
    private bool firstGUI;

    private GUINavigation guin;
    private PauseReturnToScript prts;

    void Menu_Options()
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
        GUI.BeginGroup(new Rect(Screen.width / 2 - 395, Screen.height / 2 - 3 * 70, 790, 6 * 70));
            //GUI.BeginGroup(new Rect(Screen.width / 2 - 395, Screen.height / 2 - 3.5f * 70, 790, 7 * 70));
        GUI.SetNextControlName("title");
        GUI.Box(new Rect(0, 0, 790, 7 * 70), "");
        GUI.SetNextControlName("Options");
        if (GUI.Button(new Rect(60, 0 * 70, 650, 64), "Options")) { Pause_Options(); }
        GUI.SetNextControlName("Controls");
        if (GUI.Button(new Rect(60, 1 * 70, 650, 64), "Controls")) { Pause_Controls(); }
        GUI.SetNextControlName("GiveUp");
        if (GUI.Button(new Rect(60, 2 * 70, 650, 64), "Give up")) { Pause_GiveUp(); }
        GUI.SetNextControlName("Quit");
        if (GUI.Button(new Rect(60, 3 * 70, 650, 64), "Quit")) { Pause_Quit(); }
        GUI.SetNextControlName("Return");
        if (GUI.Button(new Rect(60, 5 * 70, 650, 64), "Return")) { Pause_Back(); }

        GUI.Box(new Rect(60, 0 * 70, 650, 64), new GUIContent("", "0"));
        GUI.Box(new Rect(60, 1 * 70, 650, 64), new GUIContent("", "1"));
        GUI.Box(new Rect(60, 2 * 70, 650, 64), new GUIContent("", "2"));
        GUI.Box(new Rect(60, 3 * 70, 650, 64), new GUIContent("", "3"));
        GUI.Box(new Rect(60, 5 * 70, 650, 64), new GUIContent("", "4"));
        guin.mouseover = GUI.tooltip;

        GUI.EndGroup();

        GUI.skin.button.hover.background = background;
        GUI.skin.button.focused.textColor = inactiveColor;

        if (!guin.usingMouse)
            switch (guin.keySelect)
            {
                case 0: GUI.FocusControl("Options"); break;
                case 1: GUI.FocusControl("Controls"); break;
                case 2: GUI.FocusControl("GiveUp"); break;
                case 3: GUI.FocusControl("Quit"); break;
                case 4: GUI.FocusControl("Return"); break;
                default: GUI.FocusControl("title"); break;
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
            Pause_Back();
        else
            Menu_Options();
    }

    public void Pause_GiveUp()
    {
        selectSound.Play();
        this.enabled = false;
        prts.onMap = onMap;
        prts.enabled = true;
        guin.ClearElements();
        guin.maxKeys = 2;
        guin.AddElement(0, prts.Return_Yes);
        guin.AddElement(1, prts.Return_No);
    }

    public void Pause_Quit()
    {
        selectSound.Play();
        this.enabled = false;
        prts.quit = true;
        prts.enabled = true;
        guin.ClearElements();
        guin.maxKeys = 2;
        guin.AddElement(0, prts.Return_Yes);
        guin.AddElement(1, prts.Return_No);
    }

    public void Pause_Back()
    {
        this.enabled = false;
        cancelSound.Play();
        if (onMap)
        {
            GetComponent<MapGui>().enabled = true;
            transform.parent.gameObject.GetComponent<MapMovementController>().enabled = true;

        }
        else
        {
            Time.timeScale = 1;
            GetComponent<GUIScript>().enabled = true;
        }
        guin.ClearElements();
        guin.maxKeys = 0;
    }

    public void Pause_Controls()
    {
        this.enabled = false;
        selectSound.Play();
        GetComponent<ControlsScript>().enabled = true;
        guin.ClearElements();
        guin.maxKeys = 1;
        guin.menuKey = 0;
        guin.AddElement(0, GetComponent<ControlsScript>().Return);
    }

    public void Pause_Options()
    {
        this.enabled = false;
        selectSound.Play();
        GetComponent<OptionsMenuScript>().enabled = true;
        guin.ClearElements();
        guin.maxKeys = 6;
        guin.AddElement(5, GetComponent<OptionsMenuScript>().Menu_Options_Back);
    }

    void Start()
    {
        firstGUI = true;
        guin = GetComponent<GUINavigation>();
        prts = GetComponent<PauseReturnToScript>();
    }
}
