using UnityEngine;
using System.Collections;

/// <summary>
/// Script makes objects with a slider joint oscillate along a linear plane
/// </summary>
public class OscillatingObject : MonoBehaviour {
    public float Delay;     //Delays object at limit for set amount of time
    public bool IsActive;   //Is the slider joint active?
    public bool IgnoreWalls;

    SliderJoint2D joint;
    JointLimitState2D lastLimit;
    float timer;

	// Use this for initialization
	void Start () {
        joint = gameObject.GetComponent<SliderJoint2D>();
        timer = 0;
        lastLimit = JointLimitState2D.LowerLimit;
        if (!IsActive)
            gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
        setCollisions();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	    if(IsActive && (joint.limitState == JointLimitState2D.LowerLimit || joint.limitState == JointLimitState2D.UpperLimit))
        {
            if (timer < Delay)
            {
                timer += Time.deltaTime;
            }
            else if (joint.limitState != lastLimit)
            {
                lastLimit = joint.limitState;
                flipDirection();
                timer = 0;
            }
        }
	}

    void setCollisions()
    {
        if (IgnoreWalls)
        {
            foreach (var gObject in gameObject.GetComponentsInChildren<Transform>())
            {
                gObject.gameObject.layer = LayerMask.NameToLayer("ThroughWalls");
            }
        }
        else
        {
            foreach (var gObject in gameObject.GetComponentsInChildren<Transform>())
            {
                gObject.gameObject.layer = LayerMask.NameToLayer("Walls");
            }
        }
    }

    void flipDirection()
    {
        float newMotorSpeed = -GetComponent<SliderJoint2D>().motor.motorSpeed;
        JointMotor2D newMotor = new JointMotor2D();
        newMotor.motorSpeed = newMotorSpeed;
        newMotor.maxMotorTorque = GetComponent<SliderJoint2D>().motor.maxMotorTorque;
        GetComponent<SliderJoint2D>().motor = newMotor;
    }

    //For Switches
    void plateDepressed()
    {
        IsActive = true;
        gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
    }

    void plateReleased()
    {
    }
}
