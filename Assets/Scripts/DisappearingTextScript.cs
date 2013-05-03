using UnityEngine;
using System.Collections;

public class DisappearingTextScript : MonoBehaviour {

    public string text;
    public GUISkin gSkin;
    public int x, y;

    private float alpha;
    private int offset;
	// Use this for initialization
	void Start () {
        alpha = 1.5f;
        offset = 0;
	}
	
	// Update is called once per frame
	void Update () {
        alpha -= Time.deltaTime;
        offset -= 1;
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
        gSkin.label.fontSize = 32;

        GUI.BeginGroup(new Rect(x-300, y-200, 600, 400));

        GUI.color = new Color(0, 0, 0, Mathf.Clamp(alpha, 0, 1));
        GUI.Label(new Rect(1, 1 + offset, 600, 400), text);
        GUI.Label(new Rect(-1, -1 + offset, 600, 400), text);
        GUI.Label(new Rect(1, - 1 + offset, 600, 400), text);
        GUI.Label(new Rect(-1, 1 + offset, 600, 400), text);

        GUI.color = new Color(219f / 256f, 168f / 256f, 1f / 256f, Mathf.Clamp(alpha, 0, 1));
        GUI.Label(new Rect(0, offset, 600, 400), text);

        GUI.EndGroup();

        gSkin.label.alignment = t;
        gSkin.label.fontSize = f;
    }
}
