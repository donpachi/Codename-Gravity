using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FollowRails : MonoBehaviour
{
    public RailDefiniton Path;

    private IEnumerator<Transform> currentPoint;


    public void Start()
    {
        if (Path == null)
        {
            Debug.LogError("Rail cannot be null", gameObject);
            return;
        }

        currentPoint = Path.GetPathEnumerator();
        currentPoint.MoveNext();



    }

    void FixedUpdate()
    {
        SliderJoint2D joint = gameObject.GetComponent<SliderJoint2D>();
        JointLimitState2D state = joint.limitState;
        if (state == JointLimitState2D.LowerLimit || state == JointLimitState2D.UpperLimit)
        {

        }
    }

}
