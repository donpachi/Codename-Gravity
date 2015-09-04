﻿using UnityEngine;
using System.Collections;

//following code taken from the unity3d tutorial website


public class PinchtoZoom : MonoBehaviour
{
	public float orthoZoomSpeed = 0.1f;        // The rate of change of the orthographic size in orthographic mode.
	public float maxOrthoSize = 15f;
	private Camera playerCam;
	private float defaultOrthoSize;
	private float noTouchZoomSpeed = 0.3f;
	
	void Start(){
		playerCam = GetComponent<Camera> ();
		defaultOrthoSize = playerCam.orthographicSize;
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
			playerCam.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;
			
			// Make sure the orthographic size never drops below default.
			playerCam.orthographicSize = Mathf.Max(playerCam.orthographicSize, defaultOrthoSize);
			// make sure the orthographic size stays within limits.
			playerCam.orthographicSize = Mathf.Min(playerCam.orthographicSize, maxOrthoSize);
			
		}
		
		if (Input.touchCount < 2 && playerCam.orthographicSize > defaultOrthoSize) {
			playerCam.orthographicSize -= noTouchZoomSpeed;
		}
		
	}
}