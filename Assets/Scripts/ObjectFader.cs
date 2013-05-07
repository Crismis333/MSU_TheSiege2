using UnityEngine;
using System.Collections;
 
public class ObjectFader : MonoBehaviour
{
    private float fadingOutSpeed = 1f;
    private float alphaValue = 0.0f;
    private Renderer[] rendererObjects;
    private bool startFade;

    private bool setColor = false;

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
                	newColor.a = alphaValue;
                	newColor.a = Mathf.Lerp(0.0f, 1.0f, alphaValue);
                	rendererObjects[i].material.SetColor("_Color", newColor);
				}
            }
        }
        else if (!setColor)
        {
            for (int i = 0; i < rendererObjects.Length; i++)
            {
                if (rendererObjects[i].material.HasProperty("_Color"))
                {
                    Color newColor = rendererObjects[i].material.color;
                    rendererObjects[i].material.SetColor("_Color", newColor);
                }
            }
            setColor = true;
        }

        if (alphaValue >= 1)
            for (int i = 0; i < rendererObjects.Length; i++)
                foreach (Material m in rendererObjects[i].materials)
                    if (m.shader.name.Equals("Transparent/VertexLit with Z"))
                        m.shader = Shader.Find("Diffuse");
	}
 
}
