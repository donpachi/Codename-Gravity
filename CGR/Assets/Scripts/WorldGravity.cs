using UnityEngine;
using System.Collections;

public class WorldGravity : MonoBehaviour {
    public float GRAVITYVALUE = 25f;
    public float GRAVITYCOOLDOWN = 5f;

    private bool gravityOnCooldown;
    private float elapsedTime;
    private OrientationListener.Orientation previousGravityDirection;

    //Event for gravity change
    public delegate void GravityEvent(OrientationListener.Orientation orientation, float timer);
    public static event GravityEvent GravityChanged;

    void triggerGravityChange(OrientationListener.Orientation orientation, float timer)
    {
        if (GravityChanged != null)
            GravityChanged(orientation, timer);           
    }

    // Use this for initialization
    void Start () {
        initialize();
	}
	
	// FixedUpdate is called once per synchronized frame
	void FixedUpdate () {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > GRAVITYCOOLDOWN) {
            gravityOnCooldown = false;
        }
        if (!gravityOnCooldown && (previousGravityDirection != OrientationListener.instanceOf.currentOrientation())){
                updateGravity();
                gravityOnCooldown = true;
                elapsedTime = 0;
            }
        
	}

    void initialize()
    {
        Physics2D.gravity = OrientationListener.instanceOf.DEFAULT_ACCELEROMETER_VECTOR * GRAVITYVALUE;
        elapsedTime = 0;
        gravityOnCooldown = false;
        previousGravityDirection = (int)OrientationListener.Orientation.PORTRAIT;
    }

    public void updateGravity()
    {
        previousGravityDirection = OrientationListener.instanceOf.currentOrientation();
        Physics2D.gravity = OrientationListener.instanceOf.getRelativeDownVector() * GRAVITYVALUE;
        triggerGravityChange(OrientationListener.instanceOf.currentOrientation(), GRAVITYCOOLDOWN);
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

}
