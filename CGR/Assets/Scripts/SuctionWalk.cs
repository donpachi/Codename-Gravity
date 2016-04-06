using UnityEngine;
using System.Collections;
using UnityEngine.UI;       //FOR DEBUG REMOVE LATER

public class SuctionWalk : MonoBehaviour
{

    public float THRUST = 1f;
    public float MAXSPEED = 4f;
    public float ForwardRaySize;
    public int SuctionForce = 25;

    private Rigidbody2D playerBody;
    private Animator anim;
    private bool atTopSpeed;
    Vector2 leftVector = new Vector2(-1, 0);
    Vector2 rightVector = new Vector2(1, 0);
    private GameObject suctionText;
    private float timer;

    private LayerMask wallMask;

    private Vector3 zAxis = new Vector3(0, 0, 1);
    private Vector3 portrait = new Vector3(220, 435, 0);
    private Vector3 landscapeRight = new Vector3(-220, 435, 0);
    private Vector3 portraitUpsideDown = new Vector3(-220, -435, 0);
    private Vector3 landscapeLeft = new Vector3(220, -435, 0);
    private ConstantForce2D _cForce;

    // Use this for initialization
    void Start()
    {
        atTopSpeed = false;
        playerBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        suctionText = GameObject.Find("SuctionText");
        suctionText.GetComponent<Text>().enabled = true;
        wallMask = 1 << LayerMask.NameToLayer("Walls");
        _cForce = GetComponent<ConstantForce2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerBody.velocity.magnitude < MAXSPEED)
            atTopSpeed = false;
        else
            atTopSpeed = true;

        if (playerBody.velocity.magnitude > 0)
            anim.SetBool("Moving", true);
        else
            anim.SetBool("Moving", false);

        if (timer != 0)
        {
            timer -= Time.deltaTime;
            suctionText.GetComponent<Text>().text = timer.ToString();
            if (timer <= 0)
            {
                suctionText.GetComponent<Text>().enabled = false;
                this.GetComponent<ConstantForce2D>().enabled = false;
                this.GetComponent<Player>().SuctionStatusEnd();
                this.GetComponent<SuctionWalk>().enabled = false;
                GetComponent<Player>().gravitySpriteUpdate(GetComponent<WorldGravity>().CurrentGravityDirection, 0);
                if (!this.GetComponent<Player>().IsLaunched() && !this.GetComponent<Player>().IsInTransition())
                {
                    playerBody.gravityScale = 1.0f;
                    this.GetComponent<Walk>().enabled = true;
                    this.GetComponent<SuctionWalk>().enabled = false;
                }
            }
        }
    }

    public void SetTimer(float t)
    {
        timer = t;
        if (suctionText != null)
            suctionText.GetComponent<Text>().enabled = true;
    }

    void screenTouched(TouchInstanceData data)
    {
        TouchController.TouchLocation _touchLocation = data.touchLocation;

        if (!atTopSpeed)
        {
            switch (_touchLocation)
            {
                case TouchController.TouchLocation.LEFT:
                    if(forwardCheck(transform.right * -1, _touchLocation))
                    {
                        rotateObject(1);
                    }
                    else
                        playerBody.AddRelativeForce(leftVector * THRUST, ForceMode2D.Impulse);
                    break;
                case TouchController.TouchLocation.RIGHT:
                    if (forwardCheck(transform.right, _touchLocation))
                    {
                        rotateObject(-1);
                    }
                    else
                        playerBody.AddRelativeForce(rightVector * THRUST, ForceMode2D.Impulse);
                    break;
                case TouchController.TouchLocation.NONE:
                    break;
            }
        }
    }

    /// <summary>
    /// Updates the animator orientation value to reflect rotation, ignores if already in rotation
    /// -1 is clockwise 1 is counter
    /// </summary>
    /// <param name="direction"></param>
    void rotateObject(int direction)
    {
        if (GetComponent<Player>().InRotation)
            return;
        int current = anim.GetInteger("Orientation");
        int newOrientation = current + direction;

        if (newOrientation > 3)
            newOrientation = 0;
        else if (newOrientation < 0)
            newOrientation = 3;

        anim.SetInteger("Orientation", newOrientation);
        GetComponent<Player>().InRotation = true;
    }

    bool forwardCheck(Vector2 forwardRay, TouchController.TouchLocation direction)
    {
        RaycastHit2D forwardCheckRay = Physics2D.Raycast(transform.position, forwardRay, ForwardRaySize, wallMask);
        Debug.DrawRay(transform.position, forwardRay * ForwardRaySize, Color.cyan, 0.5f);
        if (forwardCheckRay.collider != null)
        {
            Debug.Log("Forward ray hit");
            return true;
        }
        return false;
        // use constant force value to derive the new left and right vectors
        // reorientate the player
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
