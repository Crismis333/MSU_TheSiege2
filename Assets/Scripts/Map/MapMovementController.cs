using UnityEngine;
using System.Collections;

public class MapMovementController : MonoBehaviour {

    public Transform topLeftLimit;
    public Transform bottomRightLimit;

    bool zoomedIn;
    bool mdown;
    bool onguidown;
    int mdowncool;

    private GUINavigation guin;
    private MapGui mapg;
    private Camera cam;
    void Start() {
        cam = Camera.mainCamera;
        guin = cam.GetComponent<GUINavigation>();
        mapg = cam.GetComponent<MapGui>();
        zoomedIn = true;
        mdown = false;
        onguidown = false;
        mdowncool = 0;
    }

    public void CenterCamera(Transform t)
    {
        this.transform.position = new Vector3(t.position.x,this.transform.position.y,t.position.z);
    }

    private Vector3 ClampToMap(Vector3 pos)
    {
        Vector2 topleftpos = cam.WorldToScreenPoint(topLeftLimit.transform.position);
        Vector2 botrightpos = cam.WorldToScreenPoint(bottomRightLimit.transform.position);
        if (topleftpos.x >= 0 && botrightpos.x <= Screen.width)
        {
            Vector3 newpos = cam.WorldToScreenPoint(topLeftLimit.transform.position);
            Vector3 newpos2 = cam.WorldToScreenPoint(bottomRightLimit.transform.position);
            newpos.x += Screen.width / 2f + 20;
            newpos2.x -= Screen.width / 2f;
            newpos += newpos2;
            newpos /= 2;
            pos.x = cam.ScreenToWorldPoint(newpos).x;
        }
        if (topleftpos.x >= 0)
        {
            Vector3 newpos = cam.WorldToScreenPoint(topLeftLimit.transform.position);
            newpos.x += Screen.width / 2f + 20;
            pos.x = cam.ScreenToWorldPoint(newpos).x;
        }
        else if (botrightpos.x <= Screen.width)
        {
            Vector3 newpos = cam.WorldToScreenPoint(bottomRightLimit.transform.position);
            newpos.x -= Screen.width / 2f - 20;
            pos.x = cam.ScreenToWorldPoint(newpos).x;
        }
        if (topleftpos.y <= Screen.height && botrightpos.y >= 0)
        {
            Vector3 newpos = cam.WorldToScreenPoint(topLeftLimit.transform.position);
            Vector3 newpos2 = cam.WorldToScreenPoint(bottomRightLimit.transform.position);
            newpos.y -= Screen.height / 2f-20;
            newpos2.y += Screen.height / 2f;
            newpos += newpos2;
            newpos /= 2;
            pos.z = cam.ScreenToWorldPoint(newpos).z;
        }
        else if (topleftpos.y <= Screen.height)
        {
            Vector3 newpos = cam.WorldToScreenPoint(topLeftLimit.transform.position);
            newpos.y -= Screen.height / 2f - 20;
            pos.z = cam.ScreenToWorldPoint(newpos).z;
        }
        else if (botrightpos.y >= 0)
        {
            Vector3 newpos = cam.WorldToScreenPoint(bottomRightLimit.transform.position);
            newpos.y += Screen.height / 2f + 20;
            pos.z = cam.ScreenToWorldPoint(newpos).z;
        }
        return pos;
    }

    void Update() {
        Rect GUI_Area = new Rect(Screen.width - 400, 0, 350, 530);
        if (Screen.lockCursor)
            return;
        if (Input.GetMouseButtonDown(0) &&GUI_Area.Contains(Input.mousePosition))
            {
                onguidown = true;
                return;
            }

        if (Input.GetMouseButtonUp(0))
            onguidown = false;

        if (Input.GetMouseButton(1))
        {
            if (!GUI_Area.Contains(Input.mousePosition))
            {
                LocationClick.currentActive = null;
                mapg.current_location = null;
                mapg.ResetScroll();
            }
        }

        cam.transform.Rotate(new Vector3(30, 0, 0));
        Vector3 pos = this.transform.position;
        bool left, right, up, down;
        left = right = up = down = false;

        Vector2 topleftpos = cam.WorldToScreenPoint(topLeftLimit.transform.position);
        Vector2 botrightpos = cam.WorldToScreenPoint(bottomRightLimit.transform.position);

        float multiplier = 20.0f;

        if (topleftpos.x >= -(3 * multiplier + 30) && botrightpos.x <= Screen.width + 3 * multiplier + 30) { }
        else if (topleftpos.x >= -(3*multiplier + 30))
            left = true;
        else if (botrightpos.x <= Screen.width + 3 * multiplier + 30)
            right = true;
        if (topleftpos.y <= Screen.height + 3 * multiplier + 20 && botrightpos.y >= -(3 * multiplier + 30)) { }
        else if (topleftpos.y <= Screen.height + 3 * multiplier + 30)
            up = true;
        else if (botrightpos.y >= -(3 * multiplier + 30))
            down = true;

        pos = ClampToMap(pos);

        if (mdowncool > 0)
            mdowncool--;
        float xm, ym;
        if (guin.usingMouse)
        {
            xm = Input.GetAxis("Mouse X");
            ym = Input.GetAxis("Mouse Y");
            if (xm > 3)
                xm = 3;
            else if (xm < -3)
                xm = -3;
            if (ym > 3)
                ym = 3;
            else if (ym < -3)
                ym = -3;
        }
        else
        {
            xm = -Input.GetAxis("Horizontal");
            ym = -Input.GetAxis("Vertical");
        }

        if (!mapg.started && !mapg.stopped)
            if (guin.usingMouse)
            {
                if (Input.GetMouseButton(0) && !onguidown)
                {
                    if (xm < -0.02f && !right)
                        pos.x += xm / multiplier;
                    else if (xm > 0.02f && !left)
                        pos.x += xm / multiplier;
                    if (ym < -0.02f && !up)
                        pos.z += ym / multiplier;
                    else if (ym > 0.02f && !down)
                        pos.z += ym / multiplier;
                }
            }
            else
            {
                if (xm < -0.02f && !right)
                    pos.x += xm / multiplier;
                else if (xm > 0.02f && !left)
                    pos.x += xm / multiplier;
                if (ym < -0.02f && !up)
                    pos.z += ym / multiplier;
                else if (ym > 0.02f && !down)
                    pos.z += ym / multiplier;
            }
        pos = ClampToMap(pos);
        

        if (!mdown)
        {
            if (Input.GetMouseButtonDown(1))
            {
                zoomedIn = !zoomedIn;
                mdown = true;
                mdowncool = 5;
            }
        }
        else if (mdowncool == 0)
            mdown = false;

        this.transform.position = pos;
        cam.transform.Rotate(new Vector3(-30, 0, 0));
    }
}
