using UnityEngine;
using System.Collections;
 
public class ObjectFader : MonoBehaviour
{
    private float fadingOutSpeed = 0.004f;
    private float alphaValue = 0.0f;
    private Renderer[] rendererObjects;
    private bool startFade;

    void Start()
    {
        rendererObjects = GetComponentsInChildren<Renderer>();
        startFade = true;
        for (int i = 0; i < rendererObjects.Length; i++)
        {
			if (rendererObjects[i].material.HasProperty("_Color")) {
  	          	Color newColor = rendererObjects[i].material.color;
    	        newColor.a = 0f;
        	    rendererObjects[i].material.SetColor("_Color", newColor);
			}
        }
    }

	void Update()
	{
        if (startFade && alphaValue < 1)
        {
            alphaValue += Time.deltaTime * fadingOutSpeed;

            for (int i = 0; i < rendererObjects.Length; i++)
            {
				if (rendererObjects[i].material.HasProperty("_Color")) {
                	Color newColor = rendererObjects[i].material.color;
                	newColor.a = alphaValue;//Mathf.Min ( newColor.a, alphaValue ); 
                	newColor.a = Mathf.Lerp(0.0f, 255.0f, alphaValue);
                	//newColor.a = 0.2f;
                	rendererObjects[i].material.SetColor("_Color", newColor);
				}
            }
        }
        else
            for (int i = 0; i < rendererObjects.Length; i++)
            {
				if (rendererObjects[i].material.HasProperty("_Color")) {
                    Color newColor = rendererObjects[i].material.color;
                	rendererObjects[i].material.SetColor("_Color", newColor);
				}
            }
	}
 
}
