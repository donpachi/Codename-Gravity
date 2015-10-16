using UnityEngine;
using System.Collections;
using UnityEngine.UI;       //FOR DEBUG REMOVE LATER

public class SuctionWalk : MonoBehaviour
{

    public float THRUST = 1f;
    public float MAXSPEED = 4f;

    private Rigidbody2D playerBody;
    private Animator anim;
    private bool atTopSpeed;
    private Vector2 leftVector;
    private Vector2 rightVector;

    // Use this for initialization
    void Start()
    {
        atTopSpeed = false;
        playerBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
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
                    playerBody.AddForce(leftVector * THRUST, ForceMode2D.Impulse);
                    break;
                case TouchController.TouchLocation.RIGHT:
                    playerBody.AddForce(rightVector * THRUST, ForceMode2D.Impulse);
                    break;
                case TouchController.TouchLocation.NONE:
                    break;
            }
        }

        if (TouchController.Instance.getTouchDirection() != TouchController.TouchLocation.NONE)
            anim.SetBool("Moving", true);
        else
            anim.SetBool("Moving", false);
    }

    public void GetVectors()
    {
        leftVector = OrientationListener.instanceOf.getRelativeLeftVector();
        rightVector = OrientationListener.instanceOf.getRelativeRightVector();
    }
}
