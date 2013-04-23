using UnityEngine;
using System.Collections;

public class QuitAcceptMenu : MonoBehaviour {

    public GUISkin gSkin;

    private Texture2D background;
    private Color activeColor, inactiveColor;
    private bool firstGUI;

    void Menu_Quit()
    {
        if (!Camera.mainCamera.GetComponent<GUINavigation>().usingMouse)
            GUI.skin.button.hover.background = null;
        else
            GUI.skin.button.hover.background = background;
        if (Camera.mainCamera.GetComponent<GUINavigation>().activated)
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
        Camera.mainCamera.GetComponent<GUINavigation>().mouseover = GUI.tooltip;

        GUI.EndGroup();

        GUI.skin.button.hover.background = background;
        GUI.skin.button.focused.textColor = inactiveColor;

        if (!Camera.mainCamera.GetComponent<GUINavigation>().usingMouse)
        {
            switch (Camera.mainCamera.GetComponent<GUINavigation>().keySelect)
            {
                case 0: GUI.FocusControl("Yes"); break;
                case 1: GUI.FocusControl("No"); break;
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
        {
            Menu_Quit_No();
        }
        else
            Menu_Quit();
    }

    public void Menu_Quit_Yes()
    {
        Application.Quit();
    }

    public void Menu_Quit_No()
    {
        this.enabled = false;
        GetComponent<MainMenuScript>().enabled = true;
        Camera.mainCamera.GetComponent<GUINavigation>().ClearElements();
        Camera.mainCamera.GetComponent<GUINavigation>().maxKeys = 4;
        Camera.mainCamera.GetComponent<GUINavigation>().AddElement(0, GetComponent<MainMenuScript>().Menu_Main_Start_Game);
        Camera.mainCamera.GetComponent<GUINavigation>().AddElement(1, GetComponent<MainMenuScript>().Menu_Main_Options);
        Camera.mainCamera.GetComponent<GUINavigation>().AddElement(2, GetComponent<MainMenuScript>().Menu_Main_Highscores);
        Camera.mainCamera.GetComponent<GUINavigation>().AddElement(3, GetComponent<MainMenuScript>().Menu_Main_Quit);
    }

    void Start()
    {
        firstGUI = true;
    }
}
