using UnityEngine;
using System.Collections;

/// <summary>
/// Script that defines the Box movement and player box interaction.
/// </summary>
public class RailBoxControl : MonoBehaviour {

    //defined in unity
    public float THRUST;
    public float MAXSPEED;
    public float PlayerOffset = 0.17f;
    public LayerMask WallMask;
    bool PlayerControlled;

    Rigidbody2D objectRb;
    private Animator anim;
    Player player;
    GameObject mainCamera;
    GameObject childObj;

    public Vector2 tempOffset;
    // Use this for initialization
    void Start () {
        objectRb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        player = FindObjectOfType<Player>();
        mainCamera = GameObject.Find("Main Camera");
        childObj = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (PlayerControlled)
        {
            player.transform.position = childObj.transform.position + childObj.transform.up * PlayerOffset;
        }
    }

    /// <summary>
    /// Listener Function called only when screen touch happens
    /// </summary>
    /// <param name="data"></param>
    void applyMoveForce(TouchInstanceData data)
    {
        if (!PlayerControlled || objectRb.velocity.magnitude > MAXSPEED)
            return;

        TouchController.TouchLocation movementDirection = data.touchLocation;
        switch (movementDirection)
        {
            case TouchController.TouchLocation.LEFT:
                objectRb.AddForce(OrientationListener.instanceOf.getWorldLeftVector() * THRUST, ForceMode2D.Impulse);
                anim.SetInteger("Moving", 1);
                break;
            case TouchController.TouchLocation.RIGHT:
                objectRb.AddForce(OrientationListener.instanceOf.getWorldRightVector() * THRUST, ForceMode2D.Impulse);
                anim.SetInteger("Moving", 2);
                break;
            case TouchController.TouchLocation.NONE:
                anim.SetInteger("Moving", 0);
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D colliderEvent)
    {
        if (anim.GetBool("BoxActive") == true && colliderEvent.gameObject.name == "Player")
        {
            activateControl();
            player.DeactivateControl(StateChange.BOX_IN);
        }
    }

    void activateControl()
    {
        PlayerControlled = true;
        mainCamera.GetComponent<FollowPlayer>().setFollowObject(gameObject);
    }

    void controlReleased(TouchInstanceData data)
    {
        if(PlayerControlled)
            anim.SetInteger("Moving", 0);
    }

    void deactivateControl()
    {
        player.transform.position = transform.position + childObj.transform.up * 1.2f;
        player.updatePlayerOrientation(WorldGravity.Instance.CurrentGravityDirection, 0.0f);
        player.GetComponent<Rigidbody2D>().AddForce(OrientationListener.instanceOf.getRelativeUpVector() * 200);
        player.ReactivateControl(StateChange.BOX_OUT);
        mainCamera.GetComponent<FollowPlayer>().setFollowObject(player.gameObject);
        PlayerControlled = false;
        anim.SetBool("HasExited", false);
    }

    //Event handling
    void swipeCheck(TouchController.SwipeDirection direction)
    {
        if (PlayerControlled && direction == TouchController.SwipeDirection.UP)
        {
            if (checkClearance())
            {
                anim.SetInteger("Moving", 0);
                anim.SetBool("HasExited", true);
            }
        }
    }

    bool checkClearance()
    {
        float exitRaySize = 1;
        RaycastHit2D[] exitRays = Physics2D.RaycastAll(transform.position, transform.GetChild(0).transform.up, exitRaySize, WallMask);
        Debug.DrawRay(transform.position, transform.GetChild(0).transform.up * exitRaySize, Color.red, 0.5f);
        foreach (var ray in exitRays)
        {
            if (ray.collider.gameObject != childObj)
                return false;
        }

        return true;
    }

    void checkpointReset()
    {
        if (PlayerControlled)
        {
            PlayerControlled = false;
            deactivateControl();
        }
    }

    void OnEnable()
    {
        TouchController.OnSwipe += swipeCheck;
        TouchController.OnHold += applyMoveForce;
        TouchController.ScreenReleased += controlReleased;
        LevelManager.OnCheckpointLoad += checkpointReset;
    }
    void OnDisable()
    {
        TouchController.OnSwipe -= swipeCheck;
        TouchController.OnHold -= applyMoveForce;
        TouchController.ScreenReleased -= controlReleased;
        LevelManager.OnCheckpointLoad += checkpointReset;
    }

}
