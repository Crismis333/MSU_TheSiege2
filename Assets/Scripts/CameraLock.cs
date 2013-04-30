using UnityEngine;
using System.Collections;

public class CameraLock : MonoBehaviour
{

    public Transform Target;

    public float OffsetX;
    public float OffsetY;
    public float OffsetZ;

    private float y_bump;

    public bool UseBump = false;

    private HeroMovement hm;
    private float rageIncrease = 1.0f;
    private float rageCurrent = 0;

    // Use this for initialization
    void Start()
    {
        hm = ObstacleController.PLAYER.GetComponent<HeroMovement>();
    }

    void CameraSmoothing(float rage)
    {
        if (!hm.Charging)
        {
            if (rageCurrent < rage)
            {
                rageCurrent += rageIncrease * Time.deltaTime;
                return;
            }
            else
            {
                if (rageCurrent > 0)
                {
                    rageCurrent -= rageIncrease * Time.deltaTime;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = Target.transform.position;

        CameraSmoothing(hm.Rage);

       

        newPos.x += OffsetX;
        newPos.y += OffsetY + (UseBump ? Mathf.Sin(y_bump) * 10f * Time.deltaTime : 0);
        newPos.z += OffsetZ;

        if (rageCurrent > 0)
        {
            Vector3 pos = ObstacleController.PLAYER.transform.position;
            Vector3 cam = transform.position;

            Vector3 direction = (cam - pos).normalized;

            direction *= rageCurrent * 2;

            newPos += direction;
        }

        y_bump += 0.15f;

        transform.position = newPos;
    }
}
