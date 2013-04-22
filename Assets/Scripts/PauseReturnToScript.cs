using UnityEngine;
using System.Collections;

public class PauseReturnToScript : MonoBehaviour {

    public GUISkin gSkin;
    public Texture2D backgroundScroll;
    public Texture2D black;
    public Vector2 scrollOffset;
    [HideInInspector]
    public bool onMap, quit, restart;
    private float countdown;
    private bool ended;

    void Return_Accept()
    {
        if (backgroundScroll != null)
        {
            GUI.BeginGroup(new Rect(Screen.width / 2 - backgroundScroll.width / 2 + scrollOffset.x, Screen.height / 2 - backgroundScroll.height / 2 + scrollOffset.y, backgroundScroll.width, backgroundScroll.height));
            GUI.DrawTexture(new Rect(0, 0, backgroundScroll.width, backgroundScroll.height), backgroundScroll);
            GUI.EndGroup();
        }
        GUI.BeginGroup(new Rect(Screen.width / 2 - 395, Screen.height / 2 - 2.5f * 70, 790, 5 * 70));

        //GUI.Box(new Rect(0, 0, 790, 5 * 70), "");
        GUI.color = Color.black;
        if (quit)
            GUI.Label(new Rect(0, 1 * 70, 790, 64), "Do you really wish to quit? All progress will be lost.");
        else if (onMap)
            GUI.Label(new Rect(0, 1 * 70, 790, 64), "Return to main menu? All progress will be lost.");
        else
            GUI.Label(new Rect(0, 1 * 70, 790, 64), "Give up on this level?");
        GUI.color = Color.white;
        if (GUI.Button(new Rect(60, 3 * 70, 730, 64), "Yes")) { Return_Yes(); }
        if (GUI.Button(new Rect(60, 4 * 70, 730, 64), "No")) { Return_No();  }
        GUI.EndGroup();
        if (ended)
        {
            GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
            GUI.color = new Color(1, 1, 1, Mathf.Lerp(0, 1, 1 - countdown));
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), black);
            GUI.EndGroup();
        }
    }

    void OnGUI()
    {
        GUI.skin = gSkin;
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Return_No();
        }
        else
            Return_Accept();
    }

    void Return_No()
    {
        if (!ended)
        {
            quit = onMap = restart = false;
            this.enabled = false;
            GetComponent<PauseMenuScript>().enabled = true;
        }
    }

    void Return_Yes()
    {
        //this.enabled = false;

        
        ended = true;
        countdown = 1.0f;

    }

    void Update()
    {
        if (ended)
        {
            countdown -= 0.02f;
            if (countdown <= 0)
            {
                Time.timeScale = 1;
                //if (restart)
                //{
                //    Application.LoadLevel(Application.loadedLevel);
                //}
                if (quit)
                {
                    CurrentGameState.Restart();
                    Application.Quit();
                }
                else if (onMap)
                {
                    CurrentGameState.Restart();
                    Application.LoadLevel(0);
                }
                else
                {
                    // Wins, doesn't reset. Change when game is final
                    //CurrentGameState.previousPosition = CurrentGameState.previousPreviousPosition;
                    //CurrentGameState.currentScore = CurrentGameState.previousScore;
                    //CurrentGameState.SetWin();
                    //Application.LoadLevel(1);
                    Application.LoadLevel(4);
                    CurrentGameState.highscorecondition = EndState.GaveUp;
                }
            }
        }
    }
}
