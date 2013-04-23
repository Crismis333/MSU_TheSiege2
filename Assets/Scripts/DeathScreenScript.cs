using UnityEngine;
using System.Collections;

public class DeathScreenScript : MonoBehaviour {

    public GUISkin gSkin;
    public Texture2D backgroundScroll;
    public Vector2 scrollOffset;
    public Texture2D backgroundScroll2;
    public Vector2 scrollOffset2;

    public Texture2D black;
    private long score;
    private long newscore;
    private float countdown;
    private bool returned, gaveup, decreaseComplete, started;
    private long pointsDecreaser;

    private Texture2D background;
    private Color activeColor, inactiveColor;
    private bool firstGUI;

    void Menu_HighScore()
    {
        if (!Camera.mainCamera.GetComponent<GUINavigation>().usingMouse)
            GUI.skin.button.hover.background = null;
        else
            GUI.skin.button.hover.background = background;
        if (Camera.mainCamera.GetComponent<GUINavigation>().activated)
            GUI.skin.button.focused.textColor = activeColor;
        else
            GUI.skin.button.focused.textColor = inactiveColor;
        if (backgroundScroll2 != null)
        {
            GUI.BeginGroup(new Rect(Screen.width / 2 - backgroundScroll2.width / 2 + scrollOffset.x, Screen.height / 2 - backgroundScroll2.height / 2 + scrollOffset.y, backgroundScroll2.width, backgroundScroll2.height));
            GUI.DrawTexture(new Rect(0, 0, backgroundScroll2.width, backgroundScroll2.height), backgroundScroll2);
            GUI.EndGroup();
        }
        GUI.BeginGroup(new Rect(Screen.width / 2 - 395, Screen.height / 2 - 5 * 35, 790, 10 * 35));
        //GUI.Box(new Rect(0, 0, 790, 15 * 35), "");
        GUI.color = Color.black;
        if (CurrentGameState.highscorecondition == EndState.GaveUp)
            GUI.Label(new Rect(0, 1 * 35, 790, 64), "Quitters can never be winners.");
        else
            GUI.Label(new Rect(0, 1 * 35, 790, 64), "Your army decided to run ahead of you. Slowpoke.");
        GUI.SetNextControlName("title");
        GUI.Label(new Rect(0, 2 * 35, 790, 64), "Your points have beeen halved.");

        GUI.Label(new Rect(0, 4 * 35, 790, 64), "Current score      ");
        GUI.color = Color.red;
        GUI.Label(new Rect(0, 4 * 35, 790, 64), "                      " + score);
        GUI.color = Color.white;
        GUI.SetNextControlName("Return");
        if (GUI.Button(new Rect(60, 6 * 35, 700, 64), "Return to camp (cut points)")) { ReturnToCamp(); }
        GUI.SetNextControlName("GiveUp");
        if (GUI.Button(new Rect(60, 8 * 35, 700, 64), "Give up")) { GiveUp(); }

        GUI.Box(new Rect(60, 6 * 35, 700, 64), new GUIContent("", "0"));
        GUI.Box(new Rect(60, 8 * 35, 700, 64), new GUIContent("", "1"));
        Camera.mainCamera.GetComponent<GUINavigation>().mouseover = GUI.tooltip;

        GUI.EndGroup();
        if (started)
        {
            GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
            GUI.color = new Color(1, 1, 1, Mathf.Lerp(1, 0, 1 - countdown));
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), black);
            GUI.EndGroup();
        }
        else if (gaveup || returned)
        {
            GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
            GUI.color = new Color(1, 1, 1, Mathf.Lerp(0, 1, 1 - countdown));
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), black);
            GUI.EndGroup();
        }

        GUI.skin.button.hover.background = background;
        GUI.skin.button.focused.textColor = inactiveColor;

        if (!Camera.mainCamera.GetComponent<GUINavigation>().usingMouse)
        {
            switch (Camera.mainCamera.GetComponent<GUINavigation>().keySelect)
            {
                case 0: GUI.FocusControl("Return"); break;
                case 1: GUI.FocusControl("GiveUp"); break;
            }
        }
        else
            GUI.FocusControl("title");
    }

    void Start()
    {
        firstGUI = true;
        started = true;
        countdown = 1.2f;
        returned = false;
        gaveup = false;
        decreaseComplete = false;

        score = CurrentGameState.previousScore;
        //score = 5000000;
        newscore = score / 2;
        pointsDecreaser = newscore / 59L;
        
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
            Camera.mainCamera.GetComponent<GUINavigation>().ClearElements();
            Camera.mainCamera.GetComponent<GUINavigation>().maxKeys = 2;
            Camera.mainCamera.GetComponent<GUINavigation>().AddElement(0, ReturnToCamp);
            Camera.mainCamera.GetComponent<GUINavigation>().AddElement(1, GiveUp);
        }
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    Return();
        //}
        //else
        Menu_HighScore();
    }

    public void GiveUp()
    {
        if (!started && !gaveup)
        {
            gaveup = true;
            countdown = 1.2f;
        }
    }

    public void ReturnToCamp()
    {
        if (!started && !gaveup)
        {
            returned = true;
            countdown = 2f;
        }
    }

    void Update()
    {
        if (started)
        {
            countdown -= 0.02f;
            if (countdown <= 0)
                started = false;
        }
        if (returned) {
            if (!decreaseComplete)
            {
                score -= pointsDecreaser;
                if (score <= newscore)
                {
                    score = newscore;
                    decreaseComplete = true;
                }
            }
            else
            {
                countdown -= 0.02f;
                if (countdown <= 0)
                {
                    CurrentGameState.previousPosition = CurrentGameState.previousPreviousPosition;
                    CurrentGameState.currentScore = newscore;
                    CurrentGameState.failedlast = true;
                    Application.LoadLevel(1);
                }
            }
        }
        else if (gaveup)
        {
            countdown -= 0.02f;
            if (countdown <= 0)
            {
                CurrentGameState.highscorecondition = EndState.Lost;
                Application.LoadLevel(3);
            }
        }
    }
}
