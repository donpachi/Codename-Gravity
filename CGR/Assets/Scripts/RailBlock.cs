using UnityEngine;
using System.Collections;

/// <summary>
/// The script for a block travesing nodes.
/// The origin and target nodes need to be defined in unity before use
/// </summary>
public class RailBlock : MonoBehaviour {
    public GameObject Origin;
    public GameObject Target;
    public bool IsActive;
    public bool GravityZone { get; private set; }

    private SliderJoint2D joint;
    private Animator anim;
    private RailBlockState objSaveState;
    Rigidbody2D objectRb;
    int behindPlayer = 0;        //right behind player
    int frontPlayer = 7;         //right infront of player
    SortingOrderScript spriteOrder;

    // Find 2 Nodes to start from, mby define in unity
    // Set where the box originates, set the definition of rail it rides on
    void Start ()
    {
        if (Origin == null || Target == null)
        {
            Debug.LogError("Rail Block does not have start or end node defined", gameObject);
        }
        objectRb = gameObject.GetComponent<Rigidbody2D>();
        joint = gameObject.GetComponent<SliderJoint2D>();
        spriteOrder = GetComponentInChildren<SortingOrderScript>();
        transform.position = Origin.transform.position;
        setJointParam();
        anim = gameObject.GetComponent<Animator>();
        anim.SetBool("BoxActive", IsActive);
        SetCheckpointState();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (IsActive)
        {
            if (joint.limitState == JointLimitState2D.UpperLimit)
                findNextNode(Target);
            else if (joint.limitState == JointLimitState2D.LowerLimit)
                findNextNode(Origin);
        }
	}

    public void GravityZoneOn()
    {
        GravityZone = true;
    }

    public void GravityZoneOff()
    {
        GravityZone = false;
    }

    void activateBox()
    {
        objectRb.isKinematic = false;
        spriteOrder.SetOrderTo(frontPlayer);
    }

    void deactivateBox()
    {
        objectRb.isKinematic = true;
        spriteOrder.SetOrderTo(behindPlayer);
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
        IsActive = true;
        anim.SetBool("BoxActive", IsActive);
    }

    void plateReleased()
    {
        //IsActive = false;
        //anim.SetBool("BoxActive", IsActive);
    }

    /// <summary>
    /// Updates the orientation value for the animator when the orienation changes
    /// </summary>
    /// <param name="orientation"></param>
    /// <param name="timer"></param>
    void gravitySpriteUpdate(OrientationListener.Orientation orientation, float timer)
    {
        if(!GravityZone)
            anim.SetInteger("Orientation", (int)orientation);
    }

    void SetCheckpointState()
    {
        objSaveState = new RailBlockState();
        objSaveState.Position = transform.position;
        objSaveState.Orientation = anim.GetInteger("Orientation");
        objSaveState.Origin = Origin;
        objSaveState.Target = Target;
        objSaveState.IsActive = IsActive;
    }

    void CheckpointRestart()
    {
        transform.position = objSaveState.Position;
        anim.SetInteger("Orientation", objSaveState.Orientation);
        Origin = objSaveState.Origin;
        Target = objSaveState.Target;
        setJointParam();
        IsActive = objSaveState.IsActive;
        anim.SetBool("BoxActive", IsActive);
        if (IsActive)
            activateBox();
        else
            deactivateBox();
    }

    //Listeners for player
    void OnEnable()
    {
        WorldGravity.GravityChanged += gravitySpriteUpdate;
        LevelManager.OnCheckpointLoad += CheckpointRestart;
        LevelManager.OnCheckpointSave += SetCheckpointState;
    }

    void OnDisable()
    {
        WorldGravity.GravityChanged -= gravitySpriteUpdate;
        LevelManager.OnCheckpointLoad -= CheckpointRestart;
        LevelManager.OnCheckpointSave -= SetCheckpointState;
    }
}

public class RailBlockState
{
    public Vector3 Position;
    public int Orientation;
    public GameObject Origin;
    public GameObject Target;
    public bool IsActive;
}