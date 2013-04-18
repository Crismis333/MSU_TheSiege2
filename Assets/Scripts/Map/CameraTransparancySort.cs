using UnityEngine;
using System.Collections;

public class CameraTransparancySort : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //camera.transparencySortMode = TransparencySortMode.Orthographic;
        //camera.layerCullSpherical
        GameObject o = GameObject.Find("Map 1");
        foreach (Material m in o.renderer.materials)
            m.SetPass(0);
	}
}
