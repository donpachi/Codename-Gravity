using UnityEngine;
using System.Collections;

public class PlayerJump : MonoBehaviour {

    private Vector2 startPos;
    private Vector2 direction;
    private bool fingerLifted;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
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
                    break;
                case TouchPhase.Ended:
                    fingerLifted = true;
                    break;
            }

            if (fingerLifted)
            {
                DeviceOrientation currentOrientation;
                currentOrientation = Input.deviceOrientation;

                if (currentOrientation == DeviceOrientation.Portrait)
                {
                    // +y is up -y is down

                }
                else if (currentOrientation == DeviceOrientation.LandscapeRight)
                {
                    // -x is up +x is down
                }
                else if (currentOrientation == DeviceOrientation.PortraitUpsideDown)
                {
                    // -y is up +y is down
                }
                else
                {
                    // +x is up -x is down
                }
                fingerLifted = false;
            }
        }
	}

    void checkValidJump()
    {

    }

    void jump()
    {


    }
}
