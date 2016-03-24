using UnityEngine;
using System.Collections;
using UnityEngine.UI;       //FOR DEBUG REMOVE LATER

public class Walk : MonoBehaviour {

    public float THRUST = 0.5f;
    public float INAIRTHRUST = 0.1f;
    public float MAXSPEED = 10f;
    public float MAXFLOATSPEED = 2f;

    private Rigidbody2D rBody;
    private Animator anim;
    private float minWalkSpeed = 0.1f;

	// Use this for initialization
	void Start ()
    {
        rBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        //playerState = gameObject.GetComponent<Player>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (GetComponent<GroundCheck>().InAir && rBody.velocity.magnitude < MAXFLOATSPEED)
        {
            applyMoveForce(INAIRTHRUST);
        }
        else if (rBody.velocity.magnitude < MAXSPEED && !GetComponent<GroundCheck>().InAir)
        {
            applyMoveForce(THRUST);
            if (TouchController.Instance.getTouchDirection() != TouchController.TouchLocation.NONE)
                anim.SetBool("Moving", true);
        }

        if (rBody.velocity.magnitude < minWalkSpeed)
            anim.SetBool("Moving", false);
	}

    void applyMoveForce(float force)
    {
        TouchController.TouchLocation movementDirection = TouchController.Instance.getTouchDirection();

        switch (movementDirection)
        {
            case TouchController.TouchLocation.LEFT:
                rBody.AddForce(OrientationListener.instanceOf.getWorldLeftVector() * force, ForceMode2D.Impulse);
                break;
            case TouchController.TouchLocation.RIGHT:
                rBody.AddForce(OrientationListener.instanceOf.getWorldRightVector() * force, ForceMode2D.Impulse);
                break;
            case TouchController.TouchLocation.NONE:
                break;
        }

    }

}
