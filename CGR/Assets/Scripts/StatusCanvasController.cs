using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StatusCanvasController : MonoBehaviour {

    private bool gravityCooldownFlag;
    private bool suctionCounterFlag;

    private GameObject suctionText;
    private GameObject gcText;

    private float SCCounter = 0;
    private float GCCounter = 0;

    //Event functions
    public delegate void SCTimerEvent();
    public static event SCTimerEvent SCEnd;

    public delegate void GCEvent();
    public static event GCEvent GCEnd;

    void triggerSCEnd()
    {
        if (SCEnd != null)
            SCEnd();
    }

    void triggerGCEnd()
    {
        if (GCEnd != null)
            GCEnd();
    }

	// Use this for initialization
	void Start () {
        suctionCounterFlag = false;
        gravityCooldownFlag = false;
        suctionText = transform.FindChild("SuctionText").gameObject;
        gcText = transform.FindChild("GravityCooldown").gameObject;
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
        SCCounter -= Time.deltaTime;
        
        if (SCCounter <= 0)
        {
            suctionCounterFlag = false;
            suctionText.GetComponent<Text>().text = "";
        }
        else
            suctionText.GetComponent<Text>().text = SCCounter.ToString();
    }

    void updateGCCounter()
    {
        GCCounter -= Time.deltaTime;

        if (GCCounter <= 0)
        {
            gravityCooldownFlag = false;
            gcText.GetComponent<Text>().text = "";
        }
        else
            gcText.GetComponent<Text>().text = GCCounter.ToString();
    }

    //Orientation Listener: updates all the objects in canvas to the correct orientation
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
        suctionText.GetComponent<Text>().text = timer.ToString();
    }

    void setGCTimer(float timer)
    {
        gravityCooldownFlag = true;
        GCCounter = timer;
        gcText.GetComponent<Text>().text = timer.ToString();
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
