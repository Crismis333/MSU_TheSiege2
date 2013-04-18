using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Location))]
public class FigurineFader : MonoBehaviour {

    //private static float duration = 10f;
    //private float lerp;
    private int counter;
	// Use this for initialization
	void Start () {
        //lerp = 0f;
        counter = 0;
	}
	
	// Update is called once per frame
	void Update () {

        if (Camera.main.GetComponent<MapGui>().current_location == this.GetComponent<Location>() && !this.GetComponent<Location>().RB_activated)
        {
            counter++;
            //lerp = Mathf.PingPong(counter, duration) / duration;
            foreach (Renderer rs in this.gameObject.GetComponentsInChildren<Renderer>())
                foreach (Material m in rs.materials) 
                    m.SetColor("_OutlineColor", new Color(255, 255, 255, 255));
        }
        else {
            //lerp = 0f;
            counter = 0;
            //lerp = Mathf.PingPong(counter, duration) / duration;
            foreach (Renderer rs in this.gameObject.GetComponentsInChildren<Renderer>())
                foreach (Material m in rs.materials)
                    m.SetColor("_OutlineColor", new Color(255, 255, 255, 0));
        }

        //foreach(Renderer rs in this.gameObject.GetComponentsInChildren<Renderer>())
            //foreach(Material m in rs.materials)
                //rs[i].material.SetFloat("_Blend", lerp);
                //rs.material.SetColor("_OutlineColor", new Color(255,255,255,255));
        //print(lerp);
	}
}
