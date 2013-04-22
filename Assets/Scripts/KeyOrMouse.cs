using UnityEngine;
using System.Collections;

public class KeyOrMouse : MonoBehaviour {

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
