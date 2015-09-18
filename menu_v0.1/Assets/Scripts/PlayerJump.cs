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

    public float DeadZone = 0;
    public float jumpForce = 10;
    public float doubleJumpLimit = 10;

	// Use this for initialization
	void Start () {
        playerBody = GetComponent<Rigidbody2D>();
        doubleJumpMeter = GameObject.Find("JumpBar");
        doubleJumpMeter.SetActive(false);
        doubleJumpDecValue = 1.0f / doubleJumpLimit;
        jumpLimit = 1;
        jumpCount = 0;
	}
	
	// Update is called once per frame
    //void FixedUpdate()
    //{
    //    if (jumpCount < 1)
    //    {
    //        if (Input.touchCount > 0)
    //        {
    //            Touch touch = Input.GetTouch(0);

    //            switch (touch.phase)
    //            {
    //                case TouchPhase.Began:
    //                    startPos = touch.position;
    //                    break;

    //                case TouchPhase.Moved:
    //                    direction = touch.position - startPos;
    //                    fingerMoved = true;
    //                    break;

    //                case TouchPhase.Ended:
    //                    fingerLifted = true;
    //                    break;
    //            }

    //            if (fingerLifted && fingerMoved)
    //            {
    //                jump();
    //                fingerLifted = false;
    //                fingerMoved = false;
    //            }
    //        }
    //    }
    //}

    void Update()
    {
        if (jumpCount < jumpLimit)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                Debug.Log("Phase: " + touch.phase + "Jump Count" + jumpCount);
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        startPos = touch.position;
                        Debug.Log("Start Pos: " + startPos);
                        break;

                    case TouchPhase.Moved:
                        //direction = touch.position - startPos;
                        fingerMoved = true;
                        break;

                    case TouchPhase.Ended:
                        direction = touch.position - startPos;
                        fingerLifted = true;
                        Debug.Log("End Pos: " + touch.position);
                        Debug.Log("Direction: " + touch.position);
                        break;
                }
                Debug.Log("Lifted: " + fingerLifted + "Moved: " + fingerMoved);
                if (fingerLifted && fingerMoved)
                {
                    if (jumpLimit != 1)
                        doubleJumpCheck();
                    jump();
                    fingerLifted = false;
                    fingerMoved = false;
                }
            }
        }
    }

    //Ignores invalid directions
    void jump()
    {
        Debug.Log("Current Orientation: " + currentOrientation);
        if (Input.deviceOrientation == DeviceOrientation.Portrait || Input.deviceOrientation == DeviceOrientation.LandscapeLeft
            || Input.deviceOrientation == DeviceOrientation.LandscapeRight || Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown)
        {currentOrientation = Input.deviceOrientation;}

        if (currentOrientation == DeviceOrientation.Portrait)
        {
            // +y is up -y is down
            if (direction.y > DeadZone)
            {
                playerBody.AddForce(upVector * jumpForce);
                //Debug.Log("StartPosition: " + startPos + "/nDirection: " + direction);
            }
        }
        else if (currentOrientation == DeviceOrientation.LandscapeRight)
        {
            // -x is up +x is down
            if (direction.x < -DeadZone)
            {
                playerBody.AddForce(leftVector * jumpForce);
            }
        }
        else if (currentOrientation == DeviceOrientation.PortraitUpsideDown)
        {
            // -y is up +y is down
            if (direction.y < -DeadZone)
            {
                playerBody.AddForce(downVector * jumpForce);
            }
        }
        else if (currentOrientation == DeviceOrientation.LandscapeLeft)
        {
            // +x is up -x is down
            if (direction.x > DeadZone)
            {
                playerBody.AddForce(rightVector * jumpForce);
            }
        }
        ++jumpCount;
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
}
