using UnityEngine;
using System.Collections;
using UnityEngine.UI;       //FOR DEBUG REMOVE LATER

public class SuctionWalk : MonoBehaviour
{

    public float THRUST = 1f;
    public float MAXSPEED = 4f;
    public float ForwardRaySize;
    //public int SuctionForce = 25;

    private Rigidbody2D playerBody;
    private Animator anim;
    private bool atTopSpeed;
    Vector2 leftVector = Vector2.left;
    Vector2 rightVector = Vector2.right;
    private GameObject suctionText;
    private float timer;
    private GroundCheck gCheck;
    private PinchtoZoom cameraZoom;
    private Player player;

    private LayerMask wallMask;
    private ConstantForce2D _cForce;

    // Use this for initialization
    void Start()
    {
        atTopSpeed = false;
        playerBody = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
        anim = GetComponent<Animator>();
        suctionText = GameObject.Find("SuctionText");
        suctionText.GetComponent<Text>().enabled = true;
        wallMask = 1 << LayerMask.NameToLayer("Walls");
        _cForce = GetComponent<ConstantForce2D>();
        cameraZoom = Camera.main.GetComponent<PinchtoZoom>();
        gCheck = GetComponent<GroundCheck>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerBody.velocity.magnitude < MAXSPEED)
            atTopSpeed = false;
        else
            atTopSpeed = true;

        if (playerBody.velocity.magnitude <= 0.1f || TouchController.Instance.GetTouchCount() <= 0)
            anim.SetBool("Moving", false);

        if (timer != 0)
        {
            timer -= Time.deltaTime;
            suctionText.GetComponent<Text>().text = timer.ToString();
            if (timer <= 0)
            {
                suctionText.GetComponent<Text>().enabled = false;
                player.ReactivateControl(Player.StateChange.SWALK);
            }
            else
                checkGround();
        }
    }

    public void SetTimer(float t)
    {
        timer = t;
        if (suctionText != null)
            suctionText.GetComponent<Text>().enabled = true;
    }

    public float GetTimer()
    {
        return timer;
    }

    void checkGround()
    {
        if(gCheck.InAir)
        {
            player.updatePlayerOrientation(WorldGravity.Instance.CurrentGravityDirection, 0);
            playerBody.gravityScale = 1.0f;
            playerBody.GetComponent<ConstantForce2D>().enabled = false;
        }
        else
        {
            playerBody.gravityScale = 0.0f;
            playerBody.GetComponent<ConstantForce2D>().enabled = true;
        }
    }

    void screenTouched(TouchInstanceData data)
    {
        TouchController.TouchLocation _touchLocation = data.touchLocation;

        if (!atTopSpeed && !cameraZoom.Zooming)
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
            anim.SetBool("Moving", true);
        }
    }

    /// <summary>
    /// Updates the animator orientation value to reflect rotation, ignores if already in rotation
    /// -1 is clockwise 1 is counter
    /// </summary>
    /// <param name="direction"></param>
    void rotateObject(int direction)
    {
        if (player.InRotation)
            return;
        int current = anim.GetInteger("Orientation");
        int newOrientation = current + direction;

        if (newOrientation > 3)
            newOrientation = 0;
        else if (newOrientation < 0)
            newOrientation = 3;

        anim.SetInteger("Orientation", newOrientation);
        player.InRotation = true;
    }

    bool forwardCheck(Vector2 forwardRay, TouchController.TouchLocation direction)
    {
        RaycastHit2D forwardCheckRay = Physics2D.Raycast(transform.position, forwardRay, ForwardRaySize, wallMask);
        Debug.DrawRay(transform.position, forwardRay * ForwardRaySize, Color.cyan, 0.5f);
        if (forwardCheckRay.collider != null)
        {
            return true;
        }
        return false;
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
