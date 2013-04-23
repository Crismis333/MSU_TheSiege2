using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void MenuActivation();

public class GUINavigation : MonoBehaviour {

    public int maxKeys;

    private bool started, usingMouse, movedUp, movedDown, firstGUI, activated;
    private int keySelect;
    private string mouseover;


    private Dictionary<int, MenuActivation> menuelements;

    void Addelement(int key, MenuActivation action)
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
	}
	
	// Update is called once per frame
	void Update () {
        if (!activated)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                activated = true;
            }
        }
        else
        {
            if (Input.GetKeyUp(KeyCode.Return))
            {
                activated = false;
                menuelements[keySelect]();
            }
        }
        if (!usingMouse && KeyOrMouse.MouseUsed())
        {
            usingMouse = true;
            Screen.showCursor = true;
            print("mouse used!");
        }
        if (usingMouse && KeyOrMouse.KeyUsed())
        {
            usingMouse = false;
            Screen.showCursor = false;
            print("key used!");
        }
        if (!usingMouse)
        {
            if (keySelect == -1)
            {
                if (Input.GetAxisRaw("Vertical") > 0)
                {
                    keySelect = maxKeys-1;
                    movedDown = true;
                    movedUp = false;
                }
                else if (Input.GetAxisRaw("Vertical") < 0)
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
                if (Input.GetAxisRaw("Vertical") > 0)
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
                else if (Input.GetAxisRaw("Vertical") < 0)
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
                if (mouseover.Equals("0"))
                    keySelect = 0;
                else if (mouseover.Equals("1"))
                    keySelect = 1;
                else if (mouseover.Equals("2"))
                    keySelect = 2;
                else if (mouseover.Equals("3"))
                    keySelect = 3;
            }
        }	
	}

    public static bool MouseUsed()
    {

        float mv = Input.GetAxis("Mouse Y");
        float mh = Input.GetAxis("Mouse X");
        return mv >= 0.01 || mv <= -0.01 || mh >= 0.01 || mh <= -0.01;
    }

    public static bool KeyUsed()
    {
        float kv = Input.GetAxisRaw("Vertical");
        float kh = Input.GetAxisRaw("Horizontal");
        return kv >= 0.01 || kv <= -0.01 || kh >= 0.01 || kh <= -0.01;
    }
}
