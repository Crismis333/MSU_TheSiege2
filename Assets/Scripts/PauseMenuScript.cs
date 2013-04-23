using UnityEngine;
using System.Collections;

public class PauseMenuScript : MonoBehaviour {

    public GUISkin gSkin;
    public Texture2D backgroundScroll;
    public Vector2 scrollOffset;
    public bool onMap;

    private Texture2D background;
    private Color activeColor, inactiveColor;
    private bool firstGUI;

    void Menu_Options()
    {
        if (!Camera.mainCamera.GetComponent<GUINavigation>().usingMouse)
            GUI.skin.button.hover.background = null;
        else
            GUI.skin.button.hover.background = background;
        if (Camera.mainCamera.GetComponent<GUINavigation>().activated)
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
        Camera.mainCamera.GetComponent<GUINavigation>().mouseover = GUI.tooltip;

        GUI.EndGroup();

        GUI.skin.button.hover.background = background;
        GUI.skin.button.focused.textColor = inactiveColor;

        if (!Camera.mainCamera.GetComponent<GUINavigation>().usingMouse)
        {
            switch (Camera.mainCamera.GetComponent<GUINavigation>().keySelect)
            {
                case 0: GUI.FocusControl("Options"); break;
                case 1: GUI.FocusControl("Controls"); break;
                case 2: GUI.FocusControl("GiveUp"); break;
                case 3: GUI.FocusControl("Quit"); break;
                case 4: GUI.FocusControl("Return"); break;
            }
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
        if (Input.GetKeyDown(KeyCode.Escape))
            Pause_Back();
        else
            Menu_Options();
    }

    /* 
    void Pause_Restart()
    {
        this.enabled = false;
        GetComponent<PauseReturnToScript>().restart = true;
        GetComponent<PauseReturnToScript>().enabled = true;
    }
    */ 

    public void Pause_GiveUp()
    {
        this.enabled = false;
        GetComponent<PauseReturnToScript>().onMap = onMap;
        GetComponent<PauseReturnToScript>().enabled = true;
        Camera.mainCamera.GetComponent<GUINavigation>().ClearElements();
        Camera.mainCamera.GetComponent<GUINavigation>().maxKeys = 2;
        Camera.mainCamera.GetComponent<GUINavigation>().AddElement(0, GetComponent<PauseReturnToScript>().Return_Yes);
        Camera.mainCamera.GetComponent<GUINavigation>().AddElement(1, GetComponent<PauseReturnToScript>().Return_No);
    }
    /*
    void Pause_MainMenu()
    {
        this.enabled = false;
        GetComponent<PauseReturnToScript>().onMap = true;
        GetComponent<PauseReturnToScript>().enabled = true;
    }
    */
    public void Pause_Quit()
    {
        this.enabled = false;
        GetComponent<PauseReturnToScript>().quit = true;
        GetComponent<PauseReturnToScript>().enabled = true;
        Camera.mainCamera.GetComponent<GUINavigation>().ClearElements();
        Camera.mainCamera.GetComponent<GUINavigation>().maxKeys = 2;
        Camera.mainCamera.GetComponent<GUINavigation>().AddElement(0, GetComponent<PauseReturnToScript>().Return_Yes);
        Camera.mainCamera.GetComponent<GUINavigation>().AddElement(1, GetComponent<PauseReturnToScript>().Return_No);
    }

    public void Pause_Back()
    {
        this.enabled = false;
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
        Camera.mainCamera.GetComponent<GUINavigation>().ClearElements();
        Camera.mainCamera.GetComponent<GUINavigation>().maxKeys = 0;
    }

    public void Pause_Controls()
    {
    }

    public void Pause_Options()
    {
        this.enabled = false;
        GetComponent<OptionsMenuScript>().enabled = true;
        Camera.mainCamera.GetComponent<GUINavigation>().ClearElements();
        Camera.mainCamera.GetComponent<GUINavigation>().maxKeys = 6;
        Camera.mainCamera.GetComponent<GUINavigation>().AddElement(5, GetComponent<OptionsMenuScript>().Menu_Options_Back);
    }

    void Start()
    {
        firstGUI = true; 
    }
}
