using UnityEngine;
using System.Collections;

public class DeathScreenScript : MonoBehaviour {

    public GUISkin gSkin;
    public Texture2D backgroundScroll;
    public Vector2 scrollOffset;
    public Texture2D backgroundScroll2;
    public Vector2 scrollOffset2;
    public int pointsDecreaser;
    public Texture2D black;
    private long score;
    private long newscore;
    private float countdown;
    private bool returned, gaveup, decreaseComplete, started;

    void Menu_HighScore()
    {
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
            GUI.Label(new Rect(0, 2 * 35, 790, 64), "Your points have beeen halved.");

            GUI.Label(new Rect(0, 4 * 35, 790, 64), "Current score      ");
            GUI.color = Color.red;
            GUI.Label(new Rect(0, 4 * 35, 790, 64), "                      " + score);
            GUI.color = Color.white;
            if (GUI.Button(new Rect(60, 6 * 35, 700, 64), "Return to camp (cut points)")) { ReturnToCamp(); }
            if (GUI.Button(new Rect(60, 8 * 35, 700, 64), "Give up")) { GiveUp(); }
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
    }

    void Start()
    {
        started = true;
        countdown = 1.2f;
        returned = false;
        gaveup = false;
        decreaseComplete = false;

        score = CurrentGameState.previousScore;
        //score = 5000000;
        newscore = score / 2;
        
    }

    void OnGUI()
    {
        GUI.skin = gSkin;

        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    Return();
        //}
        //else
        Menu_HighScore();
    }

    void GiveUp()
    {
        if (!started && !gaveup)
        {
            gaveup = true;
            countdown = 1.2f;
        }
    }

    void ReturnToCamp()
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
                if (score < newscore)
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
