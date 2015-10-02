using UnityEngine;
using System.Collections;


//Will only follow the first two fingers on screen
//First finger on screen will be the priority for movement. Second finger can be used for swipe up and down.
//Manager has a deadzone for movement
//

public class touchController : MonoBehaviour {

    public static touchController controller;
    public float SwipeTime;     //delta time which should determine a swipe motion vs a move
    public float DeadZone;      //DeadZone of movement

    //Screen width and height
    private int height;
    private int width;
    //max touches counted
    private int MAXTOUCHES = 2;
    //Array for touch data
    private TouchInstanceData[] touchDataArray;
    int[] a;
    
    

	void Awake () {
        if (controller == null)
        {
            DontDestroyOnLoad(gameObject);
            controller = this;
        }
        else if (controller != this)
        {
            Destroy(gameObject);
        }
        height = Screen.height;
        width = Screen.width;
        touchDataArray = new TouchInstanceData[2];
	}
	
	void Update () {
        if (Input.touchCount > 0 && Input.touchCount <= MAXTOUCHES)
        {
            for (int i = 0; i < Input.touchCount; ++i)        //only loops for the number of max touches
            {
                processATouch(Input.touches[i]);
            }
        }
	}

    void processATouch(Touch touch)
    {
        switch (touch.phase)
        {
            //Beginning of a touch, need start position, set delta time to 0
            case TouchPhase.Began:
                touchDataArray[touch.fingerId] = new TouchInstanceData();
                touchDataArray[touch.fingerId].StartPosition = touch.position;
                touchDataArray[touch.fingerId].LastPosition = touch.position;
                touchDataArray[touch.fingerId].totalTime = 0;
                break;
            //Midpoint of a touch, need to see how far it moved, how fast it moved that distance and react accordingly
            case TouchPhase.Moved:
                

                break;
            //End of a touch, need
            case TouchPhase.Ended:
                break;
        }

    }
}

class TouchInstanceData
{
    public Vector2 StartPosition;
    public Vector2 LastPosition;    //last known position
    public float totalTime;
}



