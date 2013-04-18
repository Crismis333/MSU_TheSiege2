using UnityEngine;
using System.Collections;

public class PreviousLines : MonoBehaviour {

    public Material Line_Material;
    public GameObject origin;
    public GameObject firstPosition;
    private GameObject[] linerenderes;
    private GameObject herolinerenderer;
    private GameObject firstlinerenderer;
    //private int offset;
    private Hero hero;
    private Vector3 herostart;

	// Use this for initialization
	void Start () {
        //offset = 0;
	}

    public void Init(Hero hero)
    {
        this.hero = hero;
        herostart = hero.transform.position;
        linerenderes = new GameObject[CurrentGameState.completedLevelLocations.Count - 1];
        GameObject lr;
        for (int i = 0; i < CurrentGameState.completedLevelLocations.Count - 1; i++)
        {
            lr = new GameObject();
            lr.AddComponent<LineRenderer>();
            SetupLineRenderer(lr.GetComponent<LineRenderer>(), CurrentGameState.completedLevelLocations[i], CurrentGameState.completedLevelLocations[i + 1]);
            linerenderes[i] = lr;
        }
        if (hero != null)
        {
            lr = new GameObject();
            lr.AddComponent<LineRenderer>();
            SetupLineRenderer(lr.GetComponent<LineRenderer>(), herostart, hero.transform.position);
            herolinerenderer = lr;
            herolinerenderer.GetComponent<LineRenderer>().enabled = false;
        }
        if (origin != null)
        {
            lr = new GameObject();
            lr.AddComponent<LineRenderer>();
            SetupLineRenderer(lr.GetComponent<LineRenderer>(), origin.transform.position, firstPosition.transform.position);
            firstlinerenderer = lr;
        }
    }

    private void SetupLineRenderer(LineRenderer lr, Vector3 po1, Vector3 po2)
    {
        lr.receiveShadows = false;
        lr.SetWidth(0.01F, 0.01F);
        lr.SetVertexCount(2);
        Vector3 nl = po1;
        nl.y += 0.1f;
        lr.SetPosition(0, nl);
        nl = po2;
        nl.y += 0.1f;
        float diff = Vector3.Distance(po1, po2);
        lr.SetPosition(1, nl);
        lr.material = Line_Material;
        lr.material.mainTextureScale = new Vector2(1F * (diff) * 10f, 1F);
    }
	
	// Update is called once per frame
	void Update () {
        if (herolinerenderer != null)
        {
            herolinerenderer.GetComponent<LineRenderer>().SetPosition(1, hero.transform.position);
            float diff = Vector3.Distance(herostart, hero.transform.position);
            if (diff > 0.03)
                herolinerenderer.GetComponent<LineRenderer>().enabled = true;
            herolinerenderer.GetComponent<LineRenderer>().material.mainTextureScale = new Vector2(1F * (diff) * 10f, 1F);
        }
        /*
        if (linerenderes.Length > 0)
        {
            if (Camera.mainCamera.GetComponent<MapGui>().stopped)
            {
                for (int i = 0; i < linerenderes.Length; i++)
                {
                    linerenderes[i].SetActive(false);
                }
                linerenderes = new GameObject[0];
            }
            for (int i = 0; i < linerenderes.Length; i++)
            {
                linerenderes[i].GetComponent<LineRenderer>().material.mainTextureOffset = new Vector2(-Time.time * 0.4f, 0);
            }
            offset++;
            if (offset > 1024)
                offset = 0;
        }*/
	}
}
