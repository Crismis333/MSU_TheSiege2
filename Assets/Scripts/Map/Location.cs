using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Location : MonoBehaviour {

    public int levelID;
    public int layer;
    public string LevelName;
    [Multiline]
    public string description;
    public int difficulty_soldier, difficulty_length, difficulty_pits, difficulty_obstacles, difficulty_catapults;
    public Location[] locations;
    public Material Line_Material;
    public List<Modifier> modifiers;
    [HideInInspector]
    public Vector3 startLocation;
	
	public List<GameObject> SideModules;
	public GameObject SpecialModule;
    public int SpecialPartCount;
	public GameObject DefaultRoad;

    [HideInInspector]
    public bool RB_activated;

    private GameObject[] linerenderes;
    private int offset;
    
    private bool pushed;

    void Update()
    {
        if (RB_activated && !pushed)
        {
            if (transform.position.y > startLocation.y + 0.1f)
            {
                GetComponent<Rigidbody>().AddForce(new Vector3(3, 0, 3));
                pushed = true;
            }
        }
        if (linerenderes.Length > 0)
        {
            if (Camera.mainCamera.GetComponent<MapGui>().stopped)
            {
                for (int i = 0; i < linerenderes.Length; i++)
                    linerenderes[i].SetActive(false);
                linerenderes = new GameObject[0];
            }
            for (int i = 0; i < linerenderes.Length; i++)
            {
                linerenderes[i].GetComponent<LineRenderer>().material.mainTextureOffset = new Vector2(-Time.time * 0.4f, 0);
            }
            offset++;
            if (offset > 1024)
                offset = 0;
        }
    }

    void Start()
    {
        startLocation = this.transform.position;
        offset = 0;
        RB_activated = false;
        pushed = false;
        //if (levelID == 0)
        //{
        //    this.GetComponent<CapsuleCollider>().enabled = false;
        //    foreach (MeshRenderer mr in this.GetComponentsInChildren<MeshRenderer>())
        //        mr.enabled = false;
        //}
        if (CurrentGameState.JustStarted && levelID == 0)
        {
            CurrentGameState.JustStarted = false;
            CurrentGameState.locID = this.levelID;
            CurrentGameState.previousPosition = this.transform.position;
            CurrentGameState.completedLevelLocations.Add(this.transform.position);
        }
        if (CurrentGameState.locID == this.levelID)
        {
            CurrentGameState.loc = this;
            GameObject lr;
            linerenderes = new GameObject[locations.Length];
            for (int i = 0; i < locations.Length; i++)
            {
                lr = new GameObject();
                lr.AddComponent<LineRenderer>();
                SetupLineRenderer(lr.GetComponent<LineRenderer>(), locations[i]);
                linerenderes[i] = lr;
            }
        }
        else 
        {
            linerenderes = new GameObject[0];
            /*
            //linerenderes = new GameObject[0];
            GameObject lr;
            linerenderes = new GameObject[locations.Length];
            for (int i = 0; i < locations.Length; i++)
            {
                lr = new GameObject();
                lr.AddComponent<LineRenderer>();
                SetupLineRenderer(lr.GetComponent<LineRenderer>(), locations[i]);
                linerenderes[i] = lr;
            }*/
        }
        if (CurrentGameState.completedlevels.Contains(this.levelID))
            ActivateRigidBody();
        else
        {
            this.GetComponent<CapsuleCollider>().enabled = false;
            foreach (MeshRenderer mr in this.GetComponentsInChildren<MeshRenderer>())
                mr.enabled = false;
        }
    }

    private void SetupLineRenderer(LineRenderer lr, Location lo)
    {
        //Material m = new Material(Shader.Find("Self-Illumin/Diffuse"));
        //m.color = Color.white;
        //lr.material = m;

        lr.receiveShadows = false;
        lr.SetWidth(0.02F, 0.02F);
        lr.SetVertexCount(2);
        Vector3 nl = this.gameObject.transform.position;
        nl.y += 0.1f;
        lr.SetPosition(0, nl);
        nl = lo.gameObject.transform.position;
        nl.y += 0.1f;
        float diff = Vector3.Distance(this.gameObject.transform.position,lo.gameObject.transform.position);
        lr.SetPosition(1, nl);
        lr.material = Line_Material;
        lr.material.mainTextureScale = new Vector2(1F*(diff)*5f, 1F);
    }

    public bool isChildOfCurrent()
    {
        bool found = false;
        for (int i = 0; i < CurrentGameState.loc.locations.Length; i++)
        {
            if (CurrentGameState.loc.locations[i] == this)
            {
                found = true;
                break;
            }
        }
        return found;
    }

    public void ActivateRigidBody()
    {
        if (!RB_activated)
        {
            this.gameObject.AddComponent<Rigidbody>();
            this.GetComponent<Rigidbody>().drag = 0;
            this.GetComponent<Rigidbody>().angularDrag = 10;
            RB_activated = true;
        }
    }
}
