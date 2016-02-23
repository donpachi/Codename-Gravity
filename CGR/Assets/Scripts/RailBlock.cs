using UnityEngine;
using System.Collections;

public class RailBlock : MonoBehaviour {
    public GameObject Origin;
    public GameObject Target;

    private SliderJoint2D joint;

	// Find 2 Nodes to start from, mby define in unity
    // Set where the box originates, set the definition of rail it rides on
	void Start () {
        //if (((Target.transform.position - Origin.transform.position).normalized == Vector3.down)
        //        || ((Target.transform.position - Origin.transform.position).normalized == Vector3.up))
        //    gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;
        joint = gameObject.GetComponent<SliderJoint2D>();
        transform.position = Origin.transform.position;
        setJointParam();
        

	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (joint.limitState == JointLimitState2D.UpperLimit)
            findNextNode(Target);
        else if (joint.limitState == JointLimitState2D.LowerLimit)
            findNextNode(Origin);
	}

    //Sets the rail definition according to nodes
    void setJointParam()
    {
        Vector3 railVector = Target.transform.position - Origin.transform.position;
        
        //set the angle
        joint.angle = Vector3.Angle(Vector3.right, railVector);
        //set the new anchor
        joint.connectedAnchor = Origin.transform.position;
        //set the new limits
        JointTranslationLimits2D limits = joint.limits;
        limits.max = (Target.transform.position - Origin.transform.position).magnitude;
        joint.limits = limits;
    }

    //finds the next applicable node
    void findNextNode(GameObject node)
    {
        
        //if(gameObject.GetComponent<Rigidbody2D>().velocity.normalized != )
    }
}
