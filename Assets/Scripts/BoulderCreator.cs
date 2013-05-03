using UnityEngine;
using System.Collections;

public class BoulderCreator : MonoBehaviour {

    private float countDown;
    private float CountDownTime;

    public GameObject Boulder;

    // Use this for initialization
    void Start()
    {
        CountDownTime = RatioToSeconds(ObstacleController.CATAPULT_RATIO);
    }

    // Update is called once per frame
    void Update()
    {
        if (countDown <= 0)
        {
            float z = ObstacleController.PLAYER.transform.position.z;

            HeroMovement hm = ObstacleController.PLAYER.GetComponent<HeroMovement>();
            if (hm.CurrentSpeed > 0)
                countDown = CountDownTime * (hm.MoveSpeed / hm.CurrentSpeed);

            if (z + 70 < ObstacleController.LEVEL_LENGTH_Z || LevelCreator.INF_MODE)
            {
                GameObject go = Instantiate(Boulder, new Vector3(Random.Range(-6, 7), 16f, z + 70), Quaternion.AngleAxis(180, Vector3.up)) as GameObject;
                go.GetComponent<Rigidbody>().AddForce(Vector3.up * 30);
                go.GetComponent<Rigidbody>().AddForce(Vector3.back * (40+Random.Range(0,30)));
                go.GetComponent<Rigidbody>().angularVelocity = new Vector3(Random.Range(-2, 3), Random.Range(-2, 3), Random.Range(-2, 3));
            }
        }
        countDown -= Time.deltaTime;
    }

    public float RatioToSeconds(int ratio)
    {
        //return (5.333f * Mathf.Pow(10, -0.125076810788137f * ratio));
        return (5.333f * Mathf.Pow(10, -0.1f * ratio));
    }

    public void RecalcTimer()
    {
        CountDownTime = RatioToSeconds(ObstacleController.CATAPULT_RATIO);
    }
}
