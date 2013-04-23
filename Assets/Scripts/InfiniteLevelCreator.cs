using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class InfiniteLevelCreator : MonoBehaviour
{
    public List<GameObject> sideModules;
    public List<GameObject> specialModules;
    public List<int> specialModuleParts;
    public GameObject defaultRoad;

    private Queue<GameObject> sidesA;
    private int sideAState = -1;
    private int prevASide = -1;
    private int curASide = 0;
    private int varACount = 0;

    private Queue<GameObject> sidesB;
    private int sideBState = -1;
    private int prevBSide = -1;
    private int curBSide = 0;
    private int varBCount = 0;

    private Queue<GameObject> roads;
    private Queue<GameObject> specials;

    private int currentIndex = 0;

    // Use this for initialization
    void Start()
    {
        roads = new Queue<GameObject>();
        sidesA = new Queue<GameObject>();
        sidesB = new Queue<GameObject>();
        specials = new Queue<GameObject>();

        LevelCreator.INF_MODE = true;
    }

    // Update is called once per frame
    void Update()
    {
        bool nextIndex = false;
        if (roads.Count < 5)
        { 
            CreateRoads();
            nextIndex = true;
        }

        if (sidesA.Count < 5)
        { 
            CreateSides(sidesA, true);
            nextIndex = true;
        }

        if (sidesB.Count < 5)
        { 
            CreateSides(sidesB, false);
            nextIndex = true;
        }
        
        if (nextIndex)
            currentIndex++;

        //CreateSpecials();

        GameObject tmpRoad = roads.Peek();
        if (tmpRoad.transform.position.z <= ObstacleController.PLAYER.transform.position.z - 64)
        {
            Destroy(roads.Dequeue());
        }

        GameObject tmpSideA = sidesA.Peek();
        if (tmpSideA.transform.position.z <= ObstacleController.PLAYER.transform.position.z - 64)
        {
            Destroy(sidesA.Dequeue());
        }

        GameObject tmpSideB = sidesB.Peek();
        if (tmpSideB.transform.position.z <= ObstacleController.PLAYER.transform.position.z - 64)
        {
            Destroy(sidesB.Dequeue());
        }

        if (specials.Count != 0)
        {
            GameObject tmpSpecial = specials.Peek();
            if (tmpSpecial.transform.position.z <= ObstacleController.PLAYER.transform.position.z - 64)
            {
                Destroy(specials.Dequeue());
            }
        }
    }

    private void CreateRoads()
    {
        GameObject tmp;
        Vector3 pos = transform.position;
        pos.z = 64 * currentIndex;

        tmp = Instantiate(defaultRoad, pos, defaultRoad.transform.rotation) as GameObject;

        roads.Enqueue(tmp);
    }

    private void CreateSides(Queue<GameObject> sides, bool left)
    {
        int variationCap = 7;

        int randomSide = left ? curASide : curBSide;
        int variationCounter = left ? varACount : varBCount;
        int state = left ? sideAState : sideBState;
        int prevSide = left ? prevASide : prevBSide;
        //bool forceSide = false;

        GameObject tmp;
        Vector3 pos = transform.position;
        pos.z = 64 * currentIndex;

        if (left)
        {
            pos.x -= 40;
        }
        else
        {
            pos.x += 40;
        }

        if (state == -1)
        {
            randomSide = Random.Range(0, sideModules.Count);
            if (variationCounter >= variationCap)
            {
                int q = randomSide;
                while (q == randomSide)
                {
                    randomSide = Random.Range(0, sideModules.Count);
                }
            }
        }

        if (prevSide != -1)
        {
            string tmpName = Regex.Replace(sideModules[randomSide].name, @"[\d-]", string.Empty);
            string prevName = Regex.Replace(sideModules[prevSide].name, @"[\d-]", string.Empty);
            GameObject side = null;

            if (!tmpName.Equals(prevName) && state == -1)
            {
                state = 0;
                variationCounter = 0;
            }
            else
            {
                variationCounter++;
            }

            string realName = sideModules[randomSide].name;

            switch (state)
            {
                case -1:
                    side = Resources.Load("SideModules/Sides/" + realName, typeof(GameObject)) as GameObject;
                    break;
                case 0:
                    side = Resources.Load("SideModules/SideStarts/" + prevName + "_start", typeof(GameObject)) as GameObject;
                    state++;
                    break;
                case 1:
                    side = Resources.Load("SideModules/SideTransitions/" + prevName + "_to_" + tmpName, typeof(GameObject)) as GameObject;
                    state++;
                    break;
                case 2:
                    side = Resources.Load("SideModules/SideStarts/" + tmpName + "_start", typeof(GameObject)) as GameObject;
                    state++;
                    break;
                case 3:
                    side = Resources.Load("SideModules/Sides/" + realName, typeof(GameObject)) as GameObject;
                    state = -1;
                    break;
            }

            tmp = Instantiate(side, pos, side.transform.rotation) as GameObject;

            if (((state == 1 || state == 2) && left) || (state == 3 && !left))
            {
                Vector3 tmpScale = tmp.transform.localScale;
                tmpScale.y = -1;
                tmp.transform.localScale = tmpScale;
            }
        }
        else
        {
            tmp = Instantiate(sideModules[randomSide], pos, sideModules[randomSide].transform.rotation) as GameObject;
        }

        if (!left)
        {
            tmp.transform.rotation *= Quaternion.Euler(0, 0, 180);
        }

        sides.Enqueue(tmp);

        if (state == -1 || state == 3)
        {
            prevSide = randomSide;
        }

        if (left)
        {
            sideAState = state;
            prevASide = prevSide;
            varACount = variationCounter;
            curASide = randomSide;
        }
        else
        {
            sideBState = state;
            prevBSide = prevSide;
            varBCount = variationCounter;
            curBSide = randomSide;
        }
    }

    /*
    private void CreateSpecials()
    {
        GameObject tmp;
        Vector3 pos = transform.position;
        pos.z = 64 * currentIndex;

        string sName = Regex.Replace(specialModules.name, @"[\d-]", string.Empty);

        specialModule = Resources.Load("SpecialModules/" + sName + (i - specialModuleIndex + 1), typeof(GameObject)) as GameObject;

        tmp = Instantiate(specialModule, pos, specialModule.transform.rotation) as GameObject;

        specials.Enqueue(tmp);
    }
    */
}
