using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Modifier { None, Soldier, Obstacle, Pit, Catapult, Jump, MoveSpeed, SlowDown }
public enum EndState { Won, GaveUp, Lost, Infinite }

public class CurrentGameState : MonoBehaviour {

    public static int locID = 0;
    public static Location loc;
    public static bool JustStarted = true;
    public static int soldierModifier = 0;
    public static int obstacleModifier = 0;
    public static int pitModifier = 0;
    public static int catapultModifier = 0;
    public static float jumpLengthModifier = 1.0f;
    public static float moveSpeedModifier = 1.0f;
    public static float slowDownModifier = 1.0f;
    public static List<int> completedlevels = new List<int>();
    public static List<Vector3> completedLevelLocations = new List<Vector3>();
    public static Hero hero;
    public static Vector3 previousPosition, previousPreviousPosition;
    public static long currentScore;
    public static long previousScore;
    public static EndState highscorecondition;
    public static bool failedlast;
    public static bool InfiniteMode;
    private static List<Modifier> wins;
    private static int nextLevel;
    private static LinkedList<HighScoreElement> highscoreCampaign;
    private static LinkedList<HighScoreElement> highscoreInfinite;
    

    public static void CreateHero()
    {
        GameObject o = Resources.Load("HeroFigure", typeof(GameObject)) as GameObject;
        hero = (Instantiate(o) as GameObject).GetComponent<Hero>();
        hero.transform.position = loc.startLocation;

        if (loc.locations.Length > 0)
        {
            hero.LookAtLoc(loc.locations[0]);
        }
    }

    public static void SetWinModifiers(List<Modifier> modifiers, int levelID)
    {
        wins = modifiers;
        nextLevel = levelID;
    }

    public static void SetWin() 
    {
        locID = nextLevel;
        completedlevels.Add(locID);
        completedLevelLocations.Add(previousPosition);
        if (!InfiniteMode)
            foreach (Modifier m in wins)
                IncreaseModifier(m);
    }

    public static void Restart()
    {
        locID = 0;
        loc = null;
        JustStarted = true;
        soldierModifier = obstacleModifier = pitModifier = catapultModifier = 0;
        jumpLengthModifier = moveSpeedModifier = slowDownModifier = 1.0f;
        completedlevels = new List<int>();
        completedLevelLocations = new List<Vector3>();
        highscoreCampaign = new LinkedList<HighScoreElement>();
        highscoreInfinite = new LinkedList<HighScoreElement>();
        hero = null;
        wins = null;
        nextLevel = 0;
        currentScore = 0;
    }

    public static void AddHighscoreElement(GameObject go)
    {
        HighScoreElement hse = go.GetComponent<HighScoreElement>();
        LinkedListNode<HighScoreElement> node = highscoreCampaign.First;
        while (node != null)
            if (node.Value.score < hse.score)
            {
                highscoreCampaign.AddBefore(node,hse);
                return;
            }
            else
                node = node.Next;
        if (highscoreCampaign.Count < 10)
            highscoreCampaign.AddLast(hse);
    }

    public static void AddHighscoreElementInfinite(GameObject go)
    {
        HighScoreElement hse = go.GetComponent<HighScoreElement>();
        LinkedListNode<HighScoreElement> node = highscoreInfinite.First;
        while (node != null)
            if (node.Value.score < hse.score)
            {
                highscoreInfinite.AddBefore(node, hse);
                return;
            }
            else
                node = node.Next;
        if (highscoreInfinite.Count < 10)
            highscoreInfinite.AddLast(hse);
    }

    public static void UpdateHighscore()
    {
        LinkedListNode<HighScoreElement> node = highscoreCampaign.First;
        int i = 0;
        while (node != null)
        {
            i++;
            PlayerPrefs.SetString("Highscore" + i + "CampaignName", node.Value.user);
            PlayerPrefs.SetString("Highscore" + i + "CampaignScore", node.Value.score.ToString());
            node = node.Next;
        }
        PlayerPrefs.Save();
    }

    public static void UpdateHighscoreInfinite()
    {
        LinkedListNode<HighScoreElement> node = highscoreInfinite.First;
        int i = 0;
        while (node != null)
        {
            i++;
            PlayerPrefs.SetString("Highscore" + i + "InfiniteName", node.Value.user);
            PlayerPrefs.SetString("Highscore" + i + "InfiniteScore", node.Value.score.ToString());
            node = node.Next;
        }
        PlayerPrefs.Save();
    }

    public static long MinimumHighscoreRequirement()
    {
        if (highscoreCampaign.Count < 10)
            return 0;
        else
            return highscoreCampaign.Last.Value.score + 1;
    }

    public static long MinimumHighscoreRequirementInfinite()
    {
        if (highscoreInfinite.Count < 10)
            return 0;
        else
            return highscoreInfinite.Last.Value.score + 1;
    }


    private static void IncreaseModifier(Modifier mod) 
    {
        switch (mod)
        {
            case Modifier.Soldier: soldierModifier++; break;
            case Modifier.Obstacle: obstacleModifier++; break;
            case Modifier.Pit: pitModifier++; break;
            case Modifier.Catapult: catapultModifier++; break;
            case Modifier.Jump:
                {
                    if (jumpLengthModifier < 1.5f)
                        jumpLengthModifier += 0.05f; 
                    break;
                }
            case Modifier.MoveSpeed:
                {
                    if (moveSpeedModifier < 1.5f)
                        moveSpeedModifier += 0.05f;
                    break;
                }
            case Modifier.SlowDown:
                {
                    if (moveSpeedModifier < 1.5f)
                        moveSpeedModifier += 0.05f;
                    break;
                }
        }
    }
}
