using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public delegate void MenuActivation();

public class GUINavigation : MonoBehaviour {

    public static KeyCode AButton = KeyCode.JoystickButton0;
    public static KeyCode BButton = KeyCode.JoystickButton1;
    public static KeyCode XButton = KeyCode.JoystickButton2;
    public static KeyCode YButton = KeyCode.JoystickButton3;
    public static KeyCode LBButton = KeyCode.JoystickButton4;
    public static KeyCode RBButton = KeyCode.JoystickButton5;
    public static KeyCode BackButton = KeyCode.JoystickButton6;
    public static KeyCode StartButton = KeyCode.JoystickButton7; 
    [HideInInspector]
    public int maxKeys;
    [HideInInspector]
    public int keySelect, menuKey;
    [HideInInspector]
    public bool usingMouse, movedUp, movedDown, activated, usedMenu;
    [HideInInspector]
    public string mouseover;


    private Dictionary<int, MenuActivation> menuelements;
    private bool menuDown;

    public void ClearElements()
    {
        menuelements = new Dictionary<int, MenuActivation>();
        keySelect = -1;
        menuKey = -1;
    }

    public void AddElement(int key, MenuActivation action)
    {
        menuelements.Add(key, action);
    }

    public static bool AButtonDown()
    {
        return Input.GetKeyDown(AButton);
    }

    public static bool AButtonUp()
    {
        return Input.GetKeyUp(AButton);
    }

    public static bool AButtonState()
    {
        return Input.GetKey(AButton);
    }

    public static bool BButtonDown()
    {
        return Input.GetKeyDown(BButton);
    }

    public static bool BButtonUp()
    {
        return Input.GetKeyUp(BButton);
    }

    public static bool BButtonState()
    {
        return Input.GetKey(BButton);
    }

    public static bool StartButtonDown()
    {
        return Input.GetKeyDown(StartButton);
    }

    public static bool StartButtonUp()
    {
        return Input.GetKeyUp(StartButton);
    }

    public static bool StartButtonState()
    {
        return Input.GetKey(StartButton);
    }

    public static bool BackButtonDown()
    {
        return Input.GetKeyDown(BackButton);
    }

    public static bool BackButtonUp()
    {
        return Input.GetKeyUp(BackButton);
    }

    public static bool LBButtonDown()
    {
        return Input.GetKeyDown(LBButton);
    }

    public static bool LBButtonUp()
    {
        return Input.GetKeyUp(LBButton);
    }

    public static bool RBButtonDown()
    {
        return Input.GetKeyDown(RBButton);
    }

    public static bool RBButtonUp()
    {
        return Input.GetKeyUp(RBButton);
    }

	// Use this for initialization
	void Start () {
        menuelements = new Dictionary<int, MenuActivation>();
        activated = false;
        usingMouse = false;
        usedMenu = false;
        movedUp = false;
        movedDown = false;
        Screen.showCursor = false;
        menuDown = false;
        keySelect = -1;
        menuKey = -1;
        mouseover = "";
	}
	
	// Update is called once per frame
	void Update () {

        print("menukey: " + menuKey + ", size: " + menuelements.Count);
        if (menuDown)
        {
            if (!StartButtonState())
                menuDown = false;
        }
        else
         if (!usedMenu)
            {
                if (StartButtonState())
                    usedMenu = true;
            }
            else
                if (!StartButtonState())
                    usedMenu = false;
        if (!usingMouse && MouseUsed())
        {
            usingMouse = true;
            Screen.showCursor = true;
            //print("mouse used!");
        }
        if (usingMouse && KeyUsed())
        {
            usingMouse = false;
            Screen.showCursor = false;
            //print("key used!");
        }
        if (!usingMouse)
        {
            if (menuKey != -1 && StartButtonDown())
            {
                    if (menuelements.ContainsKey(menuKey))
                    {
                        mouseover = "";
                        usingMouse = false;
                        movedUp = false;
                        movedDown = false;
                        usedMenu = false;
                        menuDown = true;
                        Screen.showCursor = false;
                        menuelements[menuKey]();
                        keySelect = -1;
                        menuKey = -1;
                        return;
                    }
            }
            if (!activated)
            {
                if (Input.GetKeyDown(KeyCode.Return) || AButtonDown())
                    activated = true;
            }
            else
            {
                if (Input.GetKeyUp(KeyCode.Return) || AButtonUp())
                {
                    activated = false;
                    if (menuelements.ContainsKey(keySelect))
                    {
                        mouseover = "";
                        usingMouse = false;
                        movedUp = false;
                        movedDown = false;
                        usedMenu = false;
                        menuDown = true;
                        Screen.showCursor = false;
                        menuelements[keySelect]();
                        //keySelect = -1;
                        //menuKey = -1;
                        return;
                    }
                }
            }
            if (keySelect == -1)
            {
                if (Input.GetAxisRaw("Vertical") > 0.2)
                {
                    keySelect = maxKeys - 1;
                    movedDown = true;
                    movedUp = false;
                }
                else if (Input.GetAxisRaw("Vertical") < -0.2)
                {
                    keySelect = 0;
                    movedUp = true;
                    movedDown = false;
                }
                else
                {
                    movedDown = false;
                    movedUp = false;
                }
            }
            else
            {
                if (Input.GetAxisRaw("Vertical") > 0.2)
                    if (keySelect == 0)
                    {
                        if (!movedDown)
                        {
                            keySelect = maxKeys - 1;
                            movedDown = true;
                            movedUp = false;
                        }
                    }
                    else
                    {
                        if (!movedDown)
                        {
                            keySelect--;
                            movedDown = true;
                            movedUp = false;
                        }
                    }
                else if (Input.GetAxisRaw("Vertical") < -0.2)
                    if (keySelect == maxKeys - 1)
                    {
                        if (!movedUp)
                        {
                            keySelect = 0;
                            movedUp = true;
                            movedDown = false;
                        }
                    }
                    else
                    {
                        if (!movedUp)
                        {
                            keySelect++;
                            movedUp = true;
                            movedDown = false;
                        }
                    }
                else
                {
                    movedDown = false;
                    movedUp = false;
                }
            }
        }
        else
        {
            if (mouseover != null)
            {
                try
                {
                    keySelect = int.Parse(mouseover);
                }
                catch (Exception) { }
            }
        }
	}

    public static bool MouseUsed()
    {
        float mv = Input.GetAxis("Mouse Y");
        float mh = Input.GetAxis("Mouse X");
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            return true;
        if (AButtonDown())
            return false;
        return mv >= 0.01 || mv <= -0.01 || mh >= 0.01 || mh <= -0.01;
    }

    public static bool KeyUsed()
    {
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            return false;
        if (AButtonDown())
            return true;
        float kv = Input.GetAxisRaw("Vertical");
        float kh = Input.GetAxisRaw("Horizontal");
        return kv >= 0.01 || kv <= -0.01 || kh >= 0.01 || kh <= -0.01;
    }
}
