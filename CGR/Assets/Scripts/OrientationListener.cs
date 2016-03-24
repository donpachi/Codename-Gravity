using UnityEngine;
using System.Collections;

public class OrientationListener : MonoBehaviour {
    public Vector2 DEFAULT_ACCELEROMETER_VECTOR = new Vector2(1, 0);
    public Vector2 DEFAULT_ACCELEROMETER_PERP_VECTOR = new Vector2(0, -1);
    public enum Orientation { PORTRAIT, LANDSCAPE_LEFT, INVERTED_PORTRAIT, LANDSCAPE_RIGHT }

    private float AccelerometerUpdateInterval;
    private float LowPassKernalWidthInSeconds;
    private float LowPassFilterFactor;
    private Vector3 lowPassValue = Vector3.zero;
    private static Vector2 downVector, rightVector;
    private Orientation currentorientation;
    private Vector2 gVector;
    private Vector2 gVectorPerpendicularCW;

    public static OrientationListener instanceOf;

    void Awake()
    {
        if (instanceOf == null)
        {
            DontDestroyOnLoad(gameObject);
            instanceOf = this;
        }
        else if (instanceOf != this)
        {
            Destroy(gameObject);
        }
        initialize();
    }

    // Use this for initialization
    void Start () {
        //initialize();
        PolarizeAccelerometerValues(ApplyLowPassFilterAccelerometer(LowPassFilterFactor));
    }
	
	// Update is called once per frame
	void Update () {
	    PolarizeAccelerometerValues(ApplyLowPassFilterAccelerometer(LowPassFilterFactor));
	}

    void PolarizeAccelerometerValues(Vector2 filteredVector)
    {
        if (Mathf.Abs(filteredVector.x) > Mathf.Abs(filteredVector.y))
        {
            if (filteredVector.x < 0)
            {
                downVector.x = -1;
                downVector.y = 0;
                rightVector.x = 0;
                rightVector.y = -1;
                currentorientation = Orientation.LANDSCAPE_LEFT;
            }
            else
            {
                downVector.x = 1;
                downVector.y = 0;
                rightVector.x = 0;
                rightVector.y = 1;
                currentorientation = Orientation.LANDSCAPE_RIGHT;
            }
        }
        else if (Mathf.Abs(filteredVector.x) <= Mathf.Abs(filteredVector.y))
        {
            if (filteredVector.y >= 0)
            {
                downVector.y = 1;
                downVector.x = 0;
                rightVector.y = 0;
                rightVector.x = -1;
                currentorientation = Orientation.INVERTED_PORTRAIT;
            }
            else
            {
                downVector.y = -1;
                downVector.x = 0;
                rightVector.y = 0;
                rightVector.x = 1;
                currentorientation = Orientation.PORTRAIT;
            }
        }
    }

    public Orientation currentOrientation()
    {
        return currentorientation;
    }

    Vector2 ApplyLowPassFilterAccelerometer(float filter)
    {
        float xfilter = Mathf.Lerp(lowPassValue.x, Input.acceleration.x, filter);
        float yfilter = Mathf.Lerp(lowPassValue.y, Input.acceleration.y, filter);
        lowPassValue = new Vector2(xfilter, yfilter);
        return lowPassValue;
    }

    void initialize()
    {
        downVector = DEFAULT_ACCELEROMETER_VECTOR;
        rightVector = DEFAULT_ACCELEROMETER_PERP_VECTOR;
        AccelerometerUpdateInterval = 1.0f / 60.0f;
        LowPassKernalWidthInSeconds = 0.1f;
        LowPassFilterFactor = AccelerometerUpdateInterval / LowPassKernalWidthInSeconds;
        lowPassValue = DEFAULT_ACCELEROMETER_VECTOR;
    }

    public void saveGravityVector(Vector2 gravVector)
    {
        gVector = gravVector;
        gVectorPerpendicularCW = new Vector2(gravVector.y * -1, gravVector.x);
    }

    public Vector2 getWorldDownVector()
    {
        return gVector;
    }

    public Vector2 getWorldUpVector()
    {
        return gVector * -1;
    }

    public Vector2 getWorldLeftVector()
    {
        return gVectorPerpendicularCW * -1;
    }

    public Vector2 getWorldRightVector()
    {
        return gVectorPerpendicularCW;
    }

    public Vector2 getRelativeDownVector()
    {
        return downVector;
    }

    public Vector2 getRelativeUpVector()
    {
        return downVector * -1;
    }

    public Vector2 getRelativeRightVector()
    {
        return rightVector;
    }

    public Vector2 getRelativeLeftVector()
    {
        return rightVector * -1;
    }

    
}
 