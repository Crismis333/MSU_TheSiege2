using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class HeroAttack : MonoBehaviour
{

    public List<GameObject> AttackList;

    public float SlowTime = 2;
    public float SlowAmount = 2;

    private int selectedIndex;

    private GUIScript GUI;

    private bool charging;
    private float chargePercent, lastChargePercent, chargeTime;
    public float MaxCharge = 1.2f; // Time in seconds to fully charge
    public const float MIN_CHARGE = 0.1f;
    private HeroMovement hm;

    private List<GameObject> hitableEnemies;

    private float a, b, c;

    private Animator anim;

    // Use this for initialization
    void Start()
    {
        if (ObstacleController.PLAYER != null)
        {
            hm = ObstacleController.PLAYER.GetComponent<HeroMovement>();
            anim = ObstacleController.PLAYER.GetComponent<Animator>();
            anim.SetInteger("AttackingState", 0);
            anim.SetLayerWeight(1, 1);
            anim.SetBool("Attacking", false);
        }
        AttackList = new List<GameObject>();
        GUI = GameObject.Find("GUI").GetComponent<GUIScript>();
        hitableEnemies = new List<GameObject>();
        //  Time.timeScale = 0.5f;
        LstSquQuadRegr solvr = new LstSquQuadRegr();
        solvr.AddPoints(0, 0);
        solvr.AddPoints(MaxCharge / 2, 1);
        solvr.AddPoints(MaxCharge, 0);
        a = (float)solvr.aTerm();
        b = (float)solvr.bTerm();
        c = (float)solvr.cTerm();

        //print("a: " + a + ", b: " + b + ", c: " + c);
    }

    void KillEnemy(GameObject enemy)
    {
        Vector3 exppos = ObstacleController.PLAYER.transform.position;
        exppos.y = 1;
        enemy.GetComponent<EnemyAttack>().AddExplosion((ObstacleController.PLAYER.GetComponent<HeroMovement>().CurrentSpeed / 4) * (100 + 500 * chargePercent), exppos);

        enemy.GetComponent<EnemyAttack>().KillSelf(chargePercent);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("ReleaseBox"))
        {
            //    print("Enter release box");

            GameObject enemy = other.transform.parent.gameObject;
            //enemy.GetComponent<Animator>().SetInteger("State", 1);
            //enemy.GetComponent<Animator>().enabled = false;
            if (!hitableEnemies.Contains(enemy))
            {
                hitableEnemies.Add(enemy);
            }
        }
        if (other.gameObject.name.Equals("EnemyBox"))
        {
            Vector3 exppos = ObstacleController.PLAYER.transform.position;
            exppos.y = 1;
            other.transform.parent.GetComponent<EnemyAttack>().
                AddExplosion(ObstacleController.PLAYER.GetComponent<HeroMovement>().CurrentSpeed / 4 * 400, exppos);
            //  other.transform.parent.GetComponent<EnemyAttack>().KillSelf();
            gameObject.GetComponent<HeroMovement>().SlowHero(SlowTime, SlowAmount);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Equals("ReleaseBox"))
        {
            //   print("Leaving release box");

            GameObject enemy = other.transform.parent.gameObject;
            if (hitableEnemies.Contains(enemy))
            {
                hitableEnemies.Remove(enemy);
            }
        }
    }

    float ChargeSmoothing(float time)
    {
        if (time > 0 && time < MaxCharge)
        {
            return (a * time * time + b * time + c);
        }
        return 0;
    }

    void Update()
    {
        if (hm == null)
        {
            hm = ObstacleController.PLAYER.GetComponent<HeroMovement>();
            anim = ObstacleController.PLAYER.GetComponent<Animator>();
            anim.SetLayerWeight(1, 1);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            //print("Fire1 down");
            // Start charging attack
            charging = true;
            chargePercent = 0;
            chargeTime = 0;
            GUI.BarActive = true;

            anim.SetBool("Attacking", true);
            anim.SetInteger("AttackState", 1);
        }

        if (charging)
        {
            if (chargeTime > MaxCharge * 1.1f)
            {
                // Stop attacking - overcharged
                charging = false;
                GUI.ResetBar();
                GUI.BarActive = false;
            }
            else
            {
                // Continue charging attack
                chargeTime += Time.deltaTime;

                //if (false)
                //{
                //    chargePercent = chargeTime;
                //}
                //else
                //{
                lastChargePercent = chargePercent;
                chargePercent = ChargeSmoothing(chargeTime);
                //}
                //print("ChargePercent: " + chargePercent);
                float percent = chargePercent * 100;
                if (lastChargePercent > chargePercent)
                {
                    percent = (1 + (1 - chargePercent)) * 100;
                }

                //print("chargePercent: " + chargePercent + ", percent: " + percent + ", time: " + chargeTime);
                GUI.engagePercent = percent;
            }
        }

        if (Input.GetButtonUp("Fire1"))
        {
            //print("Fire1 up");
            // Release attack - hit if in a collider box

            anim.SetBool("Attacking", false);
            anim.SetInteger("AttackState", 0);

            GUI.ResetBar();
            GUI.BarActive = false;
            if (charging)
            {
                if (hitableEnemies.Count > 0)
                {

                    HitAccuracy ha = new HitAccuracy();
                    ha.Accuracy = chargePercent;
                    ha.NumberOfHits = hitableEnemies.Count;
                    ha.CurrentSpeed = hm.CurrentSpeed;

                    //print(ha.ToString());

                    GUI.AddHit(ha);
                    GUI.AttackFeedback(chargePercent);

                    if (chargePercent > MIN_CHARGE)
                    {
                        foreach (GameObject enemy in hitableEnemies)
                        {
                            KillEnemy(enemy);
                        }
                    }
                    hitableEnemies.Clear();
                }

            }
            charging = false;
        }
    }
}

public class LstSquQuadRegr
{
    /* instance variables */
    ArrayList pointArray = new ArrayList();
    private int numOfEntries;
    private double[] pointpair;

    /*constructor */
    public LstSquQuadRegr()
    {
        numOfEntries = 0;
        pointpair = new double[2];
    }

    /*instance methods */
    /// <summary>
    /// add point pairs
    /// </summary>
    /// <param name="x">x value</param>
    /// <param name="y">y value</param>
    public void AddPoints(double x, double y)
    {
        pointpair = new double[2];
        numOfEntries += 1;
        pointpair[0] = x;
        pointpair[1] = y;
        pointArray.Add(pointpair);
    }

    /// <summary>
    /// returns the a term of the equation ax^2 + bx + c
    /// </summary>
    /// <returns>a term</returns>
    public double aTerm()
    {
        if (numOfEntries < 3)
        {
            throw new InvalidOperationException(
               "Insufficient pairs of co-ordinates");
        }
        //notation sjk to mean the sum of x_i^j*y_i^k. 
        double s40 = getSx4(); //sum of x^4
        double s30 = getSx3(); //sum of x^3
        double s20 = getSx2(); //sum of x^2
        double s10 = getSx();  //sum of x
        double s00 = numOfEntries;
        //sum of x^0 * y^0  ie 1 * number of entries

        double s21 = getSx2y(); //sum of x^2*y
        double s11 = getSxy();  //sum of x*y
        double s01 = getSy();   //sum of y

        //a = Da/D
        return (s21 * (s20 * s00 - s10 * s10) -
                s11 * (s30 * s00 - s10 * s20) +
                s01 * (s30 * s10 - s20 * s20))
                /
                (s40 * (s20 * s00 - s10 * s10) -
                 s30 * (s30 * s00 - s10 * s20) +
                 s20 * (s30 * s10 - s20 * s20));
    }

    /// <summary>
    /// returns the b term of the equation ax^2 + bx + c
    /// </summary>
    /// <returns>b term</returns>
    public double bTerm()
    {
        if (numOfEntries < 3)
        {
            throw new InvalidOperationException(
               "Insufficient pairs of co-ordinates");
        }
        //notation sjk to mean the sum of x_i^j*y_i^k.
        double s40 = getSx4(); //sum of x^4
        double s30 = getSx3(); //sum of x^3
        double s20 = getSx2(); //sum of x^2
        double s10 = getSx();  //sum of x
        double s00 = numOfEntries;
        //sum of x^0 * y^0  ie 1 * number of entries

        double s21 = getSx2y(); //sum of x^2*y
        double s11 = getSxy();  //sum of x*y
        double s01 = getSy();   //sum of y

        //b = Db/D
        return (s40 * (s11 * s00 - s01 * s10) -
                s30 * (s21 * s00 - s01 * s20) +
                s20 * (s21 * s10 - s11 * s20))
                /
                (s40 * (s20 * s00 - s10 * s10) -
                 s30 * (s30 * s00 - s10 * s20) +
                 s20 * (s30 * s10 - s20 * s20));
    }

    /// <summary>
    /// returns the c term of the equation ax^2 + bx + c
    /// </summary>
    /// <returns>c term</returns>
    public double cTerm()
    {
        if (numOfEntries < 3)
        {
            throw new InvalidOperationException(
                       "Insufficient pairs of co-ordinates");
        }
        //notation sjk to mean the sum of x_i^j*y_i^k.
        double s40 = getSx4(); //sum of x^4
        double s30 = getSx3(); //sum of x^3
        double s20 = getSx2(); //sum of x^2
        double s10 = getSx();  //sum of x
        double s00 = numOfEntries;
        //sum of x^0 * y^0  ie 1 * number of entries

        double s21 = getSx2y(); //sum of x^2*y
        double s11 = getSxy();  //sum of x*y
        double s01 = getSy();   //sum of y

        //c = Dc/D
        return (s40 * (s20 * s01 - s10 * s11) -
                s30 * (s30 * s01 - s10 * s21) +
                s20 * (s30 * s11 - s20 * s21))
                /
                (s40 * (s20 * s00 - s10 * s10) -
                 s30 * (s30 * s00 - s10 * s20) +
                 s20 * (s30 * s10 - s20 * s20));
    }

    public double rSquare() // get r-squared
    {
        if (numOfEntries < 3)
        {
            throw new InvalidOperationException(
               "Insufficient pairs of co-ordinates");
        }
        // 1 - (residual sum of squares / total sum of squares)
        return 1 - getSSerr() / getSStot();
    }


    /*helper methods*/
    private double getSx() // get sum of x
    {
        double Sx = 0;
        foreach (double[] ppair in pointArray)
        {
            Sx += ppair[0];
        }
        return Sx;
    }

    private double getSy() // get sum of y
    {
        double Sy = 0;
        foreach (double[] ppair in pointArray)
        {
            Sy += ppair[1];
        }
        return Sy;
    }

    private double getSx2() // get sum of x^2
    {
        double Sx2 = 0;
        foreach (double[] ppair in pointArray)
        {
            Sx2 += Math.Pow(ppair[0], 2); // sum of x^2
        }
        return Sx2;
    }

    private double getSx3() // get sum of x^3
    {
        double Sx3 = 0;
        foreach (double[] ppair in pointArray)
        {
            Sx3 += Math.Pow(ppair[0], 3); // sum of x^3
        }
        return Sx3;
    }

    private double getSx4() // get sum of x^4
    {
        double Sx4 = 0;
        foreach (double[] ppair in pointArray)
        {
            Sx4 += Math.Pow(ppair[0], 4); // sum of x^4
        }
        return Sx4;
    }

    private double getSxy() // get sum of x*y
    {
        double Sxy = 0;
        foreach (double[] ppair in pointArray)
        {
            Sxy += ppair[0] * ppair[1]; // sum of x*y
        }
        return Sxy;
    }

    private double getSx2y() // get sum of x^2*y
    {
        double Sx2y = 0;
        foreach (double[] ppair in pointArray)
        {
            Sx2y += Math.Pow(ppair[0], 2) * ppair[1]; // sum of x^2*y
        }
        return Sx2y;
    }

    private double getYMean() // mean value of y
    {
        double y_tot = 0;
        foreach (double[] ppair in pointArray)
        {
            y_tot += ppair[1];
        }
        return y_tot / numOfEntries;
    }

    private double getSStot() // total sum of squares
    {
        //the sum of the squares of the differences between 
        //the measured y values and the mean y value
        double ss_tot = 0;
        foreach (double[] ppair in pointArray)
        {
            ss_tot += Math.Pow(ppair[1] - getYMean(), 2);
        }
        return ss_tot;
    }

    private double getSSerr() // residual sum of squares
    {
        //the sum of the squares of te difference between 
        //the measured y values and the values of y predicted by the equation
        double ss_err = 0;
        foreach (double[] ppair in pointArray)
        {
            ss_err += Math.Pow(ppair[1] - getPredictedY(ppair[0]), 2);
        }
        return ss_err;
    }

    private double getPredictedY(double x)
    {
        //returns value of y predicted by the equation for a given value of x
        return aTerm() * Math.Pow(x, 2) + bTerm() * x + cTerm();
    }
}
