using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GUINavigation))]
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

    private Texture2D background;
    private Color activeColor, inactiveColor;
    private bool firstGUI;
    private int finalchar;
    private bool movedUp, movedDown, movedLeft, movedRight, keyDownA, keyDownB;
    private int highscoreNr;

    void Menu_HighScore()
    {
        if (!GetComponent<GUINavigation>().usingMouse)
            GUI.skin.button.hover.background = null;
        else
            GUI.skin.button.hover.background = background;
        if (GetComponent<GUINavigation>().activated)
            GUI.skin.button.focused.textColor = activeColor;
        else
            GUI.skin.button.focused.textColor = inactiveColor;

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

            GUI.SetNextControlName("title");
            if (highscoreNr == 0)
                GUI.Label(new Rect(60, 1 * 30+10, 790, 64), "Campaign Mode");
            else if (highscoreNr == 1)
                GUI.Label(new Rect(60, 1 * 30 + 10, 790, 64), "Eternal Rush Mode");
            GUI.skin.label.fontSize = 14;
            GUI.Label(new Rect(0, 1 * 30 + 70, 790, 64), "I");
            GUI.Label(new Rect(0, 2 * 30 + 70, 790, 64), "II");
            GUI.Label(new Rect(0, 3 * 30 + 70, 790, 64), "III");
            GUI.Label(new Rect(0, 4 * 30 + 70, 790, 64), "IV");
            GUI.Label(new Rect(0, 5 * 30 + 70, 790, 64), "V");
            GUI.Label(new Rect(0, 6 * 30 + 70, 790, 64), "VI");
            GUI.Label(new Rect(0, 7 * 30 + 70, 790, 64), "VII");
            GUI.Label(new Rect(0, 8 * 30 + 70, 790, 64), "VIII");
            GUI.Label(new Rect(0, 9 * 30 + 70, 790, 64), "IX");
            GUI.Label(new Rect(0, 10 * 30 + 70, 790, 64), "X");
            GUI.color = Color.red;

            string s = "";
            if (highscoreNr == 0)
                s = "Campaign";
            else if (highscoreNr == 1)
                s = "Infinite";
            for (int i = 1; i <= 10; i++) {
                GUI.Label(new Rect(50, i * 30 + 70, 790, 64), "  " + PlayerPrefs.GetString("Highscore"+i+s+"Name"));
                GUI.Label(new Rect(400, i * 30 + 70, 790, 64), PlayerPrefs.GetString("Highscore"+i+s+"Score"));
            }
            
            //GUI.color = Color.white;
            GUI.skin.label.fontSize = 20;

            GUI.SetNextControlName("Return");
            if (GUI.Button(new Rect(60, 12 * 35, 640, 64), "Return")) { Return(); }
            GUI.Box(new Rect(60, 12 * 35, 640, 64), new GUIContent("", "0"));
            GetComponent<GUINavigation>().mouseover = GUI.tooltip;

            GUI.EndGroup();
            //GUI.color = Color.black;


            GUI.skin.button.fontSize = 40;
            //GUI.BeginGroup(new Rect(Screen.width / 2 - 395, Screen.height / 2 - 5 * 35, 790, 10 * 35));
            GUI.BeginGroup(new Rect(Screen.width / 2 - 495, Screen.height / 2 - 7.5f * 35, 155, 15 * 35));
            if (GUI.Button(new Rect(0, 6 * 30, 155, 100), "<"))
            {
                highscoreNr--;
                if (highscoreNr < 0) highscoreNr = 1;
            }
            GUI.EndGroup();

            GUI.BeginGroup(new Rect(Screen.width / 2 + 280, Screen.height / 2 - 7.5f * 35, 155, 15 * 35));
            if (GUI.Button(new Rect(0, 6*30, 155, 100), ">"))
            {
                highscoreNr--;
                if (highscoreNr < 0) highscoreNr = 1;
            }
            GUI.EndGroup();

            GUI.skin.button.fontSize = 24;
            GUI.color = Color.white;
            if (!GetComponent<GUINavigation>().usingMouse)
            {
                if (GetComponent<GUINavigation>().keySelect == 0)
                    GUI.FocusControl("Return");
                else
                    GUI.FocusControl("title");
            }
            else
                GUI.FocusControl("title");
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
        GUI.skin.button.hover.background = background;
        GUI.skin.button.focused.textColor = inactiveColor;
    }

    void Start()
    {
        highscoreNr = 0;
        finalchar = -1;
        keyDownA = false;
        keyDownB = false;
        movedLeft = false;
        movedRight = false;
        movedUp = false;
        movedDown = false;
        firstGUI = true; 
        setname = "";
        if (!mainMenu)
            started = true;
        else
            started = false;
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
        //print("last character: " + GetLastCharacter());
        //SetLastCharacter('5');
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
        else if (addnewScore)
        {
            float kv = Input.GetAxisRaw("Vertical");


            if (!Camera.mainCamera.GetComponent<GUINavigation>().usingMouse)
            {

                if (!keyDownA && (Input.GetButton("Fire1") || Input.GetKey(KeyCode.Z) || GUINavigation.AButtonState()))
                {
                    keyDownA = true;
                    setname += 'a';
                    finalchar = 1;
                }
                else if (keyDownA && (!Input.GetButton("Fire1") && !Input.GetKey(KeyCode.Z) && !GUINavigation.AButtonState()))
                {
                    keyDownA = false;
                }

                if (!keyDownB && (Input.GetButton("Fire2") || Input.GetKey(KeyCode.X) || GUINavigation.BButtonState()))
                {
                    keyDownB = true;
                    if (setname.Length > 0)
                    {
                        setname = setname.Substring(0, setname.Length - 1);
                        finalchar = IndexFinder(GetLastCharacter());
                    }
                }
                else if (keyDownB && (!Input.GetButton("Fire2") && !Input.GetKey(KeyCode.X) && !GUINavigation.BButtonState()))
                {
                    keyDownB = false;
                }

                if (Input.GetKeyDown(KeyCode.Return) || GetComponent<GUINavigation>().usedMenu)
                {
                    Add_Score();
                    return;
                }

                int finalcharstart = finalchar;

                if (finalchar == -1)
                {
                    if (kv < -0.01 && !movedUp)
                        finalchar = 1;
                    else if (kv > 0.01 && !movedDown)
                        finalchar = 0;
                }
                else
                {
                    if (kv < -0.01 && !movedUp)
                        finalchar++;
                    else if (kv > 0.01 && !movedDown)
                        finalchar--;
                    if (finalchar < 0) finalchar = 69;
                    else if (finalchar > 69) finalchar = 0;
                }

                if (finalcharstart != finalchar)
                    SetLastCharacter(CharIndexFinder(finalchar));
            }



            if (kv > 0.01)
            {
                movedDown = true;
                movedUp = false;
            }
            else if (kv < -0.01)
            {
                movedDown = false;
                movedUp = true;
            }
            else
            {
                movedDown = false;
                movedUp = false;
            }

        }
        else
        {
            float kh = Input.GetAxisRaw("Horizontal");

            if (kh < -0.01 && !movedLeft)
                highscoreNr++;
            else if (kh > 0.01 && !movedRight)
                highscoreNr--;
            if (highscoreNr < 0) highscoreNr = 1;
            else if (highscoreNr > 1) highscoreNr = 0;

            if (kh > 0.01)
            {
                movedRight = true;
                movedLeft = false;
            }
            else if (kh < -0.01)
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
    }

    void OnGUI()
    {
        GUI.skin = gSkin;
        if (firstGUI)
        {
            background = GUI.skin.button.hover.background;
            activeColor = GUI.skin.button.active.textColor;
            inactiveColor = GUI.skin.button.focused.textColor;
            if (addnewScore)
            {
            }
            else
            {
                GetComponent<GUINavigation>().ClearElements();
                GetComponent<GUINavigation>().maxKeys = 1;
                GetComponent<GUINavigation>().menuKey = 0;
                GetComponent<GUINavigation>().AddElement(0, Accept);
            }
            firstGUI = false;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Accept();
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
                GetComponent<GUINavigation>().ClearElements();
                GetComponent<GUINavigation>().maxKeys = 7;
                GetComponent<GUINavigation>().AddElement(0, GetComponent<MainMenuScript>().Menu_Main_Start_Campaign);
                GetComponent<GUINavigation>().AddElement(1, GetComponent<MainMenuScript>().Menu_Main_Start_Endless);
                GetComponent<GUINavigation>().AddElement(2, GetComponent<MainMenuScript>().Menu_Main_Controls);
                GetComponent<GUINavigation>().AddElement(3, GetComponent<MainMenuScript>().Menu_Main_Options);
                GetComponent<GUINavigation>().AddElement(4, GetComponent<MainMenuScript>().Menu_Main_Highscores);
                GetComponent<GUINavigation>().AddElement(5, GetComponent<MainMenuScript>().Menu_Main_Credits);
                GetComponent<GUINavigation>().AddElement(6, GetComponent<MainMenuScript>().Menu_Main_Quit);
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
        if (!CurrentGameState.InfiniteMode)
        {
            CurrentGameState.AddHighscoreElement(ob);
            CurrentGameState.UpdateHighscore();
        }
        else
        {
            CurrentGameState.AddHighscoreElementInfinite(ob);
            CurrentGameState.UpdateHighscoreInfinite();
        }
        GetComponent<GUINavigation>().ClearElements();
        GetComponent<GUINavigation>().maxKeys = 1;
        GetComponent<GUINavigation>().menuKey = 0;
        GetComponent<GUINavigation>().AddElement(0, Accept);
    }

    public void Accept()
    {
        if (addnewScore)
            Add_Score();
        else
            Return();
    }
  
    private char GetLastCharacter()
    {
        if (setname.Length == 0)
            return '\0';
        else
           return setname.ToCharArray()[setname.Length - 1];
    }

    private void SetLastCharacter(char c)
    {
        if (setname.Length == 0)
            setname = ""+c;
        else
            setname = setname.Substring(0, setname.Length - 1) + c;
    }

    private char CharIndexFinder(int index)
    {
        switch (index)
        {
            case 0: return ' ';
            case 1: return 'a';
            case 2: return 'b';
            case 3: return 'c';
            case 4: return 'd';
            case 5: return 'e';
            case 6: return 'f';
            case 7: return 'g';
            case 8: return 'h';
            case 9: return 'i';
            case 10: return 'j';
            case 11: return 'k';
            case 12: return 'l';
            case 13: return 'm';
            case 14: return 'n';
            case 15: return 'o';
            case 16: return 'p';
            case 17: return 'q';
            case 18: return 'r';
            case 19: return 's';
            case 20: return 't';
            case 21: return 'u';
            case 22: return 'v';
            case 23: return 'w';
            case 24: return 'x';
            case 25: return 'y';
            case 26: return 'z';
            case 27: return 'A';
            case 28: return 'B';
            case 29: return 'C';
            case 30: return 'D';
            case 31: return 'E';
            case 32: return 'F';
            case 33: return 'G';
            case 34: return 'H';
            case 35: return 'I';
            case 36: return 'J';
            case 37: return 'K';
            case 38: return 'L';
            case 39: return 'M';
            case 40: return 'N';
            case 41: return 'O';
            case 42: return 'P';
            case 43: return 'Q';
            case 44: return 'R';
            case 45: return 'S';
            case 46: return 'T';
            case 47: return 'U';
            case 48: return 'V';
            case 49: return 'W';
            case 50: return 'X';
            case 51: return 'Y';
            case 52: return 'Z';
            case 53: return '1';
            case 54: return '2';
            case 55: return '3';
            case 56: return '4';
            case 57: return '5';
            case 58: return '6';
            case 59: return '7';
            case 60: return '8';
            case 61: return '9';
            case 62: return '0';
            case 63: return '-';
            case 64: return '_';
            case 65: return '?';
            case 66: return '!';
            case 67: return '+';
            case 68: return '.';
            case 69: return ',';
            default: return 'a';
        }
    }

    private int IndexFinder(char ch)
    {
        switch (ch)
        {
            case ' ': return 0;
            case 'a': return 1;
            case 'b': return 2;
            case 'c': return 3;
            case 'd': return 4;
            case 'e': return 5;
            case 'f': return 6;
            case 'g': return 7;
            case 'h': return 8;
            case 'i': return 9;
            case 'j': return 10;
            case 'k': return 11;
            case 'l': return 12;
            case 'm': return 13;
            case 'n': return 14;
            case 'o': return 15;
            case 'p': return 16;
            case 'q': return 17;
            case 'r': return 18;
            case 's': return 19;
            case 't': return 20;
            case 'u': return 21;
            case 'v': return 22;
            case 'w': return 23;
            case 'x': return 24;
            case 'y': return 25;
            case 'z': return 26;
            case 'A': return 27;
            case 'B': return 28;
            case 'C': return 29;
            case 'D': return 30;
            case 'E': return 31;
            case 'F': return 32;
            case 'G': return 33;
            case 'H': return 34;
            case 'I': return 35;
            case 'J': return 36;
            case 'K': return 37;
            case 'L': return 38;
            case 'M': return 39;
            case 'N': return 40;
            case 'O': return 41;
            case 'P': return 42;
            case 'Q': return 43;
            case 'R': return 44;
            case 'S': return 45;
            case 'T': return 46;
            case 'U': return 47;
            case 'V': return 48;
            case 'W': return 49;
            case 'X': return 50;
            case 'Y': return 51;
            case 'Z': return 52;
            case '1': return 53;
            case '2': return 54;
            case '3': return 55;
            case '4': return 56;
            case '5': return 57;
            case '6': return 58;
            case '7': return 59;
            case '8': return 60;
            case '9': return 61;
            case '0': return 62;
            case '-': return 63;
            case '_': return 64;
            case '?': return 65;
            case '!': return 66;
            case '+': return 67;
            case '.': return 68;
            case ',': return 69;
            default: return 1;
        }
    }
}
