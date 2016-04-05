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
    public Vector2 leftVector;
    public Vector2 rightVector;
    private GameObject suctionText;
    private float timer;

    private Vector3 zAxis = new Vector3(0, 0, 1);
    private Vector3 portrait = new Vector3(220, 435, 0);
    private Vector3 landscapeRight = new Vector3(-220, 435, 0);
    private Vector3 portraitUpsideDown = new Vector3(-220, -435, 0);
    private Vector3 landscapeLeft = new Vector3(220, -435, 0);
    private TouchController.TouchLocation _touchLocation;

    // Use this for initialization
    void Start()
    {
        atTopSpeed = false;
        playerBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        suctionText = GameObject.Find("SuctionText");
        suctionText.GetComponent<Text>().enabled = true;
        _touchLocation = TouchController.TouchLocation.NONE;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerBody.velocity.magnitude < MAXSPEED)
            atTopSpeed = false;
        else
            atTopSpeed = true;

        if (timer != 0)
        {
            timer -= Time.deltaTime;
            suctionText.GetComponent<Text>().text = timer.ToString();
            if (timer <= 0)
            {
                suctionText.GetComponent<Text>().enabled = false;
                this.GetComponent<ConstantForce2D>().enabled = false;
                this.GetComponent<ConstantForce2D>().force = new Vector2 (0,0);
                this.GetComponent<Player>().SuctionStatusEnd();
                //this.GetComponent<PlayerJump>().enabled = true;
                this.GetComponent<SuctionWalk>().enabled = false;
                if (!this.GetComponent<Player>().IsLaunched() && !this.GetComponent<Player>().IsInTransition())
                {
                    playerBody.gravityScale = 1.0f;
                    this.GetComponent<Walk>().enabled = true;
                    this.GetComponent<SuctionWalk>().enabled = false;
                }
            }
        }

    }

    public void SetVectors(Vector2 downVector)
    {
        leftVector = OrientationListener.instanceOf.getRelativeLeftVector(downVector);
        rightVector = OrientationListener.instanceOf.getRelativeRightVector(downVector);
    }

    public void SetTimer(float t)
    {
        timer = t;
        if (suctionText != null)
            suctionText.GetComponent<Text>().enabled = true;
    }

    void screenTouched(TouchInstanceData data)
    {
        _touchLocation = data.touchLocation;

        if (!atTopSpeed)
        {
            switch (_touchLocation)
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
        if (_touchLocation != TouchController.TouchLocation.NONE)
            anim.SetBool("Moving", true);
        else
            anim.SetBool("Moving", false);
    }

    void OnEnable()
    {
        TouchController.ScreenTouched += screenTouched;
    }

    void OnDisable()
    {
        TouchController.ScreenTouched -= screenTouched;
    }
}
