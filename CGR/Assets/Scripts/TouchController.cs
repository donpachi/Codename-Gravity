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
    public float TapTimeAllowance = 0.4f;      //delta time allowance that will determine if the screen was tapped
    public float TapPositionGrace = 2f;       //movement allowed for a touch to be considered a tap
    public float stationaryTouchGrace = 0.1f;   //Time allowed for a touch to be stationary
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
    private Dictionary<int, TouchInstanceData> touchDataDictionary;



    //Event Stuff
    public delegate void SwipeEvent(SwipeDirection direction);
    public static event SwipeEvent OnSwipe;

    public delegate void TapEvent();
    public static event TapEvent OnTap;

    public delegate void TouchEvent(TouchInstanceData data);
    public static event TouchEvent ScreenTouched;

    void triggerSwipe(SwipeDirection direction)
    {
        if (OnSwipe != null)
            OnSwipe(direction);
    }

    void screenTapped()
    {
        if (OnTap != null)
            OnTap();
    }

    void screenTouched(TouchInstanceData data)
    {
        if (ScreenTouched != null)
            ScreenTouched(data);
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

        touchDataDictionary = new Dictionary<int, TouchInstanceData>();
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
        for (int i = 0; i < Input.touchCount; ++i)
        {
            //processATouch(Input.touches[i], i);
            processATouch(Input.GetTouch(i));
            //GameObject.Find("ScreenText").GetComponent<Text>().text += "TouchLocation: " + touchDataArray[Input.touches[i].fingerId].touchLocation + " touchposition: " + Input.touches[i].position + "\n";
        }
	}

    void processATouch(Touch touch)
    {
        TouchInstanceData data;
        try {
        switch (touch.phase)
        {
            //Beginning of a touch, need start position, set delta time to 0
            case TouchPhase.Began:
                data = new TouchInstanceData();
                resetTouchData(touch, data);
                updateTouchLocation(touch, data);
                data.phase = touch.phase;
                data.FingerID = touch.fingerId;
                screenTouched(data);
                touchDataDictionary.Add(touch.fingerId, data);
                break;
            //Midpoint of a touch, need to see how far it moved, how fast it moved that distance and react accordingly
            case TouchPhase.Moved:
                processMovePhase(touch);
                updateTouchLocation(touch, touchDataDictionary[touch.fingerId]);
                screenTouched(touchDataDictionary[touch.fingerId]);
                break;
            //Finger stopped, if long enough stop swipe state ends
            case TouchPhase.Stationary:
                processStationaryPhase(touch);
                updateTouchLocation(touch, touchDataDictionary[touch.fingerId]);
                screenTouched(touchDataDictionary[touch.fingerId]);
                break;
            //End of a touch, finger has lifted
            case TouchPhase.Ended:
                if (isTap(touch, touchDataDictionary[touch.fingerId]))
                {
                    screenTapped();             //fire event for a screen tap given the touch has been short enough
                }
                touchDataDictionary.Remove(touch.fingerId);
                break;
            case TouchPhase.Canceled:
                if (isTap(touch, touchDataDictionary[touch.fingerId]))
                {
                    screenTapped();
                }
                touchDataDictionary.Remove(touch.fingerId);
                break;
        }
    }
        catch(System.ArgumentException exception)
        {
            Debug.LogError("FingerID: " + touch.fingerId + "/n Data: " + touchDataDictionary.Keys.Count);
        }
    }

    void processMovePhase(Touch touch)
    {
        TouchInstanceData data = touchDataDictionary[touch.fingerId];
        data.moveTime += touch.deltaTime;
        data.totalTime += touch.deltaTime;
        if(data.phase == TouchPhase.Stationary)
        {
            data.stopTime = 0;
        }
        checkSwipe(touch, data);
        data.phase = touch.phase;
    }

    void processStationaryPhase(Touch touch)
    {
        TouchInstanceData data = touchDataDictionary[touch.fingerId];
        data.totalTime += touch.deltaTime;
        if (data.stopTime > stationaryTouchGrace)
        {
            resetSwipeData(touch, data);
            data.swipeTriggered = false;
        }
        else
        {
            data.stopTime += touch.deltaTime;
        }
        data.phase = touch.phase;
    }

    //void processATouch(Touch touch, int touchOrder)
    //{
    //    switch (touch.phase)
    //    {
    //        //Beginning of a touch, need start position, set delta time to 0
    //        case TouchPhase.Began:
    //            resetTouchData(touch, touchDataArray[touch.fingerId]);
    //            updateTouchLocation(touch, touchDataArray[touch.fingerId]);
    //            screenTouched(touchDataArray[touch.fingerId]);    //fire event for a screen touch
    //            break;
    //        //Midpoint of a touch, need to see how far it moved, how fast it moved that distance and react accordingly
    //        case TouchPhase.Moved:
    //            TouchInstanceData data = touchDataArray[touch.fingerId];
    //            data.moveTime += touch.deltaTime;
    //            data.totalTime += touch.deltaTime;
    //            checkSwipe(touch, data);
    //            updateTouchLocation(touch, data);
    //            break;
    //        //Finger stopped or ended, swipe data resets
    //        case TouchPhase.Stationary:
    //            touchDataArray[touch.fingerId].swipeTriggered = false;
    //            touchDataArray[touch.fingerId].totalTime += touch.deltaTime;
    //            resetSwipeData(touch, touchDataArray[touch.fingerId]);
    //            updateTouchLocation(touch, touchDataArray[touch.fingerId]);
    //            break;
    //        case TouchPhase.Ended:
    //            updateTouchLocation(touch, touchDataArray[touch.fingerId]);
    //            touchDataArray[touch.fingerId].totalTime += touch.deltaTime;
    //            if (isTap(touch, touchDataArray[touch.fingerId]))
    //            {
    //                screenTapped();             //fire event for a screen tap given the touch has been short enough
    //            }
    //            resetTouchData(touch, touchDataArray[touch.fingerId]);
    //            break;
    //        case TouchPhase.Canceled:
    //            if (isTap(touch, touchDataArray[touch.fingerId]))
    //            {
    //                screenTapped();
    //            }
    //            resetTouchData(touch, touchDataArray[touch.fingerId]);
    //            break;
    //    }
    //}

    /// <summary>
    /// Looks at a touch and data and determines if it should be a tap
    /// </summary>
    /// <param name="touch"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    bool isTap(Touch touch, TouchInstanceData data)
    {
        if (data.totalTime <= TapTimeAllowance)
        {
            //Debug.Log("Start: " + data.StartPosition + " end: " + touch.position + " offset: " + (data.StartPosition - touch.position).magnitude + " time: " + data.totalTime);
            if ((data.StartPosition - touch.position).magnitude < TapPositionGrace)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Updates the touch location in realation to orientation and screen position
    /// </summary>
    /// <param name="touch"></param>
    /// <param name="data"></param>
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
                if (touch.position.y < height / 2)
                    data.touchLocation = TouchLocation.RIGHT;
                else
                    data.touchLocation = TouchLocation.LEFT;
                break;
            case OrientationListener.Orientation.LANDSCAPE_RIGHT:
                if (touch.position.y > height / 2)
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
        data.stopTime = 0;
        data.totalTime = 0;
        data.swipeTriggered = false;
    }

    /// <summary>
    /// Returns the number of fingers on screen
    /// </summary>
    /// <returns></returns>
    public int GetTouchCount()
    {
        return touchDataDictionary.Count;
    }

}

public class TouchInstanceData
{
    public Vector2 StartPosition { get; set; }   //the origin of the touch
    public Vector2 DeltaFromSwipe { get; set; }  //the movement vector from where the swipe started
    public Vector2 swipeOriginPosition { get; set; }    //The point where the swipe starts
    public float moveTime { get; set; }           //how long the swipe has been moving
    public float stopTime { get; set; }          //length how long a touch is 'stationary'
    public float totalTime { get; set; }          //total life of touch
    public bool swipeTriggered { get; set; }    //flag to prevent one swipe firing multiple events
    public TouchController.TouchLocation touchLocation { get; set; }  //Touch location in relation to orientation
    public TouchPhase phase { get; set; }        //Phase of touch from last frame
    public int FingerID { get; set; }           //Id of touch

    public TouchInstanceData()
    {
        moveTime = 0;
        stopTime = 0;
        totalTime = 0;
        swipeTriggered = false;
        touchLocation = TouchController.TouchLocation.NONE;
    }                                   
}



