using UnityEngine;
using System.Collections;

public class WorldGravity : MonoBehaviour {
    public float GRAVITYVALUE = 25f ;
    public float GRAVITYCOOLDOWN = 5f;
    public Vector2 gVector = Vector2.down;
    public OrientationListener.Orientation CurrentGravityDirection { get; private set; }
    public static WorldGravity Instance;

    private bool gravityOnCooldown, gShiftDisabled;
    private float elapsedTime;

    //Event for gravity change
    public delegate void GravityEvent(OrientationListener.Orientation orientation, float timer);
    public static event GravityEvent GravityChanged;

    public delegate void GShitReady();
    public static event GShitReady GravityReady;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start () {
        initialize();
    }

	// FixedUpdate is called once per synchronized frame
	void FixedUpdate () {
        if (gShiftDisabled)
            return;
        elapsedTime += Time.deltaTime;
        if (elapsedTime > GRAVITYCOOLDOWN) {
            gravityOnCooldown = false;
            triggerGravityReady();
        }
        if (!gravityOnCooldown && validUpdateState()){
                updateGravity();
                gravityOnCooldown = true;
                elapsedTime = 0;
            }
	}

    void initialize()
    {
        //gVector = OrientationListener.instanceOf.DEFAULT_ACCELEROMETER_VECTOR;
        OrientationListener.instanceOf.saveGravityVector(gVector);
        Physics2D.gravity = gVector * GRAVITYVALUE; // modify so that this is modifiable when the level starts.
        elapsedTime = 0;
        gravityOnCooldown = true; gShiftDisabled = false;
        CurrentGravityDirection = OrientationListener.Orientation.PORTRAIT;
    }

    bool validUpdateState()
    {
        int diff = CurrentGravityDirection - OrientationListener.instanceOf.currentOrientation();
        if (diff == 2 || diff == -2 || CurrentGravityDirection == OrientationListener.instanceOf.currentOrientation())
        {
            return false;
        }
        
        return true;
    }

    void triggerGravityChange(OrientationListener.Orientation orientation, float timer)
    {
        if (GravityChanged != null)
            GravityChanged(orientation, timer);
    }

    void triggerGravityReady()
    {
        if (GravityReady != null)
            GravityReady();
    }

    public void updateGravity()
    {
        if (!gShiftDisabled)
        {
            CurrentGravityDirection = OrientationListener.instanceOf.currentOrientation();
            float gx = OrientationListener.instanceOf.getRelativeDownVector().x;
            float gy = OrientationListener.instanceOf.getRelativeDownVector().y;
            gVector = new Vector2(gx, gy);
            OrientationListener.instanceOf.saveGravityVector(gVector);
            Physics2D.gravity = gVector * GRAVITYVALUE;
            triggerGravityChange(OrientationListener.instanceOf.currentOrientation(), GRAVITYCOOLDOWN);
        }
    }

    public void toggleCooldown()
    {
        gravityOnCooldown = !gravityOnCooldown;
    }

    public void clearCooldown()
    {
        gravityOnCooldown = false;
    }

    public void enableCooldown()
    {
        gravityOnCooldown = true;
    }

    /// <summary>
    /// Worker method that Enables/Disables gravity shift depending on the input parameter
    /// </summary>
    /// <param name="toggle">True disables gravity shift \nFalse enables gravity shift</param>
    public void disableGravityShift(bool toggle)
    {
        if (toggle)
            gShiftDisabled = true;
        else if (!toggle)
            gShiftDisabled = false;
        else  // should never get here
            Debug.Log("Invalid parameter specified for toggle");
    }

}
