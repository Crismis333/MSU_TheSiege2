using UnityEngine;
using System.Collections;

public class LevelCompleteScript : MonoBehaviour {

    public GUISkin gSkin;
    public Texture2D backgroundScroll;
    public Vector2 scrollOffset;
    public Texture2D black;

    private Texture2D background;
    private Color activeColor, inactiveColor;
    private bool firstGUI, returned;
    private float countdown;

    void LevelComplete()
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

        GUI.BeginGroup(new Rect(Screen.width / 2 - 395, Screen.height / 2 - 7.5f * 35, 790, 15 * 35));
        GUI.color = Color.black;

        GUI.color = Color.white;

        GUI.SetNextControlName("Return");
        if (GUI.Button(new Rect(60, 12 * 35, 640, 64), "Return")) { Accept(); }
        GUI.Box(new Rect(60, 12 * 35, 640, 64), new GUIContent("", "0"));
        Camera.mainCamera.GetComponent<GUINavigation>().mouseover = GUI.tooltip;

        GUI.EndGroup();


        if (returned)
        {
            GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
            GUI.color = new Color(1, 1, 1, Mathf.Lerp(0, 1, 1 - countdown));
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), black);
            GUI.EndGroup();
        }
        GUI.skin.button.hover.background = background;
        GUI.skin.button.focused.textColor = inactiveColor;
    }

	// Use this for initialization
	void Start () {
        firstGUI = true;
        returned = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (returned)
        {
            countdown -= 0.02f;
            if (countdown <= 0)
            {
                CurrentGameState.SetWin();
                CurrentGameState.currentScore = GUIScript.SCORE;
                //Debug.Log("End Score: "+ GUIScript.SCORE);
                if (CurrentGameState.locID == 38)
                {

                    CurrentGameState.highscorecondition = EndState.Won;
                    Application.LoadLevel(3);
                }
                else
                    Application.LoadLevel(1);
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
            Camera.mainCamera.GetComponent<GUINavigation>().ClearElements();
            Camera.mainCamera.GetComponent<GUINavigation>().maxKeys = 1;
            Camera.mainCamera.GetComponent<GUINavigation>().AddElement(0, Accept);
            firstGUI = false;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Accept();
        }
        else
            LevelComplete();
    }

    public void Accept()
    {
        if (!returned)
        {
            returned = true;
            countdown = 1f;
        }
    }
}
