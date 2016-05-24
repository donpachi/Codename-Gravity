using UnityEngine;
using System.Collections;

public class GaugeOrientation : MonoBehaviour {

    Animator anim;
    GameObject gauge;
    int[] angles = { 0, 90, 180, 270 };
    int gravityOrient;
    int charOrient;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        gauge = transform.Find("Car/CarBack/Gauge").gameObject;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        flipGauge();
        int gaugeOrient = determineOrientation();
        anim.SetInteger("GaugeOrientation", gaugeOrient);
	}

    int determineOrientation()
    {
        int charOrient = anim.GetInteger("Orientation") - gravityOrient;

        if (charOrient == -1)
            charOrient = 3;
        else if (charOrient == -3)
            charOrient = 1;
        else
            charOrient = Mathf.Abs(charOrient);
        int gaugeAngle = angles[charOrient]; //Mathf.Abs(angles[gravityOrient] - angles[charOrient]);

        gaugeAngle = 360 - gaugeAngle;
        switch (gaugeAngle)
        {
            case 90:
                gaugeAngle = 1;
                break;
            case 180:
                gaugeAngle = 2;
                break;
            case 270:
                gaugeAngle = 3;
                break;
            default:
                gaugeAngle = 0;
                break;
        }
        return gaugeAngle;
    }

    //Flip character while moving left and right
    void flipGauge()
    {
        Vector3 gaugeScale = gauge.transform.localScale;
        gaugeScale.x = transform.localScale.x;

        gauge.transform.localScale = gaugeScale;
    }

    void gravityUpdated(OrientationListener.Orientation orientation, float timer)
    {
        gravityOrient = (int)orientation;
    }

    //Listeners for player
    void OnEnable()
    {
        WorldGravity.GravityChanged += gravityUpdated;
    }

    void OnDisable()
    {
        WorldGravity.GravityChanged -= gravityUpdated;
    }

}
