using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Location))]
public class LocationClick : MonoBehaviour {

    [HideInInspector]
    public static LocationClick currentActive;

    private float countdown;
    private MapGui mapg;
	// Update is called once per frame
    void OnMouseDown()
    {
        if (!CurrentGameState.hero.completed)
            return;
        if (mapg.enabled && this.GetComponent<Location>().isChildOfCurrent())
        {
            mapg.current_location = this.GetComponent<Location>();
            mapg.keyLocation = this.GetComponent<Location>().positionInParent;
            mapg.ResetScroll();
            mapg.PlayLocationClick();
            CurrentGameState.hero.LookAtLoc(this.GetComponent<Location>());

            if (currentActive == this && countdown > 0)
            {
                currentActive = null;
                countdown = 0;
                mapg.Battle_Pressed();
            }
            else
            {
                currentActive = this;
                countdown = 0.2f;
            }
        }
	}

    void Start()
    {
        mapg = Camera.mainCamera.GetComponent<MapGui>();
        countdown = 0;
    }

    void Update()
    {
        if (countdown > 0)
            countdown -= Time.deltaTime;
        else
            countdown = 0;
    }
}
