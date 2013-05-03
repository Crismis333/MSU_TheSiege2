using UnityEngine;
using System.Collections;

public class ArmyMovement : MonoBehaviour
{

    private float scoreCountdown = 0.1f;
    private float pZ = 0;
    private float aZ = 0;

    private float dZ = 0;

    private float speedMod = 1.0f;

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

        if (pZ > levelLength)
        {
            return;
        }

        dZ = pZ - aZ;

        //if (dZ >= 50)
        //{
        //    speedMod = (dZ / 45);
        //    speedMod = Mathf.Min(((Mathf.Max(hm.CurrentSpeed, hm.MoveSpeed) - 0.1f) / hm.MoveSpeed), speedMod);
        //}
        //else
        {
            speedMod = 1.05f;

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
                //CurrentGameState.highscorecondition = EndState.Lost;
                //Application.LoadLevel(4);
                if (!hm.dead)
                    hm.Kill();
            }
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
