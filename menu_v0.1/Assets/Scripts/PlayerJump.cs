using UnityEngine;
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

    public float DeadZone = 0;
    public float jumpForce = 10;

	// Use this for initialization
	void Start () {
        playerBody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        //if (!inAir)
        //{
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
                        fingerLifted = true;
                        break;
                }

                if (fingerLifted && fingerMoved)
                {
                    jump();
                    fingerLifted = false;
                    fingerMoved = false;
                }
            }
        //}
	}

    //Ignores invalid directions
    void jump()
    {
        DeviceOrientation currentOrientation;
        currentOrientation = Input.deviceOrientation;

        if (currentOrientation == DeviceOrientation.Portrait)
        {
            // +y is up -y is down
            if (direction.y > DeadZone)
            {
                playerBody.AddForce(upVector * jumpForce);
                Debug.Log("StartPosition: " + startPos + "/nDirection: " + direction);
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
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Wall")
        {
            inAir = false;
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
