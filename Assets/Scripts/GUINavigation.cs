using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public delegate void MenuActivation();

public class GUINavigation : MonoBehaviour {

    public static KeyCode AButton = KeyCode.JoystickButton16;
    public static KeyCode BButton = KeyCode.JoystickButton17;
    public static KeyCode XButton = KeyCode.JoystickButton18;
    public static KeyCode YButton = KeyCode.JoystickButton19;
    public static KeyCode StartButton = KeyCode.JoystickButton9;
    public static KeyCode BackButton = KeyCode.JoystickButton10; 
    [HideInInspector]
    public int maxKeys;
    [HideInInspector]
    public int keySelect;
    [HideInInspector]
    public bool usingMouse, movedUp, movedDown, activated;
    [HideInInspector]
    public string mouseover;


    private Dictionary<int, MenuActivation> menuelements;

    public void ClearElements()
    {
        menuelements = new Dictionary<int, MenuActivation>();
    }

    public void AddElement(int key, MenuActivation action)
    {
        menuelements.Add(key, action);
    }

	// Use this for initialization
	void Start () {
        menuelements = new Dictionary<int, MenuActivation>();
        activated = false;
        usingMouse = false;
        movedUp = false;
        movedDown = false;
        Screen.showCursor = false;
        keySelect = -1;
        mouseover = "";
	}
	
	// Update is called once per frame
	void Update () {
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
            if (!activated)
            {
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Fire1"))
                {
                    activated = true;
                }
            }
            else
            {
                if (Input.GetKeyUp(KeyCode.Return) || Input.GetButtonDown("Fire1"))
                {
                    activated = false;
                    if (menuelements.ContainsKey(keySelect))
                    {
                        mouseover = "";
                        usingMouse = false;
                        movedUp = false;
                        movedDown = false;
                        Screen.showCursor = false;

                        //print("selecting key " + keySelect + ", elements: " + menuelements.Count);
                        menuelements[keySelect]();
                        keySelect = -1;
                        return;
                    }
                }
            }
            if (keySelect == -1)
            {
                if (Input.GetAxisRaw("Vertical") > 0.01)
                {
                    keySelect = maxKeys-1;
                    movedDown = true;
                    movedUp = false;
                }
                else if (Input.GetAxisRaw("Vertical") < -0.01)
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
                if (Input.GetAxisRaw("Vertical") > 0.01)
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
                else if (Input.GetAxisRaw("Vertical") < -0.01)
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
                catch (Exception) {  }
            }
        }
	}

    public static bool MouseUsed()
    {
        float mv = Input.GetAxis("Mouse Y");
        float mh = Input.GetAxis("Mouse X");
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            return true;
        return mv >= 0.01 || mv <= -0.01 || mh >= 0.01 || mh <= -0.01;
    }

    public static bool KeyUsed()
    {
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            return false;
        float kv = Input.GetAxisRaw("Vertical");
        float kh = Input.GetAxisRaw("Horizontal");
        return kv >= 0.01 || kv <= -0.01 || kh >= 0.01 || kh <= -0.01;
    }
}
