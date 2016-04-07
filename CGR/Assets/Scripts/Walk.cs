using UnityEngine;
using System.Collections;
using UnityEngine.UI;       //FOR DEBUG REMOVE LATER

public class Walk : MonoBehaviour {

    public float THRUST = 0.5f;
    public float INAIRTHRUST = 0.1f;
    public float MAXSPEED = 10f;
    public float MAXFLOATSPEED = 2f;

    private Rigidbody2D rBody;
    private Animator anim;
    private float minWalkSpeed = 0.1f;
    private TouchController.TouchLocation _touchLocation;
    private PinchtoZoom cameraZoom;

    // Use this for initialization
    void Start ()
    {
        rBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        _touchLocation = TouchController.TouchLocation.NONE;
        cameraZoom = GameObject.Find("Main Camera").GetComponent<PinchtoZoom>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (rBody.velocity.magnitude < minWalkSpeed)
            anim.SetBool("Moving", false);
	}

    void applyMoveForce(float force)
    {
        switch (_touchLocation)
        {
            case TouchController.TouchLocation.LEFT:
                rBody.AddForce(OrientationListener.instanceOf.getWorldLeftVector() * force, ForceMode2D.Impulse);
                break;
            case TouchController.TouchLocation.RIGHT:
                rBody.AddForce(OrientationListener.instanceOf.getWorldRightVector() * force, ForceMode2D.Impulse);
                break;
            case TouchController.TouchLocation.NONE:
                break;
        }
    }

    /// <summary>
    /// Listens for every screen touch, just returns when its more than 1
    /// </summary>
    /// <param name="data"></param>
    void screenTouched(TouchInstanceData data)
    {
        if (TouchController.Instance.GetTouchCount() > 1)
            return;

        _touchLocation = data.touchLocation;

        if (GetComponent<GroundCheck>().InAir && rBody.velocity.magnitude < MAXFLOATSPEED && !cameraZoom.Zooming)
        {
            applyMoveForce(INAIRTHRUST);
        }
        else if (rBody.velocity.magnitude < MAXSPEED && !GetComponent<GroundCheck>().InAir && !cameraZoom.Zooming)
        {
            applyMoveForce(THRUST);
            if (_touchLocation != TouchController.TouchLocation.NONE)
                anim.SetBool("Moving", true);
        }

    }

    void OnEnable()
    {
        TouchController.ScreenTouched += screenTouched;
    }

    void OnDisable()
    {
        TouchController.ScreenTouched -= screenTouched;
    }

}
