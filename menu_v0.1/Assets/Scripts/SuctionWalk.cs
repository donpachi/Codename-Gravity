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
    private GameObject suctionText;
    private float timer;

    private Vector3 zAxis = new Vector3(0, 0, 1);
    private Vector3 portrait = new Vector3(220, 435, 0);
    private Vector3 landscapeRight = new Vector3(-220, 435, 0);
    private Vector3 portraitUpsideDown = new Vector3(-220, -435, 0);
    private Vector3 landscapeLeft = new Vector3(220, -435, 0);

    // Use this for initialization
    void Start()
    {
        atTopSpeed = false;
        playerBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        suctionText = GameObject.Find("SuctionText");
        suctionText.GetComponent<Text>().enabled = true;
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

        if (Input.deviceOrientation == DeviceOrientation.Portrait)
        {
            suctionText.transform.localPosition = portrait;
            suctionText.transform.rotation = Quaternion.AngleAxis(0, zAxis);
        }
        if (Input.deviceOrientation == DeviceOrientation.LandscapeRight)
        {
            suctionText.transform.localPosition = landscapeRight;
            suctionText.transform.rotation = Quaternion.AngleAxis(90, zAxis);
        }
        if (Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown)
        {
            suctionText.transform.localPosition = portraitUpsideDown;
            suctionText.transform.rotation = Quaternion.AngleAxis(180, zAxis);
        }
        if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft)
        {
            suctionText.transform.localPosition = landscapeLeft;
            suctionText.transform.rotation = Quaternion.AngleAxis(270, zAxis);
        }

        if (timer != 0)
        {
            timer -= Time.deltaTime;
            suctionText.GetComponent<Text>().text = timer.ToString();
            if (timer <= 0)
            {
                suctionText.GetComponent<Text>().enabled = false;
                playerBody.gravityScale = 1.0f;
                this.GetComponent<ConstantForce2D>().enabled = false;
                this.GetComponent<Player>().SuctionStatusEnd();
                this.GetComponent<Walk>().enabled = true;
                this.GetComponent<PlayerJump>().enabled = true;
                this.GetComponent<SuctionWalk>().enabled = false;
            }
        }

    }

    public void GetVectors(Vector2 downVector)
    {
        float sin = Mathf.Sin(-90 * Mathf.Deg2Rad);
        float cos = Mathf.Cos(-90 * Mathf.Deg2Rad);
        leftVector.x = (cos * downVector.x) - (sin * downVector.y);
        leftVector.y = (sin * downVector.x) + (cos * downVector.y);

        sin = Mathf.Sin(90 * Mathf.Deg2Rad);
        cos = Mathf.Cos(90 * Mathf.Deg2Rad);
        rightVector.x = (cos * downVector.x) - (sin * downVector.y);
        rightVector.y = (sin * downVector.x) + (cos * downVector.y);
    }

    public void SetTimer(float t)
    {
        timer = t;
        if (suctionText != null)
            suctionText.GetComponent<Text>().enabled = true;
    }
}
