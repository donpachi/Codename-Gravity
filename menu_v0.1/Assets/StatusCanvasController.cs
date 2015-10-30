using UnityEngine;
using System.Collections;

public class StatusCanvasController : MonoBehaviour {

    //Positions looking at the phone from portrait
    private Vector3 topLeft = new Vector3(220, 435, 0);
    private Vector3 topRight = new Vector3(-220, 435, 0);
    private Vector3 bottomLeft = new Vector3(-220, -435, 0);
    private Vector3 bottomRight = new Vector3(220, -435, 0);

    private bool gravityCooldownFlag;
    private bool suctionCounterFlag;

    private float SCCounter = 0;
    private float GCCounter = 0;

	// Use this for initialization
	void Start () {
        suctionCounterFlag = false;
        gravityCooldownFlag = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (suctionCounterFlag)
            updateSCCounter();
        if (gravityCooldownFlag)
            updateGCCounter();
	}

    void updateSCCounter()
    {
        if (SCCounter <= 0)
        {
            suctionCounterFlag = false;
        }
    }

    void updateGCCounter()
    {

    }

    //update all the objects in canvas to the correct orientation
    void updateCanvasObjectOrientation(OrientationListener.Orientation orientation, float timer)
    {
        foreach (Transform child in transform)
        {
            switch (orientation)
            {
                case OrientationListener.Orientation.PORTRAIT:
                    child.rotation = Quaternion.AngleAxis(0, Vector3.forward);
                    break;
                case OrientationListener.Orientation.LANDSCAPE_RIGHT:
                    child.rotation = Quaternion.AngleAxis(90, Vector3.forward);
                    break;
                case OrientationListener.Orientation.LANDSCAPE_LEFT:
                    child.rotation = Quaternion.AngleAxis(270, Vector3.forward);
                    break;
                case OrientationListener.Orientation.INVERTED_PORTRAIT:
                    child.rotation = Quaternion.AngleAxis(180, Vector3.forward);
                    break;
            }
        }

        setGCTimer(timer);
    }

    //Set fuctions for when events thrown
    void setSCTimer(float timer)
    {
        suctionCounterFlag = true;
        SCCounter = timer;
    }

    void setGCTimer(float timer)
    {
        gravityCooldownFlag = true;
        GCCounter = timer;
    }

    //Listeners for canvas
    void OnEnable()
    {
        WorldGravity.GravityChanged += updateCanvasObjectOrientation;
        SuctionCup.SCActivated += setSCTimer;
    }

    void OnDisable()
    {
        WorldGravity.GravityChanged -= updateCanvasObjectOrientation;
        SuctionCup.SCActivated += setSCTimer;
    }

}
