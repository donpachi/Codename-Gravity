using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public delegate void OnOrientationChange();

public class Controls : MonoBehaviour
{
    public float THRUST = 0.5f;
    public Rigidbody2D playerBody;
    public float GRAVITYVALUE = 25f;
    public float GRAVITYCOOLDOWN = 5f;
    public float MAXSPEED = 10f;
    public bool launched = false;

    private float AccelerometerUpdateInterval = 1.0f / 60.0f;
    private float LowPassKernalWidthInSeconds = 0.1f;
    private Vector3 lowPassValue = Vector3.zero;
    private Vector2 rightForce = new Vector2(1, 0);
    private Vector2 leftForce = new Vector2(-1, 0);
    private Vector2 downForce = new Vector2(0, -1);
    private Vector2 upForce = new Vector2(0, 1);
    private Vector2 savedGravityState;
    private bool topRight, topLeft, bottomRight, bottomLeft = false;
    private bool inAir, gravityOnCooldown, topSpeedReached;
    private float LowPassFilterFactor, elapsedTime;
    /* gravity Orientation states as follows
    * 0 - Portrait
    * 1 - Landscape Left
    * 2 - Landscape Right
    * 3 - Portrait Upside Down
    */
    private char gravityOrientation;
    private static Vector2 gravVector;
    private Animator anim;


    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        elapsedTime = 0;
        playerBody = GetComponent<Rigidbody2D>();
        lowPassValue = Input.acceleration;
        LowPassFilterFactor = AccelerometerUpdateInterval / LowPassKernalWidthInSeconds;
        gravityOnCooldown = false;
        topSpeedReached = false;
        elapsedTime = 0;
        gravityOrientation = '0';
        savedGravityState = downForce;
        Physics2D.gravity = downForce * GRAVITYVALUE;
        gravityOnCooldown = true;
        elapsedTime = 4.0f;
    }

    private void filterAccelerometerValues(Vector2 filteredVector)
    {
        if (Mathf.Abs(filteredVector.x) > Mathf.Abs(filteredVector.y))
        {
            if (filteredVector.x < 0)
            {
                gravVector.x = -1;
                gravVector.y = 0;
            }
            else
            {
                gravVector.x = 1;
                gravVector.y = 0;
            }
        }
        else if (Mathf.Abs(filteredVector.x) <= Mathf.Abs(filteredVector.y))
        {
            if (filteredVector.y < 0)
            {
                gravVector.y = -1;
                gravVector.x = 0;
            }
            else
            {
                gravVector.y = 1;
                gravVector.x = 0;
            }
        }
    }

    private void updateGravity()
    {
        savedGravityState = gravVector;                     //save the current gravity state right before changing to allow for comparison for when elapsed time > gravity cooldown time
        Physics2D.gravity = gravVector * GRAVITYVALUE;      //set new gravity values according to the accelerometer values of the phone
    }

    // called once per physics clock cycle
    void FixedUpdate()
    {

        if (gravVector == downForce)
            gravityOrientation = 'P';           //'P' portrait
        else if (gravVector == rightForce)
            gravityOrientation = 'L';           //'L' landscape left
        else if (gravVector == leftForce)
            gravityOrientation = 'R';           //'R' landscape right
        else if (gravVector == upForce)
            gravityOrientation = 'U';           //'U' portrait upside down

        bool left, right;

        if (playerBody.velocity.magnitude < MAXSPEED)
            topSpeedReached = false;
        else
            topSpeedReached = true;

        if (!topSpeedReached)
        {
            TouchController.TouchLocation movementDirection = TouchController.Instance.getTouchDirection();
            switch (movementDirection)
            {
                case TouchController.TouchLocation.LEFT:
                    addForce(OrientationListener.instanceOf.getRelativeLeftVector(), ForceMode2D.Impulse);
                    break;
                case TouchController.TouchLocation.RIGHT:
                    addForce(OrientationListener.instanceOf.getRelativeRightVector(), ForceMode2D.Impulse);
                    break;
                case TouchController.TouchLocation.NONE:
                    break;
            }
        }
        anim.SetFloat("Speed", playerBody.velocity.magnitude);

        //if (Input.touchCount == 1)
        //{
        //    Touch myTouch = Input.GetTouch(0);
        //    switch (gravityOrientation)
        //    {
        //        case 'P':                       // portrait mode
        //            left = bottomLeft;
        //            right = bottomRight;
        //            if (left && !topSpeedReached)
        //                addForce(leftForce * THRUST, ForceMode2D.Impulse);
        //            else if (right && !topSpeedReached)
        //                addForce(rightForce * THRUST, ForceMode2D.Impulse);
        //            break;
        //        case 'L':                       // Landscape left
        //            left = topLeft;
        //            right = bottomLeft;
        //            if (left && !topSpeedReached)
        //                addForce(upForce * THRUST, ForceMode2D.Impulse);
        //            else if (right && !topSpeedReached)
        //                addForce(downForce * THRUST, ForceMode2D.Impulse);
        //            break;
        //        case 'R':                       // Landscape Right
        //            left = bottomRight;
        //            right = topRight;
        //            if (left && !topSpeedReached)
        //                addForce(downForce * THRUST, ForceMode2D.Impulse);
        //            else if (right && !topSpeedReached)
        //                addForce(upForce * THRUST, ForceMode2D.Impulse);
        //            break;
        //        case 'U':                       // Portrait Ura
        //            left = topRight;
        //            right = topLeft;
        //            if (left && !topSpeedReached)
        //                addForce(rightForce * THRUST, ForceMode2D.Impulse);
        //            else if (right && !topSpeedReached)
        //                addForce(leftForce * THRUST, ForceMode2D.Impulse);
        //            break;
        //    }
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 0)
            resetMovementFlags();

        filterAccelerometerValues(LowPassFilterAccelerometer(LowPassFilterFactor));
        if (!gravityOnCooldown && savedGravityState != gravVector)
        {
            updateGravity();
            gravityOnCooldown = true;
            elapsedTime = 0;
        }
        if (elapsedTime < GRAVITYCOOLDOWN)
            elapsedTime += Time.deltaTime;
        else if (elapsedTime >= GRAVITYCOOLDOWN && gravityOnCooldown)
        {
            gravityOnCooldown = false;
        }
    }

    void OnCollisionStay2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Wall")
        {
            inAir = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (launched)
        {
            playerBody.gravityScale = 1.0f;
            launched = false;
        }
    }

    void OnCollisionLeave2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Wall")
        {
            inAir = true;
        }
    }

    //Flag Handling for buttons
    public void TopRightDown()
    {
        topRight = true;
    }

    public void TopRightUp()
    {
        topRight = false;
    }

    public void TopLeftDown()
    {
        topLeft = true;
    }

    public void TopLeftUp()
    {
        topLeft = false;
    }

    public void BottomRightDown()
    {
        bottomRight = true;
    }

    public void BottomRightUp()
    {
        bottomRight = false;
    }

    public void BottomLeftDown()
    {
        bottomLeft = true;
    }

    public void BottomLeftUp()
    {
        bottomLeft = false;
    }

    Vector2 getGravityDirectionVector()
    {
        return gravVector;
    }

    public void resetMovementFlags()
    {
        topRight = false;
        topLeft = false;
        bottomRight = false;
        bottomLeft = false;
    }

    Vector2 LowPassFilterAccelerometer(float filter)
    {
        float xfilter = Mathf.Lerp(lowPassValue.x, Input.acceleration.x, filter);
        float yfilter = Mathf.Lerp(lowPassValue.y, Input.acceleration.y, filter);
        lowPassValue = new Vector2(xfilter, yfilter);
        return lowPassValue;
    }

    public void addForce(Vector2 vect, ForceMode2D force)
    {
        playerBody.AddForce(vect, force);
    }

    public void launchStatusOn()
    {
        launched = true;
    }
}