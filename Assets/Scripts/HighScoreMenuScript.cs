using UnityEngine;
using System.Collections;

public class HighScoreMenuScript : MonoBehaviour {

    public GUISkin gSkin;
    public Texture2D backgroundScroll;
    public Vector2 scrollOffset;
    public Texture2D backgroundScroll2;
    public Vector2 scrollOffset2;
    public Texture2D black;
    public bool mainMenu;
    private bool addnewScore, started, returned;
    private string setname;
    private float countdown;

    void Menu_HighScore()
    {
        if (addnewScore)
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
            switch (CurrentGameState.highscorecondition) {
                case EndState.Won:
                    {
                        GUI.Label(new Rect(0, 1 * 35, 790, 64), "You have made a high score! You are truly");
                        GUI.Label(new Rect(0, 2 * 35, 790, 64), "a great barbarian! Conglaturations!");
                        break;
                    }
                case EndState.Lost:
                    {
                        GUI.Label(new Rect(0, 1 * 35, 790, 64), "You made the high score. Still,");
                        GUI.Label(new Rect(0, 2 * 35, 790, 64), "try to be less bad next time.");
                        break;
                    }
                case EndState.GaveUp:
                    {
                        GUI.Label(new Rect(0, 1 * 35, 790, 64), "A true barbarian never gives up.");
                        GUI.Label(new Rect(0, 2 * 35, 790, 64), "But you still made the high score.");
                        break;
                    }
            }

            GUI.Label(new Rect(0, 4 * 35, 790, 64), "Final score      ");
            GUI.Label(new Rect(0, 6 * 35, 790, 64), "Name: ");
            GUI.color = Color.red;
            GUI.Label(new Rect(0, 4 * 35, 790, 64), "                      " + CurrentGameState.previousScore);
            GUI.color = Color.white;
            setname = GUI.TextField(new Rect(170, 6 * 35, 540, 64), setname, 10);
            if (GUI.Button(new Rect(60, 8 * 35, 700, 64), "Add score")) { Add_Score(); }
            GUI.EndGroup();
        }
        else
        {
            if (backgroundScroll != null)
            {
                GUI.BeginGroup(new Rect(Screen.width / 2 - backgroundScroll.width / 2 + scrollOffset.x, Screen.height / 2 - backgroundScroll.height / 2 + scrollOffset.y, backgroundScroll.width, backgroundScroll.height));
                GUI.DrawTexture(new Rect(0, 0, backgroundScroll.width, backgroundScroll.height), backgroundScroll);
                GUI.EndGroup();
            }
            GUI.BeginGroup(new Rect(Screen.width / 2 - 395, Screen.height / 2 - 7.5f * 35, 790, 15 * 35));
            GUI.color = Color.black;
            GUI.Label(new Rect(0, 1 * 35, 790, 64), "I");
            GUI.Label(new Rect(0, 2 * 35, 790, 64), "II");
            GUI.Label(new Rect(0, 3 * 35, 790, 64), "III");
            GUI.Label(new Rect(0, 4 * 35, 790, 64), "IV");
            GUI.Label(new Rect(0, 5 * 35, 790, 64), "V");
            GUI.Label(new Rect(0, 6 * 35, 790, 64), "VI");
            GUI.Label(new Rect(0, 7 * 35, 790, 64), "VII");
            GUI.Label(new Rect(0, 8 * 35, 790, 64), "VIII");
            GUI.Label(new Rect(0, 9 * 35, 790, 64), "IX");
            GUI.Label(new Rect(0, 10 * 35, 790, 64), "X");
            GUI.color = Color.red;

            GUI.Label(new Rect(50, 1 * 35, 790, 64), "  " + PlayerPrefs.GetString("Highscore1Name"));
            GUI.Label(new Rect(400, 1 * 35, 790, 64), PlayerPrefs.GetString("Highscore1Score"));
            GUI.Label(new Rect(50, 2 * 35, 790, 64), "  " + PlayerPrefs.GetString("Highscore2Name"));
            GUI.Label(new Rect(400, 2 * 35, 790, 64), PlayerPrefs.GetString("Highscore2Score"));
            GUI.Label(new Rect(50, 3 * 35, 790, 64), "  " + PlayerPrefs.GetString("Highscore3Name"));
            GUI.Label(new Rect(400, 3 * 35, 790, 64), PlayerPrefs.GetString("Highscore3Score"));
            GUI.Label(new Rect(50, 4 * 35, 790, 64), "  " + PlayerPrefs.GetString("Highscore4Name"));
            GUI.Label(new Rect(400, 4 * 35, 790, 64), PlayerPrefs.GetString("Highscore4Score"));
            GUI.Label(new Rect(50, 5 * 35, 790, 64), "  " + PlayerPrefs.GetString("Highscore5Name"));
            GUI.Label(new Rect(400, 5 * 35, 790, 64), PlayerPrefs.GetString("Highscore5Score"));
            GUI.Label(new Rect(50, 6 * 35, 790, 64), "  " + PlayerPrefs.GetString("Highscore6Name"));
            GUI.Label(new Rect(400, 6 * 35, 790, 64), PlayerPrefs.GetString("Highscore6Score"));
            GUI.Label(new Rect(50, 7 * 35, 790, 64), "  " + PlayerPrefs.GetString("Highscore7Name"));
            GUI.Label(new Rect(400, 7 * 35, 790, 64), PlayerPrefs.GetString("Highscore7Score"));
            GUI.Label(new Rect(50, 8 * 35, 790, 64), "  " + PlayerPrefs.GetString("Highscore8Name"));
            GUI.Label(new Rect(400, 8 * 35, 790, 64), PlayerPrefs.GetString("Highscore8Score"));
            GUI.Label(new Rect(50, 9 * 35, 790, 64), "  " + PlayerPrefs.GetString("Highscore9Name"));
            GUI.Label(new Rect(400, 9 * 35, 790, 64), PlayerPrefs.GetString("Highscore9Score"));
            GUI.Label(new Rect(50, 10 * 35, 790, 64), "  " + PlayerPrefs.GetString("Highscore10Name"));
            GUI.Label(new Rect(400, 10 * 35, 790, 64), PlayerPrefs.GetString("Highscore10Score"));
            GUI.color = Color.white;
            if (GUI.Button(new Rect(60, 12 * 35, 640, 64), "Return")) { Return(); }
            GUI.EndGroup();
        }
        if (started)
        {
            GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
            GUI.color = new Color(1, 1, 1, Mathf.Lerp(1, 0, 1 - countdown));
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), black);
            GUI.EndGroup();
        }
        else if (returned)
        {
            GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
            GUI.color = new Color(1, 1, 1, Mathf.Lerp(0, 1, 1 - countdown));
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), black);
            GUI.EndGroup();
        }
    }

    void Start()
    {
        setname = "";
        started = true;
        returned = false;
        countdown = 1f;
        if (!mainMenu)
            if (CurrentGameState.currentScore >= CurrentGameState.MinimumHighscoreRequirement())
                addnewScore = true;
            else
                addnewScore = false;
    }

    void Update()
    {
        if (started)
        {
            countdown -= 0.02f;
            if (countdown <= 0)
                started = false;
        }
        else if (returned)
        {
            countdown -= 0.02f;
            if (countdown <= 0)
                Application.LoadLevel(0);
        }
    }

    void OnGUI()
    {
        GUI.skin = gSkin;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Return();
        }
        else
            Menu_HighScore();
    }

    void Return()
    {
        if (!returned && !started)
        {
            if (mainMenu)
            {
                this.enabled = false;
                GetComponent<MainMenuScript>().enabled = true;
            }
            else
            {
                returned = true;
                countdown = 1f;
            }
        }
            
    }

    void Add_Score()
    {
        addnewScore = false;
        GameObject ob = new GameObject();
        ob.AddComponent<HighScoreElement>();
        ob.GetComponent<HighScoreElement>().user = setname;
        ob.GetComponent<HighScoreElement>().score = CurrentGameState.currentScore;
        CurrentGameState.AddHighscoreElement(ob);
        CurrentGameState.UpdateHighscore();
    }
}
