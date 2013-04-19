using UnityEngine;
using System.Collections;

public class CameraLock : MonoBehaviour {

    public Transform Target;

    public float OffsetX;
    public float OffsetY;
    public float OffsetZ;

    private float y_bump;

    public bool UseBump = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 newPos = Target.transform.position;

        newPos.x += OffsetX;
        newPos.y += OffsetY + (UseBump ? Mathf.Sin(y_bump) * 10f * Time.deltaTime : 0);
        newPos.z += OffsetZ;

        y_bump += 0.15f;

        transform.position = newPos;
	}
}
