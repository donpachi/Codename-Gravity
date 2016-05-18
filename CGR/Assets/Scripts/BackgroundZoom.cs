using UnityEngine;
using System.Collections;

//following code taken from the unity3d tutorial website


public class BackgroundZoom : MonoBehaviour
{
    public bool Zooming;
    private float planetZoom = 0.1f / 28;
    private float currentZoom = 20.48f;
    private float defaultZoom = 20.48f;
    private float maxZoom = 19.48f;
    private Transform planets;
    private Transform space;
	
	void Start(){
        foreach (var quad in gameObject.GetComponentsInChildren<Transform>())
        {
            if (quad.name == "Space Quad")
                space = quad;
            else if (quad.name == "Planets Quad")
                planets = quad;
        }
	}
	
	void Update()
	{
		// If there are two touches on the device...
		if (Input.touchCount == 2)
		{
			// Store both touches.
			Touch touchZero = Input.GetTouch(0);
			Touch touchOne = Input.GetTouch(1);
			
			// Find the position in the previous frame of each touch.
			Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
			Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
			
			// Find the magnitude of the vector (the distance) between the touches in each frame.
			float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
			float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;
			
			// Find the difference in the distances between each frame.
			float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;


            // ... change the orthographic size based on the change in distance between the touches.
            currentZoom -= deltaMagnitudeDiff * planetZoom / 11f;
			
			// Make sure the orthographic size never drops below default.
			float newScale = Mathf.Min(currentZoom, defaultZoom);
            // make sure the orthographic size stays within limits.
            newScale = Mathf.Max(currentZoom, maxZoom);

            planets.localScale = new Vector3 (newScale, newScale, 0);
			
		}
		
		if (Input.touchCount == 0 && planets.localScale.x < defaultZoom) {
            planets.localScale = new Vector3(planets.localScale.x + planetZoom, planets.localScale.y + planetZoom, 0);
            currentZoom = planets.localScale.x + planetZoom;
			if (planets.localScale.x < (defaultZoom + planetZoom)){
                planets.localScale = new Vector3((planets.localScale.x + planetZoom), (planets.localScale.y + planetZoom), 0);
                currentZoom = (planets.localScale.x + planetZoom);
            }
		}

        if (planets.localScale.x >= defaultZoom)
        {
            Zooming = false;
            currentZoom = defaultZoom;
            planets.localScale = new Vector3(defaultZoom, defaultZoom, 0);
        }
        else
            Zooming = true;

    }
}