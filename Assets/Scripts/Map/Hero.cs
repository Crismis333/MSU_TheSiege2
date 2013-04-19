using UnityEngine;
using System.Collections;

public class Hero : MonoBehaviour {

    public bool endmove = false;
    public Location targetLoc;
    public Vector3 startLocation;
    Vector3 endLocation;
    float move;
    bool canmove;

    Quaternion startrot;

    public void LookAtLoc(Location targetLoc)
    {
        this.transform.rotation = startrot;
        Vector3 target = targetLoc.transform.position;
        target.y = transform.position.y;
        transform.LookAt(target);
        this.transform.Rotate(new Vector3(-90, 0, 0));
    }

    public void MoveToLoc(Location targetLoc, float delay)
    {
        if (endmove)
            startLocation = CurrentGameState.previousPreviousPosition;
        endLocation = targetLoc.transform.position;
        canmove = true;
        move = -delay;
    }

    void Start()
    {
        startLocation = this.transform.position;
        move = 0f;
        endmove = false;
        //this.transform.Rotate(new Vector3(-90, 0, 0));
        startrot = this.transform.rotation;
    }

    void Update()
    {
        if (canmove)
        {
            this.transform.position = Vector3.Lerp(startLocation, endLocation, move);

            if (endmove)
            {
                move += Time.deltaTime / 10;
            }
            else
            {
                move += Time.deltaTime / 2;
                if (move > 0.5 && targetLoc != null)
                    targetLoc.ActivateRigidBody();
            }
        }
    }
}
