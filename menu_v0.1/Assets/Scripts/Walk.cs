using UnityEngine;
using System.Collections;

public class Walk : MonoBehaviour {

    public float THRUST = 0.5f;
    public float MAXSPEED = 10f;

    private Rigidbody2D playerBody;
    private Animator anim;
    private bool atTopSpeed;

	// Use this for initialization
	void Start () {
        atTopSpeed = false;
        playerBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (playerBody.velocity.magnitude < MAXSPEED)
            atTopSpeed = false;
        else
            atTopSpeed = true;

        if (!atTopSpeed)
        {
            TouchController.TouchLocation movementDirection = TouchController.Instance.getTouchDirection();
            switch (movementDirection)
            {
                case TouchController.TouchLocation.LEFT:
                    playerBody.AddForce(OrientationListener.instanceOf.getRelativeLeftVector() * THRUST, ForceMode2D.Impulse);
                    break;
                case TouchController.TouchLocation.RIGHT:
                    playerBody.AddForce(OrientationListener.instanceOf.getRelativeRightVector() * THRUST, ForceMode2D.Impulse);
                    break;
                case TouchController.TouchLocation.NONE:
                    break;
            }
        }
        anim.SetFloat("Speed", playerBody.velocity.magnitude);
	}
}
