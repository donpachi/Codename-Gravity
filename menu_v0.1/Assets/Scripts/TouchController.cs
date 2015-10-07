using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;       //FOR DEBUG REMOVE LATER


//Will only follow the first two fingers on screen
//First finger on screen will be the priority for movement. Second finger can be used for swipe up and down.
//
//Manager has a deadzone for movement
//

public class TouchController : MonoBehaviour {

    public static TouchController Instance;
    public float SwipeTime = 1.0f;     //Max time for movement check of a swipe.
    public float DeadZoneMagnitude = 20;      //DeadZone of swipe movement calculated as ratio between this value and screen height
    public enum SwipeDirection {UP, DOWN, LEFT, RIGHT}
    public enum TouchLocation {LEFT, RIGHT, NONE}

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
        initTouchDataArray();
        deadZone = height / DeadZoneMagnitude;
	}

    void initTouchDataArray()
    {
        touchDataArray = new TouchInstanceData[MAXTOUCHES];
        for (int i = 0; i < touchDataArray.Length; i++)
        {
            touchDataArray[i] = new TouchInstanceData();
        }
    }
	
	void Update () {
        if (Input.touchCount > 0 && Input.touchCount <= MAXTOUCHES)
        {
            //GameObject.Find("ScreenText").GetComponent<Text>().text = "";
            for (int i = 0; i < Input.touchCount; ++i)        //only loops for the number of max touches
            {
                //GameObject.Find("ScreenText").GetComponent<Text>().text += "Key and Index Of Key: \n Key: " + Input.touches[i].fingerId + "    i: " + i + "\n";
                if (Input.touches[i].fingerId < MAXTOUCHES)
                    processATouch(Input.touches[i], i);
                //GameObject.Find("ScreenText").GetComponent<Text>().text += "TouchLocation: " + touchDataArray[Input.touches[i].fingerId].touchLocation + " touchposition: " + Input.touches[i].position + "\n";
            }

        }
	}

    void processATouch(Touch touch, int touchOrder)
    {
        switch (touch.phase)
        {
            //Beginning of a touch, need start position, set delta time to 0
            case TouchPhase.Began:
                resetTouchData(touch, touchDataArray[touch.fingerId]);
                updateTouchLocation(touch, touchDataArray[touch.fingerId]);
                break;
            //Midpoint of a touch, need to see how far it moved, how fast it moved that distance and react accordingly
            case TouchPhase.Moved:
                TouchInstanceData data = touchDataArray[touch.fingerId];
                data.moveTime += touch.deltaTime;
                data.totalTime += touch.deltaTime;
                checkSwipe(touch, data);
                updateTouchLocation(touch, data);
                break;
            //Finger stopped or ended, swipe data resets
            case TouchPhase.Stationary:
                touchDataArray[touch.fingerId].swipeTriggered = false;
                touchDataArray[touch.fingerId].totalTime += touch.deltaTime;
                resetSwipeData(touch, touchDataArray[touch.fingerId]);
                updateTouchLocation(touch, touchDataArray[touch.fingerId]);
                break;
            case TouchPhase.Ended:
                resetTouchData(touch, touchDataArray[touch.fingerId]);
                break;
        }

    }

    //update the touch location in realation to orientation and screen position
    void updateTouchLocation(Touch touch, TouchInstanceData data)
    {
        switch (OrientationListener.instanceOf.currentOrientation())
        {
            case OrientationListener.Orientation.PORTRAIT:
                if (touch.position.x > width / 2)
                    data.touchLocation = TouchLocation.RIGHT;
                else
                    data.touchLocation = TouchLocation.LEFT;
                break;
            case OrientationListener.Orientation.INVERTED_PORTRAIT:
                if (touch.position.x < width / 2)
                    data.touchLocation = TouchLocation.RIGHT;
                else
                    data.touchLocation = TouchLocation.LEFT;
                break;
            case OrientationListener.Orientation.LANDSCAPE_LEFT:
                if (touch.position.y > height / 2)
                    data.touchLocation = TouchLocation.RIGHT;
                else
                    data.touchLocation = TouchLocation.LEFT;
                break;
            case OrientationListener.Orientation.LANDSCAPE_RIGHT:
                if (touch.position.y < height / 2)
                    data.touchLocation = TouchLocation.RIGHT;
                else
                    data.touchLocation = TouchLocation.LEFT;
                break;
        }
    }

    //Checks if the move should be a swipe
    void checkSwipe(Touch touch, TouchInstanceData data)
    {
        if (!data.swipeTriggered && data.moveTime < SwipeTime)
        {
            data.DeltaFromSwipe = touch.position - data.swipeOriginPosition;
            if (data.DeltaFromSwipe.magnitude > deadZone)
            {
                //movement has been decieded as swipe, fire the corresponding event
                triggerDirection(data);
                data.swipeTriggered = true;
                resetSwipeData(touch, data);
            }
        }
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

    void resetTouchData(Touch touch, TouchInstanceData data)
    {
        data.StartPosition = touch.position;
        data.swipeOriginPosition = touch.position;
        data.touchLocation = TouchLocation.NONE;
        data.moveTime = 0;
        data.totalTime = 0;
        data.swipeTriggered = false;
    }

    //returns the touch direction
    public TouchLocation getTouchDirection()
    {
        if (Input.touchCount == 1)
            return touchDataArray[Input.touches[0].fingerId].touchLocation;

        return TouchLocation.NONE;
    }

}

class TouchInstanceData
{
    public Vector2 StartPosition;   //the origin of the touch
    public Vector2 DeltaFromSwipe;  //the movement vector from where the swipe started
    public Vector2 swipeOriginPosition;    //The point where the swipe starts
    public float moveTime;          //how long the swipe has been moving
    public float totalTime;         //total life of touch
    public bool swipeTriggered;    //flag to prevent one swipe firing multiple events
    public TouchController.TouchLocation touchLocation; //Touch location in relation to orientation 

    public TouchInstanceData()
    {
        moveTime = 0;
        totalTime = 0;
        swipeTriggered = false;
        touchLocation = TouchController.TouchLocation.NONE;
    }                                   
}



