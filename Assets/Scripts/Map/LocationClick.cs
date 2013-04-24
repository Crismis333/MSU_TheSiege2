using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Location))]
public class LocationClick : MonoBehaviour {

    [HideInInspector]
    public static LocationClick currentActive;

    private float countdown;

	// Update is called once per frame
    void OnMouseDown()
    {
        if (!CurrentGameState.hero.completed)
            return;
        if (Camera.mainCamera.GetComponent<MapGui>().enabled && this.GetComponent<Location>().isChildOfCurrent())
        {
            Camera.mainCamera.GetComponent<MapGui>().current_location = this.GetComponent<Location>();
            Camera.mainCamera.GetComponent<MapGui>().keyLocation = this.GetComponent<Location>().positionInParent;
            Camera.mainCamera.GetComponent<MapGui>().ResetScroll();
            CurrentGameState.hero.LookAtLoc(this.GetComponent<Location>());

            if (currentActive == this && countdown > 0)
            {
                currentActive = null;
                countdown = 0;
                Camera.mainCamera.GetComponent<MapGui>().Battle_Pressed();
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
