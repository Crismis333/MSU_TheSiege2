using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GUINavigation))]
[RequireComponent(typeof(PauseMenuScript))]
public class MapGui : MonoBehaviour {

    public GUISkin gSkin;
    [HideInInspector]
    public Location current_location;
    public Texture2D black;
    public Texture2D backgroundScroll;
    public Vector2 scrollOffset;
    public MapMovementController mapmove;
    [HideInInspector]
    public bool stopped, started;
    public MusicVolumeSetter music;
    public EffectVolumeSetter locationSelectSound;
    public EffectVolumeSetter levelStartSound;

    [HideInInspector]
    public int keyLocation;

    private float countdown,startcountdown;
    private bool startHero, startReset, firstGUI;

    private bool backDown, nextDown;
    Vector2 scrollPos;

    private GUINavigation guin;
    private PreviousLines prevl;

    void Map_Main()
    {
        if (current_location != null)
        {
            if (backgroundScroll != null)
            {
                GUI.BeginGroup(new Rect(Screen.width - backgroundScroll.width + scrollOffset.x, Screen.height - backgroundScroll.height + scrollOffset.y, backgroundScroll.width, backgroundScroll.height));
                GUI.DrawTexture(new Rect(0, 0, backgroundScroll.width, backgroundScroll.height), backgroundScroll);
                GUI.EndGroup();
            }

            GUI.BeginGroup(new Rect(Screen.width - 400, Screen.height - 520, Screen.width, Screen.height));

            if (current_location != null)
            {

                GUI.color = Color.black;
                GUI.skin.label.fontSize = 24;
                GUI.Label(new Rect(35, 5, 350, 100), current_location.LevelName);
                GUI.skin.label.fontSize = 10;
                GUI.color = Color.white;
                Vector2 sizeOfLabel = GUI.skin.label.CalcSize(new GUIContent(current_location.description));

                if (sizeOfLabel.y > 190)
                {
                    scrollPos = GUI.BeginScrollView(new Rect(30, 70 + 20, 330, 190), scrollPos, new Rect(0, 0, 0, sizeOfLabel.y), false, true);
                    GUI.color = Color.black;
                    GUI.Label(new Rect(0, 0, 335, sizeOfLabel.y), current_location.description);
                    GUI.EndScrollView();
                }
                else
                {
                    GUI.color = Color.black;
                    GUI.Label(new Rect(30, 70 + 20, 335, sizeOfLabel.y), current_location.description);
                }
                GUI.color = Color.white;
            }
            GUI.color = Color.black;

            GUI.Label(new Rect(15 + 15, 215 + 20 + 0 * 20 + 80, 128, 128), "Length:");
            GUI.Label(new Rect(15 + 15, 215 + 20 + 1 * 20 + 80, 128, 128), "Soldiers:");
            GUI.Label(new Rect(15 + 15, 215 + 20 + 2 * 20 + 80, 128, 128), "Obstacles:");
            GUI.Label(new Rect(15 + 15, 215 + 20 + 3 * 20 + 80, 128, 128), "Catapults:");


            GUI.Label(new Rect(15 + 100, 215 + 20 + 0 * 20 + 80, 128, 128), toNumerals(current_location.difficulty_length));
            GUI.Label(new Rect(15 + 100, 215 + 20 + 1 * 20 + 80, 128, 128), toNumerals(current_location.difficulty_soldier));
            GUI.Label(new Rect(15 + 100, 215 + 20 + 2 * 20 + 80, 128, 128), toNumerals(current_location.difficulty_obstacles));
            GUI.Label(new Rect(15 + 100, 215 + 20 + 3 * 20 + 80, 128, 128), toNumerals(current_location.difficulty_catapults));
            GUI.color = Color.white;

            if (GUI.Button(new Rect(155+40, 35 + 20 + 10 * 20+40, 150, 150), "")) 
                Battle_Pressed();

            GUI.color = Color.black;
            GUI.skin.label.fontSize = 16;
            GUI.Label(new Rect(15 + 15, 215 + 20 + 12 * 20, 128, 128), "Score:");
            GUI.color = Color.red;
            GUI.Label(new Rect(15 + 15, 215 + 20 + 12 * 20, 350, 128), "               " + CurrentGameState.currentScore);
            GUI.skin.label.fontSize = 10;
            GUI.color = Color.white;
            GUI.EndGroup();
        }
        if (stopped)
        {
            GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
            GUI.color = new Color(1, 1, 1, Mathf.Lerp(0, 1, 1 - countdown));
            music.volume = Mathf.Lerp(OptionsValues.musicVolume, 0, 1 - countdown);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), black);
            GUI.EndGroup();
        }
        else if (started)
        {
            GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
            GUI.color = new Color(1, 1, 1, Mathf.Lerp(1, 0, 1 - countdown));
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), black);
            music.volume = Mathf.Lerp(0, OptionsValues.musicVolume, 1 - countdown);
            GUI.EndGroup();
        }
        else
        {
            music.useGlobal = false;
            music.volume = OptionsValues.musicVolume;
        }
    }

    public void Battle_Pressed()
    {
        if (!CurrentGameState.hero.completed)
            return;
        if (current_location == null)
            return;
        levelStartSound.Play();
        if (current_location.difficulty_soldier < 1)
            ObstacleController.SOLDIER_RATIO = 1;
        else
            ObstacleController.SOLDIER_RATIO = current_location.difficulty_soldier;
        if (current_location.difficulty_obstacles < 1)
            ObstacleController.OBSTACLE_RATIO = 1;
        else
            ObstacleController.OBSTACLE_RATIO = current_location.difficulty_obstacles;
        if (current_location.difficulty_pits < 1)
            LevelCreator.PIT_RATIO = 1;
        else
            LevelCreator.PIT_RATIO = current_location.difficulty_pits;
        if (current_location.difficulty_catapults < 1)
            ObstacleController.CATAPULT_RATIO = 1;
        else
            ObstacleController.CATAPULT_RATIO = current_location.difficulty_catapults;

        LevelCreator.LEVEL_LENGTH = current_location.difficulty_length;
        GUIScript.SCORE = CurrentGameState.currentScore;

        CurrentGameState.previousPreviousPosition = CurrentGameState.previousPosition;
        CurrentGameState.previousPosition = current_location.transform.position;
        CurrentGameState.hero.endmove = true;
        CurrentGameState.hero.MoveToLoc(current_location,0);
        CurrentGameState.loc = null;

        Screen.lockCursor = true;
		
		LevelCreator.SIDE_MODULE_LIST.Clear();
        foreach (GameObject go in current_location.SideModules)
        {
            LevelCreator.SIDE_MODULE_LIST.Add(go.name);
        }

        LevelCreator.SPECIAL_MODULE = "";
        if (current_location.SpecialModule != null)
            LevelCreator.SPECIAL_MODULE = current_location.SpecialModule.name;
        LevelCreator.SPECIAL_PART_COUNT = current_location.SpecialPartCount;
        LevelCreator.DEFAULT_ROAD = current_location.DefaultRoad.name;

        started = false;
        stopped = true;
        countdown = 1f;
        startcountdown = 1.5f;
        //Application.LoadLevel(2);

    }

    void OnGUI()
    {
        GUI.skin = gSkin;
        if (!started && !stopped && (guin.QuitPressed() || guin.usedMenu))
        {
            this.enabled = false;
            music.useGlobal = true;
            transform.parent.gameObject.GetComponent<MapMovementController>().enabled = false;
            PauseMenuScript pms = GetComponent<PauseMenuScript>();
            pms.enabled = true;
            guin.ClearElements();
            guin.maxKeys = 5;
            guin.menuKey = 4;
            guin.AddElement(0, pms.Pause_Options);
            guin.AddElement(1, pms.Pause_Controls);
            guin.AddElement(2, pms.Pause_GiveUp);
            guin.AddElement(3, pms.Pause_Quit);
            guin.AddElement(4, pms.Pause_Back);
        }
        else
            Map_Main();
    }

    public void ResetScroll()
    {
        scrollPos = Vector2.zero;
    }

    public void PlayLocationClick()
    {
        locationSelectSound.Play();
    }

    void Start()
    {
        GameObject o = GameObject.Find("PreviousLineCreator");
        prevl = o.GetComponent<PreviousLines>();
        guin = GetComponent<GUINavigation>();
        guin.maxKeys = 0;
        backDown = false;
        nextDown = false;
        keyLocation = -1;
        firstGUI = true;
        music.useGlobal = false;
        CurrentGameState.previousScore = CurrentGameState.currentScore;
        ResetScroll();
        stopped = false;
        startReset = true;
        startHero = true;
        started = true;
        Time.timeScale = 50;
        countdown = 1f;
        startcountdown = 50f;
    }

    void Update()
    {
        float h = Input.GetAxisRaw("DPadHorizontal");
        if (startHero)
        {
            startHero = false;
            CurrentGameState.CreateHero();
            if (CurrentGameState.failedlast)
            {
                CurrentGameState.hero.transform.position = CurrentGameState.previousPosition;
                CurrentGameState.loc.ActivateRigidBody();
            }
            else
                CurrentGameState.hero.transform.position = CurrentGameState.previousPreviousPosition;
            int i = 0;
            foreach (Location loc1 in CurrentGameState.loc.locations)
            {
                loc1.positionInParent = i;
                loc1.GetComponent<CapsuleCollider>().enabled = true;
                foreach (MeshRenderer mr in loc1.GetComponentsInChildren<MeshRenderer>())
                    mr.enabled = true;
                foreach (Location loc2 in loc1.locations)
                {
                    loc2.GetComponent<CapsuleCollider>().enabled = true;
                    foreach (MeshRenderer mr in loc2.GetComponentsInChildren<MeshRenderer>())
                        mr.enabled = true;
                }

                i++;
            }
        }
        if (stopped || started)
        {
            startcountdown -= Time.deltaTime;
            if (startcountdown < 0)
                if (started)
                    countdown -= 0.02f;
                else
                    countdown -= 0.01f;


            if (started)
            {
                if (startReset)
                {
                    if (startcountdown < 0)
                    {
                        if (mapmove != null)
                            mapmove.CenterCamera(CurrentGameState.loc.transform);

                        Time.timeScale = 1;
                        if (!CurrentGameState.failedlast)
                        {
                            CurrentGameState.hero.transform.position = CurrentGameState.previousPreviousPosition;
                            CurrentGameState.hero.targetLoc = CurrentGameState.loc;
                            CurrentGameState.hero.MoveToLoc(CurrentGameState.loc, 0.5f);
                            if (CurrentGameState.locID != 0)
                                CurrentGameState.hero.LookAtLoc(CurrentGameState.loc);
                        }
                        else
                        {
                            CurrentGameState.hero.completed = true;
                            CurrentGameState.hero.LookAtLoc(CurrentGameState.loc.locations[0]);
                        }

                        prevl.Init(CurrentGameState.hero);
                        startReset = false;
                    }
                }
                if (countdown < 0)
                {
                    countdown = 0;
                    started = false;
                }

            }

            if (stopped && countdown < 0)
            {
                CurrentGameState.SetWinModifiers(current_location.modifiers, current_location.levelID);
                CurrentGameState.hero = null;
                CurrentGameState.failedlast = false;
                Application.LoadLevel(2);
            }
        }
        else
        {
            if (!guin.usingMouse)
            {
                float f = Input.GetAxisRaw("ScrollAxis");
                if (f < -0.4 || f > 0.4)
                    scrollPos.y += 3*f;
                if (!backDown && (h < -0.2 || GUINavigation.LBButtonDown()))
                {
                    if (CurrentGameState.loc.locations.Length > 1)
                        PlayLocationClick();
                    backDown = true;
                    if (keyLocation == -1 || keyLocation == 0)
                        keyLocation = CurrentGameState.loc.locations.Length - 1;
                    else
                        keyLocation--;
                    current_location = CurrentGameState.loc.locations[keyLocation];
                    ResetScroll();
                    CurrentGameState.hero.LookAtLoc(current_location);
                }
                else if (!nextDown && (h > 0.2 || GUINavigation.RBButtonDown()))
                {
                    if (CurrentGameState.loc.locations.Length > 1)
                        PlayLocationClick();
                    nextDown = true;
                    if (keyLocation == -1 || keyLocation == CurrentGameState.loc.locations.Length - 1)
                        keyLocation = 0;
                    else
                        keyLocation++;
                    current_location = CurrentGameState.loc.locations[keyLocation];
                    ResetScroll();
                    CurrentGameState.hero.LookAtLoc(current_location);
                }
                if (Input.GetKeyDown(KeyCode.Return) || GUINavigation.AButtonDown())
                {
                    Battle_Pressed();
                }
            }
        }
        if (backDown && (h >= -0.2 && !GUINavigation.LBButtonState()))
            backDown = false;
        if (nextDown && (h <= 0.2 && !GUINavigation.RBButtonState()))
            nextDown = false;
    }


    int fromFloatToInt(float val)
    {
        return ((int)((val - 1.0f) * 20));
    }

    string toNumerals(int val)
    {
        switch (val)
        {
            case 1: return "I";
            case 2: return "II";
            case 3: return "III";
            case 4: return "IV";
            case 5: return "V";
            case 6: return "VI";
            case 7: return "VII";
            case 8: return "VIII";
            case 9: return "IX";
            case 10: return "X";
            default: return "I";
        }
    }
}
