using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//TODO: Minions get stuck on wall
public class Minion : MonoBehaviour {

    public float MinionDistance;
    public float MinionFollowSpeed;
    public float DeathSpeed = 10f;

    GameObject player;
    List<GameObject> minions = new List<GameObject>();
    GameObject _parent;
    float timer = 0.1f;
    public bool isFollowing = true;
    float playerPosDiff;
    Vector2 prevPlayerLocation;
    private Animator anim;
    private FollowPlayer _camera;
    private bool _spirit;

    // Use this for initialization
    void Start() {
        anim = gameObject.GetComponent<Animator>();
        player = GameObject.Find("Player");
        //this.GetComponent<Player>().enabled = false;
        this.GetComponent<PlayerJump>().enabled = false;
        this.GetComponent<Walk>().enabled = false;
        _camera = FindObjectOfType<FollowPlayer>();
        //updateList();
    }

    void Update()
    {
        if (_spirit)
            lerpToObject(_parent);
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (!isFollowing)
            return;

        RaycastHit2D groundCheckRay = Physics2D.Raycast(transform.position, OrientationListener.instanceOf.getWorldDownVector(), 0.5f);
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
        if(direction == TouchController.SwipeDirection.DOWN && !isFollowing)
        {
            GetComponent<Rigidbody2D>().gravityScale = 0;
            GetComponent<Walk>().enabled = false;
            GetComponent<PlayerJump>().enabled = false;
            LevelManager.Instance.NewCheckpointRequest(gameObject);
        }
    }

    public void GainControl()
    {
        this.GetComponent<Rigidbody2D>().gravityScale = 1;
        this.GetComponent<PlayerJump>().enabled = true;
        this.GetComponent<Walk>().enabled = true;
        GetComponent<GroundCheck>().enabled = true;
        _camera.setFollowObject(gameObject);
        isFollowing = false;
        anim.SetBool("SwitchingToMinion", false);
    }

    private void minionDeath()
    {
        Destroy(gameObject);
        returnToPlayer();
    }

    private void becomeSpirit()
    {
        _spirit = true;
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

    //Event handling for swipe events
    void OnEnable()
    {
        TouchController.OnSwipe += swipeCheck;
    }
    void OnDisable()
    {
        TouchController.OnSwipe -= swipeCheck;
    }

    //Check for deadly collisions
    void OnCollisionEnter2D(Collision2D collisionEvent)
    {
        if (!isFollowing)
        {
            if (collisionEvent.gameObject.tag == "Hazard" || collisionEvent.relativeVelocity.magnitude > DeathSpeed)
            {
                anim.SetBool("Dying", true);
            }
        }
    }


}
