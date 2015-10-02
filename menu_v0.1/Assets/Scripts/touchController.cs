using UnityEngine;
using System.Collections;
using UnityEngine.UI;       //FOR DEBUG REMOVE LATER


//Will only follow the first two fingers on screen
//First finger on screen will be the priority for movement. Second finger can be used for swipe up and down.
//Manager has a deadzone for movement
//

public class touchController : MonoBehaviour {

    public static touchController Instance;
    public float SwipeTime;     //delta time which should determine a swipe motion vs a move
    public float DeadZone;      //DeadZone of movement

    //Screen width and height
    private int height;
    private int width;
    //delta time for update cycle
    private float updateTime;
    //max touches counted
    private int MAXTOUCHES = 2;
    //Array for touch data
    private TouchInstanceData[] touchDataArray;
    int[] a;
    
    

	void Awake () {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        height = Screen.height;
        width = Screen.width;
        touchDataArray = new TouchInstanceData[2];
        updateTime = 0;
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
                TouchInstanceData data = touchDataArray[touch.fingerId];
                data.totalTime += touch.deltaTime;
                data.moveTime += touch.deltaTime;
                data.DeltaFromStart = data.StartPosition - touch.position;
                checkSwipe(touch, data);
                data.LastPosition = touch.position;     //update last position after the swipe check happens
                break;
            //End of a touch

            case TouchPhase.Stationary:
                touchDataArray[touch.fingerId].moveTime = 0;      //reset the move time
                print("Movement time reset");
                GameObject.Find("MovingText").GetComponent<Text>().text = "Movement time reset";
                break;
            case TouchPhase.Ended:
                break;
        }

    }

    //Checks if the move should be a swipe
    void checkSwipe(Touch touch, TouchInstanceData data)
    {
        print("Time passed form movement: " + data.moveTime);
        GameObject.Find("MovingText").GetComponent<Text>().text = "Moving/n" + "Time passed form movement: " + data.moveTime;
    }

}

class TouchInstanceData
{
    public Vector2 StartPosition;
    public Vector2 DeltaFromStart;
    public Vector2 LastPosition;    //last known position
    public float totalTime;
    public float moveTime;
}



