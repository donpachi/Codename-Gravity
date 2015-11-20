using UnityEngine;
using System.Collections;
using UnityEngine.UI;       //FOR DEBUG REMOVE LATER

public class Walk : MonoBehaviour {

    public float THRUST = 0.5f;
    public float INAIRTHRUST = 0.1f;
    public float MAXSPEED = 10f;
    public float MAXFLOATSPEED = 2f;

    private Rigidbody2D playerBody;
    private Animator anim;
    private bool atTopSpeed;
    private Player playerState;

	// Use this for initialization
	void Start () {
        atTopSpeed = false;
        playerBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerState = gameObject.GetComponent<Player>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (playerBody.velocity.magnitude < MAXSPEED)
            atTopSpeed = false;
        else
            atTopSpeed = true;

        if (playerState.inAir && playerBody.velocity.magnitude < MAXFLOATSPEED)
        {
            applyMoveForce(INAIRTHRUST);
        }
        else if (!atTopSpeed && !playerState.inAir)
        {
            applyMoveForce(THRUST);
            if (TouchController.Instance.getTouchDirection() != TouchController.TouchLocation.NONE)
                anim.SetBool("Moving", true);
        }

        if (TouchController.Instance.getTouchDirection() == TouchController.TouchLocation.NONE)
            anim.SetBool("Moving", false);



	}

    void applyMoveForce(float force)
    {
        TouchController.TouchLocation movementDirection = TouchController.Instance.getTouchDirection();

        switch (movementDirection)
        {
            case TouchController.TouchLocation.LEFT:
                playerBody.AddForce(OrientationListener.instanceOf.getWorldLeftVector() * force, ForceMode2D.Impulse);
                break;
            case TouchController.TouchLocation.RIGHT:
                playerBody.AddForce(OrientationListener.instanceOf.getWorldRightVector() * force, ForceMode2D.Impulse);
                break;
            case TouchController.TouchLocation.NONE:
                break;
        }

    }

}
