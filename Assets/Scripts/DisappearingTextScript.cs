using UnityEngine;
using System.Collections;

public class DisappearingTextScript : MonoBehaviour {

    public string text;
    public GUISkin gSkin;
    public int x, y;
    public bool scoreText;

    private float alpha;
    private float offset, yscale;
	// Use this for initialization
	void Start () {
        alpha = 1.5f;
        offset = 0;
	}
	
	// Update is called once per frame
	void Update () {
        yscale = Screen.height / 1080f;
        alpha -= Time.deltaTime;
        offset -= Time.deltaTime*yscale*60;
        if (alpha <= 0)
        {
            Destroy(gameObject);
        }

	}

    void OnGUI()
    {
        GUI.skin = gSkin;
        TextAnchor t = gSkin.label.alignment;
        int f = gSkin.label.fontSize;

        gSkin.label.alignment = TextAnchor.MiddleCenter;

        if (scoreText)
            gSkin.label.fontSize = (int)(30*yscale);
        else
            gSkin.label.fontSize = (int)(45*yscale);
        GUI.BeginGroup(new Rect(x-300, y-200, 600, 400));

        if (Time.timeScale > 0)
        {

            GUI.color = new Color(0, 0, 0, Mathf.Clamp(alpha, 0, 1));
            GUI.Label(new Rect(1, 1 + offset, 600, 400), text);
            GUI.Label(new Rect(-1, -1 + offset, 600, 400), text);
            GUI.Label(new Rect(1, -1 + offset, 600, 400), text);
            GUI.Label(new Rect(-1, 1 + offset, 600, 400), text);
            if (ObstacleController.PLAYER.GetComponent<HeroMovement>().Charging)
                GUI.color = new Color(1, 0, 0, Mathf.Clamp(alpha, 0, 1));
            else
                GUI.color = new Color(219f / 256f, 168f / 256f, 1f / 256f, Mathf.Clamp(alpha, 0, 1));
            GUI.Label(new Rect(0, offset, 600, 400), text);
        }
        GUI.EndGroup();

        gSkin.label.alignment = t;
        gSkin.label.fontSize = f;
    }
}
