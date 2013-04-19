using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Location))]
public class LocationClick : MonoBehaviour {

	
	// Update is called once per frame
    void OnMouseDown()
    {
        if (!CurrentGameState.hero.completed)
            return;
        if (Camera.mainCamera.GetComponent<MapGui>().enabled && this.GetComponent<Location>().isChildOfCurrent())
        {
            Camera.mainCamera.GetComponent<MapGui>().current_location = this.GetComponent<Location>();
            Camera.mainCamera.GetComponent<MapGui>().ResetScroll();
            CurrentGameState.hero.LookAtLoc(this.GetComponent<Location>());
        }
	}
}
