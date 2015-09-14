using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Controls : MonoBehaviour
{
    public float THRUST = 0.5f;
    public Rigidbody2D rb;
    public float gravityValue = 25f;
    public float perspectiveSpeed = 0.5f;
    public float pinchSpeed = 0.5f;
    public bool launched = false;

    private float AccelerometerUpdateInterval = 1.0f / 60.0f;
    private float LowPassKernalWidthInSeconds = 0.1f;		//greater the value, the slower the acceleration will converge to the current input sampled *taken from unity docs*
    private Vector3 lowPassValue = Vector3.zero;
    private GameObject Controller;
    private GameObject PauseScreen;
    private GameObject DeathScreen;
    private Vector2 rightForce = new Vector2(1, 0);
    private Vector2 leftForce = new Vector2(-1, 0);
    private Vector2 downForce = new Vector2(0, -1);
    private Vector2 upForce = new Vector2(0, 1);
    private Vector2 jumpVect;
    private bool topRight = false;
    private bool topLeft = false;
    private bool bottomRight = false;
    private bool bottomLeft = false;
    private float MAXSPEED = 10f;
    private int jumpCount;
    private bool inAir;
    private bool doubleJumpEnable;
    private float LowPassFilterFactor;
    private float jumpForce = 5;


    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lowPassValue = Input.acceleration;
        Controller = GameObject.Find("ControlCanvas");
        PauseScreen = GameObject.Find("PauseCanvas");
        DeathScreen = GameObject.Find("DeathCanvas");
        PauseScreen.GetComponent<Canvas>().enabled = false;
        DeathScreen.GetComponent<Canvas>().enabled = false;
        jumpCount = 0;
        lowPassValue = Input.acceleration;
        LowPassFilterFactor = AccelerometerUpdateInterval / LowPassKernalWidthInSeconds; //modifiable;
        doubleJumpEnable = true;
    }

    void FixedUpdate()
    {
        bool left;
        bool right;

        if (Input.deviceOrientation == DeviceOrientation.Portrait)
        {
            left = bottomLeft;
            right = bottomRight;
            jumpVect = upForce;
            if (Input.touchCount == 1)
            {
                Touch movementTouch = Input.GetTouch(0);
                if (movementTouch.phase == TouchPhase.Stationary)
                {
                    // If right side of screen is touched
                    if (right && rb.velocity.magnitude < MAXSPEED)
                    {
                        addForce(rightForce, ForceMode2D.Impulse);
                    }
                    // If left side of screen is touched
                    else if (left && rb.velocity.magnitude < MAXSPEED)
                    {
                        addForce(leftForce, ForceMode2D.Impulse);
                    }
                }
                if (movementTouch.phase == TouchPhase.Moved && doubleJumpEnable)
                {
                    resetMovementFlags();
                    addForce(jumpVect * jumpForce, ForceMode2D.Impulse);
                    jumpCount++;
                    if (jumpCount < 2)
                    {
                        doubleJumpEnable = true;
                    }
                    else if (jumpCount == 2)
                    {
                        doubleJumpEnable = false;
                    }

                }
            }
        }


        if (Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown)
        {
            left = topLeft;
            right = topRight;
            jumpVect = downForce;
            if (Input.touchCount == 1)
            {
                Touch movementTouch = Input.GetTouch(0);
                // If right side of screen is touched
                if (right && rb.velocity.magnitude < MAXSPEED)
                {
                    addForce(rightForce, ForceMode2D.Impulse);
                }
                // If left side of screen is touched
                else if (left && rb.velocity.magnitude < MAXSPEED)
                {
                    addForce(leftForce, ForceMode2D.Impulse);
                }
                if (movementTouch.phase == TouchPhase.Moved && doubleJumpEnable)
                {
                    resetMovementFlags();
                    addForce(jumpVect * jumpForce, ForceMode2D.Force);
                    jumpCount++;
                    if (jumpCount < 2)
                    {
                        doubleJumpEnable = true;
                    }
                    else if (jumpCount == 2)
                    {
                        doubleJumpEnable = false;
                    }

                }
            }
        }


        if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft)
        {
            left = topLeft;
            right = bottomLeft;
            jumpVect = rightForce;
            if (Input.touchCount == 1)
            {
                Touch movementTouch = Input.GetTouch(0);
                // If right side of screen is touched
                if (right && rb.velocity.magnitude < MAXSPEED)
                {
                    addForce(downForce, ForceMode2D.Impulse);
                }
                // If left side of screen is touched
                else if (left && rb.velocity.magnitude < MAXSPEED)
                {
                    addForce(upForce, ForceMode2D.Impulse);
                }
                if (movementTouch.phase == TouchPhase.Moved && doubleJumpEnable)
                {
                    resetMovementFlags();
                    addForce(jumpVect * jumpForce, ForceMode2D.Force);
                    jumpCount++;
                    if (jumpCount < 2)
                    {
                        doubleJumpEnable = true;
                    }
                    else if (jumpCount == 2)
                    {
                        doubleJumpEnable = false;
                    }

                }
            }
        }


        if (Input.deviceOrientation == DeviceOrientation.LandscapeRight)
        {
            left = bottomRight;
            right = topRight;
            jumpVect = leftForce;
            if (Input.touchCount == 1)
            {
                Touch movementTouch = Input.GetTouch(0);
                // If right side of screen is touched
                if (right && rb.velocity.magnitude < MAXSPEED)
                {
                    addForce(upForce, ForceMode2D.Impulse);
                }
                // If left side of screen is touched
                else if (left && rb.velocity.magnitude < MAXSPEED)
                {
                    addForce(downForce, ForceMode2D.Impulse);
                }
                if (movementTouch.phase == TouchPhase.Moved && doubleJumpEnable)
                {
                    resetMovementFlags();
                    addForce(jumpVect * jumpForce, ForceMode2D.Force);
                    jumpCount++;
                    if (jumpCount < 2)
                    {
                        doubleJumpEnable = true;
                    }
                    else if (jumpCount == 2)
                    {
                        doubleJumpEnable = false;
                    }

                }
            }
        }

        Vector2 gravVector = LowPassFilterAccelerometer(LowPassFilterFactor);
        if (Mathf.Abs(gravVector.x) > Mathf.Abs(gravVector.y))
        {
            if (gravVector.x < 0)
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
        else if (Mathf.Abs(gravVector.x) <= Mathf.Abs(gravVector.y))
        {
            if (gravVector.y < 0)
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
        Physics2D.gravity = gravVector * gravityValue;
        if (Input.touchCount == 0)
        {
            resetMovementFlags();
        }
    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.Escape) && !DeathScreen.GetComponent<Canvas>().enabled)
        {
            Time.timeScale = 0;
            PauseScreen.GetComponent<Canvas>().enabled = true;
            Controller.GetComponent<Canvas>().enabled = false;
        }

        /*Vector3 forward = new Vector3(0,0,1); //always face along the z-plane // this can be used to rotate the player entity model KEEP THIS.
        Vector3 up = LowPassFilterAccelerometer(LowPassFilterFactor) * -1.0f; //get the upwards facing vector opposite of gravity
        Quaternion rotation = Quaternion.LookRotation (forward, up);
        transform.rotation = rotation;*/

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

    public void resetMovementFlags()
    {
        topRight = false;
        topLeft = false;
        bottomRight = false;
        bottomLeft = false;
    }

    public void LaunchStatusOn()
    {
        launched = true;
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (launched == true)
        {
            rb.gravityScale = 1.0f;
            launched = false;
        }
        if (collisionInfo.gameObject.tag == "Wall")
        {
            jumpCount = 0;
            inAir = false;
        }
    }
    void OnCollisionStay2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Wall")
        {
            jumpCount = 0;
            doubleJumpEnable = true;
        }
    }

    void OnCollisionLeave2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Wall")
        {
            inAir = true;
            doubleJumpEnable = true;
            jumpCount = 0;
        }
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
        rb.AddForce(vect, force);
    }

}