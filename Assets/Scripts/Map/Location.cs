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
    [HideInInspector]
    public List<Modifier> modifiers;
    [HideInInspector]
    public Vector3 startLocation;

    [HideInInspector]
    public int positionInParent;

	public List<GameObject> SideModules;
	public GameObject SpecialModule;
    public int SpecialPartCount;
	public GameObject DefaultRoad;

    [HideInInspector]
    public bool RB_activated;

    private GameObject[] linerenderes;
    private int offset;
    
    private bool pushed;

    public static List<int> LOC_STAT_SOLDIER, LOC_STAT_OBSTACLE, LOC_STAT_BOULDER;


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
        if (LOC_STAT_SOLDIER == null)
        {
            LOC_STAT_SOLDIER = new List<int>();
            for (int i = 0; i < 40; i++)
                LOC_STAT_SOLDIER.Add(0);
            LOC_STAT_OBSTACLE = new List<int>();
            for (int i = 0; i < 40; i++)
                LOC_STAT_OBSTACLE.Add(0);
            LOC_STAT_BOULDER = new List<int>();
            for (int i = 0; i < 40; i++)
                LOC_STAT_BOULDER.Add(0);
        }

        startLocation = this.transform.position;
        offset = 0;
        RB_activated = false;
        pushed = false;

        if (CurrentGameState.FirstTime)
        {
            LocationStats ls = new LocationStats { Boulder = 0, Soldier = 0, Obstacle = 0 };
            int modifier = (layer > 5) ? 2 : 1;

            int sum = difficulty_soldier + difficulty_obstacles + difficulty_catapults;
            int lsSum = 0;

            while (lsSum > sum + modifier || lsSum < sum - modifier)
            {
                ls.Soldier = Mathf.Min(difficulty_soldier + Random.Range(-modifier, modifier + 1), 10);
                ls.Boulder = Mathf.Min(difficulty_catapults + Random.Range(-modifier, modifier + 1), 10);
                ls.Obstacle = Mathf.Min(difficulty_obstacles + Random.Range(-modifier, modifier + 1), 10);

                lsSum = ls.Soldier + ls.Boulder + ls.Obstacle;
            }

            LOC_STAT_SOLDIER[levelID] = ls.Soldier;
            LOC_STAT_OBSTACLE[levelID] = ls.Obstacle;
            LOC_STAT_BOULDER[levelID] = ls.Boulder;
            //LOC_STATS.Insert(levelID, ls);
        }

        difficulty_soldier = LOC_STAT_SOLDIER[levelID];
        difficulty_obstacles = LOC_STAT_OBSTACLE[levelID];
        difficulty_catapults = LOC_STAT_BOULDER[levelID];

        if (CurrentGameState.JustStarted && levelID == 0)
        {
            CurrentGameState.JustStarted = false;
            CurrentGameState.locID = this.levelID;
            CurrentGameState.previousPosition = this.transform.position;
            CurrentGameState.previousPreviousPosition = this.transform.position;
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
        }
        if (CurrentGameState.completedlevels.Contains(this.levelID))
        {
            if (CurrentGameState.loc != this)
                ActivateRigidBody();
        }
        else
        {
            this.GetComponent<CapsuleCollider>().enabled = false;
            foreach (MeshRenderer mr in this.GetComponentsInChildren<MeshRenderer>())
                mr.enabled = false;
        }
    }

    private Modifier getRandomModifier()
    {
        int rI = Random.Range(1, 6);

        switch (rI)
        {
            case 1: return Modifier.Catapult;
            case 2: return Modifier.Obstacle;
            case 3: return Modifier.Soldier;
            case 4: return Modifier.MoveSpeed;
            case 5: return Modifier.Jump;
        }

        return Modifier.Catapult;
    }

    private void SetupLineRenderer(LineRenderer lr, Location lo)
    {
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

public struct LocationStats
{
    public int Soldier;
    public int Obstacle;
    public int Boulder;
}
