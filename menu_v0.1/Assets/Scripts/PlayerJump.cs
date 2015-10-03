using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerJump : MonoBehaviour {

    private Vector2 startPos;
    private Vector2 direction;
    private bool fingerMoved = false;
    private bool fingerLifted = false;
    private Vector2 rightVector = new Vector2(1, 0);
    private Vector2 leftVector = new Vector2(-1, 0);
    private Vector2 downVector = new Vector2(0, -1);
    private Vector2 upVector = new Vector2(0, 1);
    private Rigidbody2D playerBody;
    private bool inAir = false;
    private int jumpCount;
    private int jumpLimit;
    private DeviceOrientation currentOrientation;
    private GameObject doubleJumpMeter;
    private float doubleJumpState;
    private float doubleJumpDecValue;
    private GameObject doubleJumpBar;

    private bool canJump;

    int singlejumpCount = 0;

    public float DeadZone = 0;
    public float jumpForce = 10;
    public float doubleJumpLimit = 10;

	// Use this for initialization
	void Start () {
        playerBody = GetComponent<Rigidbody2D>();
        //doubleJumpMeter = GameObject.Find("JumpBar");
        //doubleJumpMeter.SetActive(false);
        doubleJumpDecValue = 1.0f / doubleJumpLimit;
        jumpLimit = 1;
        jumpCount = 0;
        canJump = false;
	}
	
	// Update is called once per frame
    void FixedUpdate()
    {
        if (canJump)
        {
            playerBody.AddForce(OrientationListener.instanceOf.getRelativeUpVector() * jumpForce);
            ++singlejumpCount;
            canJump = false;
        }
        if (gameObject.GetComponent<Player>().inAir)
        {
            //GameObject.Find("JumpCount").GetComponent<Text>().text = "Jumps Reg: " + singlejumpCount;
        }
    }

    //required conditions for a jump
    //      finger on screen
    //      finger moved a certain amount
    //      finger lifted - maybe not as essential
    //      move must be fast enough
    //      
    void Update()
    {
        if (jumpCount < jumpLimit)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        startPos = touch.position;
                        break;

                    case TouchPhase.Moved:
                        direction = touch.position - startPos;
                        fingerMoved = true;
                        break;

                    case TouchPhase.Ended:
                        direction = touch.position - startPos;
                        fingerLifted = true;
                        break;
                }
                jumpCheck();
            }
        }
    }

    void jumpCheck()
    {
        if (fingerLifted && fingerMoved)
        {
            if (jumpLimit != 1)
                doubleJumpCheck();
            //jump();
            fingerLifted = false;
            fingerMoved = false;
        }
    }

    //Ignores invalid directions
    void jump()
    {
        if (Input.deviceOrientation == DeviceOrientation.Portrait || Input.deviceOrientation == DeviceOrientation.LandscapeLeft
            || Input.deviceOrientation == DeviceOrientation.LandscapeRight || Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown)
        {currentOrientation = Input.deviceOrientation;}

        if (currentOrientation == DeviceOrientation.Portrait)
        {
            // +y is up -y is down
            if (direction.y > DeadZone)
            {
                playerBody.AddForce(upVector * jumpForce);
                ++jumpCount;
            }
        }
        else if (currentOrientation == DeviceOrientation.LandscapeRight)
        {
            // -x is up +x is down
            if (direction.x < -DeadZone)
            {
                playerBody.AddForce(leftVector * jumpForce);
                ++jumpCount;
            }
        }
        else if (currentOrientation == DeviceOrientation.PortraitUpsideDown)
        {
            // -y is up +y is down
            if (direction.y < -DeadZone)
            {
                playerBody.AddForce(downVector * jumpForce);
                ++jumpCount;
            }
        }
        else if (currentOrientation == DeviceOrientation.LandscapeLeft)
        {
            // +x is up -x is down
            if (direction.x > DeadZone)
            {
                playerBody.AddForce(rightVector * jumpForce);
                ++jumpCount;
            }
        }
    }

    void jumpCheck(TouchController.SwipeDirection direction)
    {
        if(direction == TouchController.SwipeDirection.UP && !gameObject.GetComponent<Player>().inAir)
        {
            canJump = true;
        }
    }

    void doubleJumpInit()
    {
        doubleJumpState = 1;
        doubleJumpMeter.SetActive(true);
        doubleJumpBar = doubleJumpMeter.transform.FindChild("Bar").gameObject;
        doubleJumpBar.GetComponent<Image>().fillAmount = doubleJumpState;
        jumpLimit++;
    }

    void doubleJumpCheck()
    {
        if (doubleJumpState < 0)
        {
            doubleJumpDisable();
            jumpLimit--;
        }
        else if (jumpCount > 0)
        {
            decDoubleJumpBarUpdate();
        }
    }

    void doubleJumpDisable()
    {
        doubleJumpMeter.SetActive(false);
    }

    void decDoubleJumpBarUpdate()
    {
        doubleJumpState = doubleJumpState - (doubleJumpDecValue);
        doubleJumpBar.GetComponent<Image>().fillAmount = doubleJumpState;
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Wall")
        {
            inAir = false;
            jumpCount = 0;
        }
        else if(collisionInfo.gameObject.tag == "DoubleJump")
        {
            doubleJumpInit();   
        }
    }


    void OnCollisionExit2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Wall")
        {
            inAir = true;
        }
    }

    //Event handling for swipe events
    void OnEnable()
    {
        TouchController.OnSwipe += jumpCheck;
    }
    void OnDisable()
    {
        TouchController.OnSwipe -= jumpCheck;
    }
}
