using UnityEngine;
using System.Collections;

public class PauseMenuScript : MonoBehaviour {

    public GUISkin gSkin;
    public Texture2D backgroundScroll;
    public Vector2 scrollOffset;
    public bool onMap;

    void Menu_Options()
    {

        if (backgroundScroll != null)
        {
            GUI.BeginGroup(new Rect(Screen.width / 2 - backgroundScroll.width / 2 + scrollOffset.x, Screen.height / 2 - backgroundScroll.height / 2 + scrollOffset.y, backgroundScroll.width, backgroundScroll.height));
            GUI.DrawTexture(new Rect(0, 0, backgroundScroll.width, backgroundScroll.height), backgroundScroll);
            GUI.EndGroup();
        }
        if (onMap)
        {
            GUI.BeginGroup(new Rect(Screen.width / 2 - 395, Screen.height / 2 - 3 * 70, 790, 6 * 70));
            //GUI.Box(new Rect(0, 0, 790, 6 * 70), "");
            if (GUI.Button(new Rect(60, 0 * 70, 650, 64), "Options")) { Pause_Options(); }
            if (GUI.Button(new Rect(60, 1 * 70, 650, 64), "Controls")) { }
            if (GUI.Button(new Rect(60, 2 * 70, 650, 64), "Main Menu")) { Pause_MainMenu(); }
            if (GUI.Button(new Rect(60, 3 * 70, 650, 64), "Quit")) { Pause_Quit(); }
            if (GUI.Button(new Rect(60, 5 * 70, 650, 64), "Return")) { Pause_Back(); }
        }
        else
        {
            GUI.BeginGroup(new Rect(Screen.width / 2 - 395, Screen.height / 2 - 3.5f * 70, 790, 7 * 70));
            //GUI.Box(new Rect(0, 0, 790, 7 * 70), "");
            if (GUI.Button(new Rect(60, 0 * 70, 650, 64), "Options")) { Pause_Options(); }
            if (GUI.Button(new Rect(60, 1 * 70, 650, 64), "Controls")) { }
            if (GUI.Button(new Rect(60, 2 * 70, 650, 64), "Map")) { Pause_Map(); }
            if (GUI.Button(new Rect(60, 3 * 70, 650, 64), "Restart")) { Pause_Restart(); }
            if (GUI.Button(new Rect(60, 4 * 70, 650, 64), "Quit")) { Pause_Quit(); }
            if (GUI.Button(new Rect(60, 6 * 70, 650, 64), "Return")) { Pause_Back(); }
        }
        GUI.EndGroup();
    }

    void OnGUI()
    {
        GUI.skin = gSkin;
        if (Input.GetKeyDown(KeyCode.Escape))
            Pause_Back();
        else
            Menu_Options();
    }

    void Pause_Restart()
    {
        this.enabled = false;
        GetComponent<PauseReturnToScript>().restart = true;
        GetComponent<PauseReturnToScript>().enabled = true;
    }

    void Pause_Map()
    {
        this.enabled = false;
        GetComponent<PauseReturnToScript>().enabled = true;
    }

    void Pause_MainMenu()
    {
        this.enabled = false;
        GetComponent<PauseReturnToScript>().onMap = true;
        GetComponent<PauseReturnToScript>().enabled = true;
    }

    void Pause_Quit()
    {
        this.enabled = false;
        GetComponent<PauseReturnToScript>().quit = true;
        GetComponent<PauseReturnToScript>().enabled = true;
    }

    void Pause_Back()
    {
        this.enabled = false;
        if (onMap)
        {
            GetComponent<MapGui>().enabled = true;
            transform.parent.gameObject.GetComponent<MapMovementController>().enabled = true;
        }
        else
        {
            Time.timeScale = 1; // TODO: Set SLOWDOWN!
            GetComponent<GUIScript>().enabled = true;
        }
    }

    void Pause_Options()
    {
        this.enabled = false;
        //if (onMap)
        //{
            GetComponent<OptionsMenuScript>().enabled = true;
        //}
    }
}
