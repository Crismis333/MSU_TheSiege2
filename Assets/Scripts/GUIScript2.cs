using UnityEngine;
using System.Collections;

public class GUIScript2 : MonoBehaviour
{

    public float engagePercent, releasePercent;
    public float fixedEngagePercent;
    public bool engageFixed, releaseFixed;

    GUIStyle style = new GUIStyle();
    //GUIStyle bar = new GUIStyle();
    public Texture2D texture;
    public bool BarActive;

    public float ShowLingerTime = 1.0f;
    private float lingerTimer;
    private bool showLinger;

    private int screenHeight, screenWidth;

    // Bar position and size
    public int BarWidth = 200, BarHeight = 30;

    // Use this for initialization
    void Start()
    {
		texture = new Texture2D(128, 128);
        texture.Apply();

        style.normal.background = texture;

        screenHeight = Screen.height;
        screenWidth = Screen.width;
    }

    // Update is called once per frame
    void Update()
    {
        if (showLinger)
        {
            if (lingerTimer > ShowLingerTime)
            {
                engagePercent = 0;
                releasePercent = 0;
                fixedEngagePercent = 0;
                engageFixed = false;
                showLinger = false;
            }
            else
            {
                lingerTimer += Time.deltaTime;
            }
        }
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 80, 30), "Restart"))
        {

            Application.LoadLevel(Application.loadedLevel);

        }

        if (GUI.Button(new Rect(100, 10, 80, 30), "To Menu"))
        {

            Application.LoadLevel(1);

        }

        GUI.Label(new Rect(10, 50, 80, 30), "" + Time.timeSinceLevelLoad);


        EngageReleaseBar();

    }

    void EngageReleaseBar()
    {

        handleEngagement(screenWidth / 2 - BarWidth / 2, screenHeight - BarHeight - 20, BarWidth, BarHeight);
        //  handleRelease(120, 55, 100, 20);
    }

    public void ResetBar()
    {
        showLinger = true;
        lingerTimer = 0;
    }

    private void handleEngagement(float left, float top, float width, float height)
    {


        GUI.Box(new Rect(left, top, width, height), new GUIContent(""), style);

        if (BarActive)
        {
            if (engagePercent > 0 && engagePercent < 97)
            {
                float pos = engagePercent / 100 * width;
                //  print("Engagepercent: " + engagePercent);
                GUI.Box(new Rect(left + pos, top, 2, height), new GUIContent(""));
            }

            if (engageFixed)
            {
                float pos = fixedEngagePercent / 100 * width;
                GUI.Box(new Rect(left + pos, top, 2, height), new GUIContent(""));
            }

            if (releasePercent > 0)
            {
                float pos = releasePercent / 100 * width;
                GUI.Box(new Rect(left + (width - pos), top, 2, height), new GUIContent(""));
            }
        }
    }

    private void handleRelease(float left, float top, float width, float height)
    {

        GUI.Box(new Rect(left, top, width, height), new GUIContent(""), style);


        GUI.Box(new Rect(left + releasePercent, top, 2, height), new GUIContent(""));
    }
}
