using UnityEngine;
using System.Collections;

/// <summary>
/// The script for a block travesing nodes.
/// The origin and target nodes need to be defined in unity before use
/// </summary>
public class RailBlock : MonoBehaviour {
    public GameObject Origin;
    public GameObject Target;
    public bool isActive;

    private SliderJoint2D joint;
    private Animator childAnim;
    private Animator anim;

	// Find 2 Nodes to start from, mby define in unity
    // Set where the box originates, set the definition of rail it rides on
	void Start () {
        if (Origin == null || Target == null)
        {
            Debug.LogError("Rail Block does not have start or end node defined", gameObject);
        }
        joint = gameObject.GetComponent<SliderJoint2D>();
        transform.position = Origin.transform.position;
        setJointParam();
        anim = gameObject.GetComponent<Animator>();
        childAnim = gameObject.GetComponentsInChildren<Animator>()[1];
        childAnim.SetBool("BoxActive", isActive);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (isActive)
        {
            if (joint.limitState == JointLimitState2D.UpperLimit)
                findNextNode(Target);
            else if (joint.limitState == JointLimitState2D.LowerLimit)
                findNextNode(Origin);
        }
	}

    //Sets the rail definition according to nodes
    void setJointParam()
    {
        Vector3 railVector = Origin.transform.position - Target.transform.position;
        
        //set the angle
        if(Vector3.Cross(Vector3.right, railVector).z < 0)
            joint.angle = -Vector3.Angle(Vector3.right, railVector);
        else
            joint.angle = Vector3.Angle(Vector3.right, railVector);

        Vector3.Cross(Vector3.right, railVector);
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
        GameObject nextNode = node.GetComponent<RailNode>().PathToTake();
        if (nextNode == node && node == Target)
        {
            Target = Origin;
            Origin = node;
            setJointParam();
            return;
        }
        else if (nextNode == node && node == Origin)
            return;
        Origin = node;
        Target = nextNode;
        setJointParam();
    }


    //For Switches
    void plateDepressed()
    {
        isActive = true;
        childAnim.SetBool("BoxActive", isActive);
    }

    void plateReleased()
    {
    }

    /// <summary>
    /// Updates the orientation value for the animator when the orienation changes
    /// </summary>
    /// <param name="orientation"></param>
    /// <param name="timer"></param>
    void gravitySpriteUpdate(OrientationListener.Orientation orientation, float timer)
    {
        switch (orientation)
        {
            case OrientationListener.Orientation.PORTRAIT:
                anim.SetInteger("Orientation", 0);
                break;
            case OrientationListener.Orientation.LANDSCAPE_RIGHT:
                anim.SetInteger("Orientation", 1);
                break;
            case OrientationListener.Orientation.LANDSCAPE_LEFT:
                anim.SetInteger("Orientation", 3);
                break;
            case OrientationListener.Orientation.INVERTED_PORTRAIT:
                anim.SetInteger("Orientation", 2);
                break;
        }
    }

    //Listeners for player
    void OnEnable()
    {
        WorldGravity.GravityChanged += gravitySpriteUpdate;
    }

    void OnDisable()
    {
        WorldGravity.GravityChanged -= gravitySpriteUpdate;
    }


}
