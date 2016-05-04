using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//TODO: Minions get stuck on wall
public class Minion : MonoBehaviour {

    public float MinionDistance;
    public float MinionFollowSpeed;
    public float DeathSpeed = 10f;
    public bool GravityZone { get; private set; }
    public Animator anim { get; private set; }

    GameObject player;
    List<GameObject> minions = new List<GameObject>();
    GameObject _parent;
    public bool IsFollowing = true;
    float playerPosDiff;
    Vector2 prevPlayerLocation;
    private FollowPlayer _camera;
    private GroundCheck gCheck;
    private bool facingRight = true;

    // Use this for initialization
    void Start() {
        anim = gameObject.GetComponent<Animator>();
        player = GameObject.Find("Player");
        gCheck = GetComponent<GroundCheck>();
        this.GetComponent<PlayerJump>().enabled = false;
        this.GetComponent<Walk>().enabled = false;
        _camera = FindObjectOfType<FollowPlayer>();
        GravityZoneOff();
    }

    void Update()
    {
    }

    // Update is called once per frame
    void FixedUpdate() {
        anim.SetBool("InAir", gCheck.InAir);
        if (!IsFollowing)
            return;

        RaycastHit2D groundCheckRay = Physics2D.Raycast(transform.position, -transform.up, 0.5f);
        if (player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle") && checkIfSameHeight() && groundCheckRay.collider != null && groundCheckRay.collider.name.Contains("MovingPlatform"))
        {
            if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.PORTRAIT || OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.INVERTED_PORTRAIT)
            {
                playerPosDiff = Mathf.Abs(prevPlayerLocation.x - player.transform.position.x);
                if (prevPlayerLocation.x > player.transform.position.x)
                    transform.position = new Vector2(transform.position.x - playerPosDiff, transform.position.y);
                else
                    transform.position = new Vector2(transform.position.x + playerPosDiff, transform.position.y);
            }
            else
            {
                playerPosDiff = Mathf.Abs(prevPlayerLocation.y - player.transform.position.y);
                if (prevPlayerLocation.y > player.transform.position.y)
                    transform.position = new Vector2(transform.position.x, transform.position.y + playerPosDiff);
                else
                    transform.position = new Vector2(transform.position.x, transform.position.y - playerPosDiff);
            }
        }
        else
            lerpToPlayer();

        checkGravityScale();
        prevPlayerLocation = player.transform.position;
    }

    public void GravityZoneOn()
    {
        GravityZone = true;
    }

    public void GravityZoneOff()
    {
        GravityZone = false;
    }

    public void SetParent(GameObject parentObj)
    {
        _parent = parentObj;
    }

    void checkGravityScale()
    {
        if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.PORTRAIT && transform.position.y > player.GetComponent<Player>().getPlayerFeet())
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
        else if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.PORTRAIT && transform.position.y <= player.GetComponent<Player>().getPlayerFeet())
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        else if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.INVERTED_PORTRAIT && transform.position.y > player.GetComponent<Player>().getPlayerFeet())
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        else if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.INVERTED_PORTRAIT && transform.position.y <= player.GetComponent<Player>().getPlayerFeet())
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;

        else if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.LANDSCAPE_LEFT && transform.position.x > player.GetComponent<Player>().getPlayerFeet())
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
        else if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.LANDSCAPE_LEFT && transform.position.x <= player.GetComponent<Player>().getPlayerFeet())
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        else if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.LANDSCAPE_RIGHT && transform.position.x > player.GetComponent<Player>().getPlayerFeet())
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        else if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.LANDSCAPE_RIGHT && transform.position.x <= player.GetComponent<Player>().getPlayerFeet())
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
    }

    bool checkIfSameHeight()
    {
        if ((OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.PORTRAIT || OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.INVERTED_PORTRAIT) && Mathf.Abs((player.transform.position.y - 0.25f) - transform.position.y) > 0.01f)
            return false;
        if ((OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.LANDSCAPE_LEFT || OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.LANDSCAPE_RIGHT) && Mathf.Abs((player.transform.position.y - 0.25f) - transform.position.y) > 0.01f)
            return false;
        return true;
    }

    void lerpToPlayer()
    {
        if (_parent == null)
        {
            if (Vector2.Distance(transform.position, player.GetComponent<Player>().getPlayerFeetVector()) > MinionDistance)
            {
                transform.position = Vector2.Lerp(transform.position, player.GetComponent<Player>().getPlayerFeetVector(), MinionFollowSpeed);
                anim.SetBool("Moving", true);
            }
            else
                anim.SetBool("Moving", false);
        }
        else if (Vector2.Distance(transform.position, _parent.transform.position) > MinionDistance)
        {
            anim.SetBool("Moving", true);
            transform.position = Vector2.Lerp(transform.position, _parent.transform.position, MinionFollowSpeed);
        }
        else
            anim.SetBool("Moving", false);
    }

    void lerpToObject(GameObject target)
    {
        if (Vector2.Distance(transform.position, target.transform.position) > .01f)
        {
            anim.SetBool("Moving", true);
            transform.position = Vector2.Lerp(transform.position, target.transform.position, MinionFollowSpeed);
        }
    }

    void swipeCheck(TouchController.SwipeDirection direction)
    {
        if(direction == TouchController.SwipeDirection.DOWN && !IsFollowing && !gCheck.InAir)
        {
            GetComponent<Rigidbody2D>().isKinematic = true;
            GetComponent<Walk>().enabled = false;
            GetComponent<PlayerJump>().enabled = false;
            LevelManager.Instance.NewCheckpointRequest(gameObject);
        }
    }

    void setCheckpoint()
    {
        LevelManager.Instance.setNewCheckpoint();
        minionDeath();
    }

    public void GainControl()
    {
        this.GetComponent<Rigidbody2D>().gravityScale = 1;
        this.GetComponent<PlayerJump>().enabled = true;
        this.GetComponent<Walk>().enabled = true;
        _camera.setFollowObject(gameObject);
        IsFollowing = false;
        anim.SetBool("SwitchingToMinion", false);
    }

    private void minionDeath()
    {
        Destroy(gameObject);
        returnToPlayer();
    }

    private void destroySpirit()
    {
        Destroy(gameObject);
    }

    private void returnToPlayer()
    {
        _camera.setFollowObject(player);
        player.GetComponent<Player>().switchControlToPlayer();
    }

    //Flip character while moving left and right
    void flipSprite(TouchInstanceData data)
    {
        Vector3 objScale = transform.localScale;
        if (facingRight && data.touchLocation == TouchController.TouchLocation.LEFT)
        {
            facingRight = false;
            objScale.x *= -1;
        }
        else if(!facingRight && data.touchLocation == TouchController.TouchLocation.RIGHT)
        {
            facingRight = true;
            objScale.x *= -1;
        }

        transform.localScale = objScale;
    }

    //Event handling for swipe events
    void OnEnable()
    {
        TouchController.OnSwipe += swipeCheck;
        TouchController.OnHold += flipSprite;
    }
    void OnDisable()
    {
        TouchController.OnSwipe -= swipeCheck;
        TouchController.OnHold -= flipSprite;
    }

    //Check for deadly collisions
    void OnCollisionEnter2D(Collision2D collisionEvent)
    {
        if (!IsFollowing)
        {
            if (collisionEvent.gameObject.tag == "Hazard" || collisionEvent.relativeVelocity.magnitude > DeathSpeed)
            {
                anim.SetBool("Dying", true);
            }
        }
    }


}
