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
    private int curASide = -1;
    private int varACount = 0;

    private Queue<GameObject> sidesB;
    private int sideBState = -1;
    private int prevBSide = -1;
    private int curBSide = -1;
    private int varBCount = 0;

    private Queue<GameObject> roads;
    private Queue<GameObject> specials;

    private int currentIndex = 0;

    private bool createSpecial = false;
    private int specialCount = 0;
    private int specialIndex = -1;

    private int specialVariation = 15;
    private int specialVarCount = -1;

    // Use this for initialization
    void Start()
    {
        roads = new Queue<GameObject>();
        sidesA = new Queue<GameObject>();
        sidesB = new Queue<GameObject>();
        specials = new Queue<GameObject>();

        LevelCreator.INF_MODE = true;
        GUIScript.SCORE = 0;
    }

    // Update is called once per frame
    void Update()
    {
        bool nextIndex = false;

        if (roads.Count + specials.Count < 5)
        {
            CreateRoads();
            nextIndex = true;
        }

        if (sidesA.Count + specials.Count < 5)
        { 
            CreateSides(sidesA, true);
            nextIndex = true;
        }

        if (sidesB.Count + specials.Count < 5)
        { 
            CreateSides(sidesB, false);
            nextIndex = true;
        }

        if (specialIndex != -1 && specialCount > specialModuleParts[specialIndex])
        {
            createSpecial = false;
            specialIndex = -1;
        }

        if (nextIndex)
        {
            currentIndex++;

            if (specialVarCount > specialVariation)
                specialVarCount = -1;

            if (specialVarCount != -1)
                specialVarCount++;
        }

        if (roads.Count != 0)
        {
            GameObject tmpRoad = roads.Peek();
            if (tmpRoad.transform.position.z <= ObstacleController.PLAYER.transform.position.z - 64)
                Destroy(roads.Dequeue());
        }

        if (sidesA.Count != 0)
        {
            GameObject tmpSideA = sidesA.Peek();
            if (tmpSideA.transform.position.z <= ObstacleController.PLAYER.transform.position.z - 64)
                Destroy(sidesA.Dequeue());
        }

        if (sidesB.Count != 0)
        {
            GameObject tmpSideB = sidesB.Peek();
            if (tmpSideB.transform.position.z <= ObstacleController.PLAYER.transform.position.z - 64)
                Destroy(sidesB.Dequeue());
        }

        if (specials.Count != 0)
        {
            GameObject tmpSpecial = specials.Peek();
            if (tmpSpecial.transform.position.z <= ObstacleController.PLAYER.transform.position.z - 64)
                Destroy(specials.Dequeue());
        }
    }

    private void CreateRoads()
    {
        GameObject tmp;
        Vector3 pos = transform.position;
        pos.z = 64 * currentIndex;

        if (createSpecial)
        {
            if (specialIndex == -1)
            {
                specialIndex = Random.Range(0, specialModules.Count);
                specialCount = 1;
                specialVarCount = 0;
            }

            GameObject specialMod = specialModules[specialIndex];
            string sName = Regex.Replace(specialMod.name, @"[\d-]", string.Empty);

            specialMod = Resources.Load("SpecialModules/" + sName + specialCount, typeof(GameObject)) as GameObject;

            tmp = Instantiate(specialMod, pos, specialMod.transform.rotation) as GameObject;

            specials.Enqueue(tmp);

            specialCount++;
        }
        else
        {
            tmp = Instantiate(defaultRoad, pos, defaultRoad.transform.rotation) as GameObject;
            roads.Enqueue(tmp);
        }
    }

    private void CreateSides(Queue<GameObject> sides, bool left)
    {
        int variationCap = 7;

        int randomSide = left ? curASide : curBSide;
        int variationCounter = left ? varACount : varBCount;
        int state = left ? sideAState : sideBState;
        int prevSide = left ? prevASide : prevBSide;

        if (createSpecial)
            return;

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
            randomSide = Random.Range(0, sideModules.Count + 3);
            if (randomSide >= sideModules.Count)
                randomSide = 0;
            if (variationCounter >= variationCap)
            {
                int q = randomSide;
                while (q == randomSide)
                {
                    randomSide = Random.Range(0, sideModules.Count);
                }
            }
        }

        if (!left && curBSide == 0 && curBSide == curASide &&
            (state == 3 || state == -1) && (sideAState == 3 || sideAState == -1) && specialVarCount == -1)
        {
            createSpecial = true;
            randomSide = 0;
        }

        if (prevSide != -1)
        {
            string tmpName = Regex.Replace(sideModules[randomSide].name, @"[\d-]", string.Empty);
            string prevName = Regex.Replace(sideModules[prevSide].name, @"[\d-]", string.Empty);
            GameObject side = null;

            if (!tmpName.Equals(prevName) && state == -1 && !createSpecial)
            {
                state = 0;
                variationCounter = 0;
            }
            else
            {
                variationCounter++;
            }

            switch (state)
            {
                case -1:
                    side = sideModules[randomSide];
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
                    side = sideModules[randomSide];
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
}
