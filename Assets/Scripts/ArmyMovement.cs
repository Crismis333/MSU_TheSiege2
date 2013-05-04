using UnityEngine;
using System.Collections;

public class ArmyMovement : MonoBehaviour
{
    private float pZ = 0;
    private float aZ = 0;

    private float dZ = 0;

    private float speedMod = 1.0f;
    [HideInInspector]
    public float InfSpeedMod = 1;

    public bool AreClose, GetTrampled;
    private float levelLength;

    private HeroMovement hm;

    public float VisibleDistance = 4.0f;
    private float closeTimer;
    public float MaxClose = 5.0f;
    private float trampleSpeed;

    void Start()
    {
        if (ObstacleController.PLAYER != null)
        {
            hm = ObstacleController.PLAYER.GetComponent<HeroMovement>();
        }
        levelLength = LevelCreator.LengthConverter(LevelCreator.LEVEL_LENGTH) * 64 - 32;
    }

    public void Trample()
    {
        if (AreClose)
        {
            GetTrampled = true;
            trampleSpeed = 1.2f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (hm == null && ObstacleController.PLAYER != null)
        {
            hm = ObstacleController.PLAYER.GetComponent<HeroMovement>();
        }
        pZ = ObstacleController.PLAYER.transform.position.z;
        aZ = transform.position.z;

        if (pZ > levelLength && !LevelCreator.INF_MODE)
        {
            return;
        }

        dZ = pZ - aZ;

        speedMod = 1.05f * InfSpeedMod;

        if (!AreClose)
        {
            if (dZ < VisibleDistance)
            {
                AreClose = true;
                closeTimer = 0;
            }
            else
            {
                AreClose = false;
                GetTrampled = false;
            }
                
        }
        if (dZ <= 0)
        {
            if (!hm.dead)
                hm.Kill();
        }

        if (AreClose)
        {
            if (closeTimer < MaxClose)
            {
                closeTimer += Time.deltaTime;
                speedMod = 1;
            }
            else
            {
                GetTrampled = true;
                trampleSpeed = 1.05f;
                AreClose = false;
            }
        }

        if (GetTrampled)
        {
            speedMod = trampleSpeed;
            transform.position += (new Vector3(0, 0, hm.MoveSpeed * speedMod) * Time.deltaTime);
        }
        else
        {
            transform.position += (new Vector3(0, 0, AreClose ? Mathf.Min(hm.MoveSpeed, hm.CurrentSpeed) : hm.MoveSpeed * speedMod) * Time.deltaTime);
        }
       
    }
}
