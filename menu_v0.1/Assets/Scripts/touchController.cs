using UnityEngine;
using System.Collections;
using UnityEngine.UI;       //FOR DEBUG REMOVE LATER


//Will only follow the first two fingers on screen
//First finger on screen will be the priority for movement. Second finger can be used for swipe up and down.
//Manager has a deadzone for movement
//

public class TouchController : MonoBehaviour {

    public static TouchController Instance;
    public float SwipeTime = 1.0f;     //Max time for movement check of a swipe.
    public float DeadZoneMagnitude = 20;      //DeadZone of swipe movement calculated as ratio between this value and screen height
    public enum SwipeDirection {UP, DOWN, LEFT, RIGHT}

    //Screen width and height
    private int height;
    private int width;
    //max touches counted
    private int MAXTOUCHES = 2;
    //Array for touch data
    private TouchInstanceData[] touchDataArray;
    //Deadzone of swipe move, calculated as a ratio of the screen height
    private float deadZone;


    //Event Stuff
    public delegate void SwipeEvent(SwipeDirection direction);
    public static event SwipeEvent OnSwipe;
    
    void triggerSwipe(SwipeDirection direction)
    {
        if (OnSwipe != null)
            OnSwipe(direction);
    }


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
        deadZone = height / DeadZoneMagnitude;
	}
	
	void FixedUpdate () {
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
                touchDataArray[touch.fingerId].swipeOriginPosition = touch.position;
                touchDataArray[touch.fingerId].totalTime = 0;
                break;
            //Midpoint of a touch, need to see how far it moved, how fast it moved that distance and react accordingly
            case TouchPhase.Moved:
                TouchInstanceData data = touchDataArray[touch.fingerId];
                data.totalTime += touch.deltaTime;
                data.moveTime += touch.deltaTime;
                checkSwipe(touch, data);
                break;
            //Finger stopped or ended, swipe data resets
            case TouchPhase.Stationary:
                touchDataArray[touch.fingerId].swipeTriggered = false;
                resetSwipeData(touch, touchDataArray[touch.fingerId]);
                //GameObject.Find("MovingText").GetComponent<Text>().text = "Movement time reset";
                break;
            case TouchPhase.Ended:
                touchDataArray[touch.fingerId].swipeTriggered = false;
                resetSwipeData(touch, touchDataArray[touch.fingerId]);
                break;
        }

    }

    //Checks if the move should be a swipe
    void checkSwipe(Touch touch, TouchInstanceData data)
    {
        if (!data.swipeTriggered && data.moveTime < SwipeTime)
        {
            data.DeltaFromSwipe = touch.position - data.swipeOriginPosition;
            //GameObject.Find("ScreenText").GetComponent<Text>().text = "DeadZone Magnitude: " + deadZone
            //    + "\nSwipe instance vector: " + data.DeltaFromSwipe + "  Magnatude of swipe instance: " + data.DeltaFromSwipe.magnitude;
            if (data.DeltaFromSwipe.magnitude > deadZone)
            {
                //movement has been decieded as swipe, fire the corresponding event
                triggerDirection(data);
                data.swipeTriggered = true;
                resetSwipeData(touch, data);
            }
        }
        //GameObject.Find("MovingText").GetComponent<Text>().text = "Total Time passed from movement: " + data.moveTime;
    }

    //Sets the direction of the swipe in relation to device orientation
    //  and triggers an event accordingly
    //Takes the larger of the x, y displacement and uses as direction
    //In portrait x > 0 is right, y > 0 is up
    //Can this be more elegant?
    void triggerDirection(TouchInstanceData data)
    {
        bool swipeX;    //flag to follow x or y

        if (Mathf.Abs(data.DeltaFromSwipe.x) > Mathf.Abs(data.DeltaFromSwipe.y))
            swipeX = true;
        else
            swipeX = false;
 
        switch(OrientationListener.instanceOf.currentOrientation())
        {
            case OrientationListener.Orientation.PORTRAIT:
                if(swipeX)
                {
                    if (data.DeltaFromSwipe.x > 0)
                        triggerSwipe(SwipeDirection.RIGHT);
                    else
                        triggerSwipe(SwipeDirection.LEFT);
                }
                else
                {
                    if (data.DeltaFromSwipe.y > 0)
                        triggerSwipe(SwipeDirection.UP);
                    else
                        triggerSwipe(SwipeDirection.DOWN);
                }
                break;
            case OrientationListener.Orientation.INVERTED_PORTRAIT:
                if (swipeX)
                {
                    if (data.DeltaFromSwipe.x < 0)
                        triggerSwipe(SwipeDirection.RIGHT);
                    else
                        triggerSwipe(SwipeDirection.LEFT);
                }
                else
                {
                    if (data.DeltaFromSwipe.y < 0)
                        triggerSwipe(SwipeDirection.UP);
                    else
                        triggerSwipe(SwipeDirection.DOWN);
                }
                break;
            case OrientationListener.Orientation.LANDSCAPE_LEFT:
                if (swipeX)
                {
                    if (data.DeltaFromSwipe.x > 0)
                        triggerSwipe(SwipeDirection.UP);
                    else
                        triggerSwipe(SwipeDirection.DOWN);
                }
                else
                {
                    if (data.DeltaFromSwipe.y > 0)
                        triggerSwipe(SwipeDirection.LEFT);
                    else
                        triggerSwipe(SwipeDirection.RIGHT);
                }
                break;
            case OrientationListener.Orientation.LANDSCAPE_RIGHT:
                if (swipeX)
                {
                    if (data.DeltaFromSwipe.x < 0)
                        triggerSwipe(SwipeDirection.UP);
                    else
                        triggerSwipe(SwipeDirection.DOWN);
                }
                else
                {
                    if (data.DeltaFromSwipe.y < 0)
                        triggerSwipe(SwipeDirection.LEFT);
                    else
                        triggerSwipe(SwipeDirection.RIGHT);
                }
                break;
        }
    }

    void resetSwipeData(Touch touch, TouchInstanceData data)
    {
        data.swipeOriginPosition = touch.position;
        data.moveTime = 0;
    }

}

class TouchInstanceData
{
    public Vector2 StartPosition;   //the origin of the touch
    public Vector2 DeltaFromSwipe;  //the movement vector from where the swipe started
    public Vector2 swipeOriginPosition;    //The point where the swipe starts
    public float totalTime;         //total life of the touch
    public float moveTime;          //how long the swipe has been moving
    public bool swipeTriggered;    //flag to prevent one swipe firing multiple events

    public TouchInstanceData()
    {
        totalTime = 0;
        moveTime = 0;
        swipeTriggered = false;
    }                                   
}



