using UnityEngine;
using System.Collections;

public class OrientationListener : MonoBehaviour {
    public Vector2 DEFAULT_ACCELEROMETER_VECTOR = new Vector2(0, -1);
    public Vector2 DEFAULT_ACCELEROMETER_PERP_VECTOR = new Vector2(1, 0);
    public enum Orientation { PORTRAIT, LANDSCAPE_LEFT, INVERTED_PORTRAIT, LANDSCAPE_RIGHT }

    private float AccelerometerUpdateInterval;
    private float LowPassKernalWidthInSeconds;
    private float LowPassFilterFactor;
    private Vector3 lowPassValue = Vector3.zero;
    private static Vector2 downVector, rightVector;
    private int orientationInt;

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
    }

    // Use this for initialization
    void Start () {
        initialize();
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
                orientationInt = 1;
            }
            else
            {
                downVector.x = 1;
                downVector.y = 0;
                rightVector.x = 0;
                rightVector.y = 1;
                orientationInt = 3;
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
                orientationInt = 2;
            }
            else
            {
                downVector.y = -1;
                downVector.x = 0;
                rightVector.y = 0;
                rightVector.x = 1;
                orientationInt = 0;
            }
        }
    }

    public Orientation currentOrientation()
    {
        return (Orientation)orientationInt;
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
 