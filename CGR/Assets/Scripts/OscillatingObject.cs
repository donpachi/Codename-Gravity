using UnityEngine;
using System.Collections;

/// <summary>
/// Script makes objects with a slider joint oscillate along a linear plane
/// </summary>
public class OscillatingObject : MonoBehaviour {
    public float Delay;     //Delays object at limit for set amount of time
    public bool IsActive;   //Is the slider joint active?
    public bool IgnoreWalls;
    public bool IsElevator;

    SliderJoint2D joint;
    JointLimitState2D lastLimit;
    float timer;
    Rigidbody2D rBody;
    bool elevatorTrigger;
    Animator anim;

	// Use this for initialization
	void Start () {
        joint = GetComponent<SliderJoint2D>();
        timer = 0;
        lastLimit = JointLimitState2D.LowerLimit;
        rBody = GetComponent<Rigidbody2D>();

        if (!IsActive || IsElevator)
            rBody.isKinematic = true;

        if (IsElevator)
            elevatorTrigger = false;
        else
            elevatorTrigger = true;

        anim = GetComponent<Animator>();

        setCollisions();
	}
	
	void FixedUpdate () {
        if (anim)
        {
            anim.SetBool("On", IsActive);
        }

	    if(IsActive && (joint.limitState == JointLimitState2D.LowerLimit || joint.limitState == JointLimitState2D.UpperLimit))
        {
            if (timer < Delay)
            {
                timer += Time.deltaTime;
            }
            else if (elevatorTrigger && joint.limitState != lastLimit)
            {
                flipDirection();
                timer = 0;
            }
        }
	}

    void setCollisions()
    {
        if (IgnoreWalls)
        {
            foreach (var gObject in GetComponentsInChildren<Transform>())
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
        if (IsElevator)
        {
            elevatorTrigger = false;
            rBody.isKinematic = true;
        }

        lastLimit = joint.limitState;
        float newMotorSpeed = -joint.motor.motorSpeed;
        JointMotor2D newMotor = new JointMotor2D();
        newMotor.motorSpeed = newMotorSpeed;
        newMotor.maxMotorTorque = joint.motor.maxMotorTorque;
        joint.motor = newMotor;
    }

    //For Switches
    void plateDepressed()
    {
        if (!IsElevator)
        {
            IsActive = !IsActive;
            rBody.isKinematic = !rBody.isKinematic;
        }
        else
        {
            if(joint.limitState == JointLimitState2D.LowerLimit || joint.limitState == JointLimitState2D.UpperLimit)
            {
                elevatorTrigger = true;
                rBody.isKinematic = !rBody.isKinematic;
            }
        }
    }

    void plateReleased()
    {
        if (!IsElevator)
        {
            IsActive = !IsActive;
            rBody.isKinematic = !rBody.isKinematic;
        }
    }
}
