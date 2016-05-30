using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//TODO: Minions get stuck on wall
public class Minion : MonoBehaviour, ICharacter{

    public float MinionDistance;
    public float MinionFollowSpeed;
    public float DeathSpeed = 10f;
    public bool IsFollowing = true;
    public bool GravityZone { get; private set; }
    public Animator anim { get; private set; }
    public MinionState CurrentState { get; set; }

    Player player; 
    Rigidbody2D rBody;
    Walk walkControl;
    PlayerJump jumpControl;
    Orientation orientControl;
    GameObject _parent;
    Vector2 prevPlayerLocation;
    CircleCollider2D bodyCollider;
    private FollowPlayer _camera;
    private GroundCheck gCheck;
    private bool facingRight = true;
    private float teleportDistance = 4f;
    private Renderer[] renderParts;
    private float _followDistance;
    private float _followSpeed;

    public enum MinionState { FOLLOWING, CONTROL, PORTAL, CANNON }

    // Use this for initialization
    void Start() {
        anim = gameObject.GetComponent<Animator>();
        player = GameObject.Find("Player").GetComponent<Player>();
        orientControl = GetComponent<Orientation>();
        gCheck = GetComponent<GroundCheck>();
        walkControl = GetComponent<Walk>();
        jumpControl = GetComponent<PlayerJump>();
        walkControl.enabled = false;
        jumpControl.enabled = false;
        _camera = FindObjectOfType<FollowPlayer>();
        rBody = GetComponent<Rigidbody2D>();
        GravityZoneOff();
        renderParts = GetComponentsInChildren<Renderer>();
        _followDistance = MinionDistance;
        _followSpeed = MinionFollowSpeed;
        CurrentState = MinionState.FOLLOWING;
        bodyCollider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate() {
        anim.SetBool("InAir", gCheck.InAir);
        if (!IsFollowing || CurrentState != MinionState.FOLLOWING)
            return;

        //RaycastHit2D groundCheckRay = Physics2D.Raycast(transform.position, -transform.up, 0.5f);
        //if (player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle") && checkIfSameHeight() && groundCheckRay.collider != null && groundCheckRay.collider.name.Contains("MovingPlatform"))
        //{
        //    if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.PORTRAIT || OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.INVERTED_PORTRAIT)
        //    {
        //        playerPosDiff = Mathf.Abs(prevPlayerLocation.x - player.transform.position.x);
        //        if (prevPlayerLocation.x > player.transform.position.x)
        //            transform.position = new Vector2(transform.position.x - playerPosDiff, transform.position.y);
        //        else
        //            transform.position = new Vector2(transform.position.x + playerPosDiff, transform.position.y);
        //    }
        //    else
        //    {
        //        playerPosDiff = Mathf.Abs(prevPlayerLocation.y - player.transform.position.y);
        //        if (prevPlayerLocation.y > player.transform.position.y)
        //            transform.position = new Vector2(transform.position.x, transform.position.y + playerPosDiff);
        //        else
        //            transform.position = new Vector2(transform.position.x, transform.position.y - playerPosDiff);
        //    }
        //}
        //else

        lerpToParent();

        if ((transform.position - _parent.transform.position).magnitude > teleportDistance)
            teleportToParent();

        //checkGravityScale();
        //prevPlayerLocation = player.transform.position;
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

    public void SetRenderOrder(int order)
    {
        SortingOrderScript[] orderChangers = gameObject.GetComponentsInChildren<SortingOrderScript>();
        foreach (var changer in orderChangers)
        {
            changer.SetOrderTo(order);
        }
    }

    public void SetRenderLayer(string layer)
    {
        SortingOrderScript[] orderChangers = gameObject.GetComponentsInChildren<SortingOrderScript>();
        foreach (var changer in orderChangers)
        {
            changer.SetLayerTo(layer);
        }
    }

    public void ToggleRender(bool state)
    {
        renderParts = GetComponentsInChildren<Renderer>();
        foreach (Renderer i in renderParts)
            i.enabled = state;
    }

    public void ToggleRenderPart(string targetPart)
    {
        foreach (var part in renderParts)
        {
            if (part.name == targetPart)
                part.enabled = !part.enabled;
        }
    }

    public void DeactivateControl(StateChange state)
    {
        switch (state)
        {
            case StateChange.PORTAL_IN:
                ToggleRender(false);
                bodyCollider.enabled = false;
                walkControl.enabled = false;
                jumpControl.enabled = false;
                rBody.gravityScale = 0;
                rBody.Sleep();
                break;
            case StateChange.CHECKPOINT:
                anim.SetBool("Checkpoint", true);
                IsFollowing = false;
                walkControl.enabled = false;
                jumpControl.enabled = false;
                rBody.isKinematic = true;
                break;
            case StateChange.DEATH:
                anim.SetBool("Dying", true);
                
                break;
        }
    }

    public void ReactivateControl(StateChange state)
    {
        switch (state)
        {
            case StateChange.PORTAL_OUT:
                ToggleRender(true);
                bodyCollider.enabled = true;
                walkControl.enabled = true;
                jumpControl.enabled = true;
                rBody.gravityScale = 1;
                break;
            case StateChange.MINION:
                rBody.gravityScale = 1;
                jumpControl.enabled = true;
                walkControl.enabled = true;
                _camera.setFollowObject(gameObject);
                IsFollowing = false;
                anim.SetBool("SwitchingToMinion", false);
                break;
        }
    }

    void checkGravityScale()
    {
        if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.PORTRAIT && transform.position.y > player.getPlayerFeet())
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
        else if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.PORTRAIT && transform.position.y <= player.getPlayerFeet())
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        else if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.INVERTED_PORTRAIT && transform.position.y > player.getPlayerFeet())
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        else if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.INVERTED_PORTRAIT && transform.position.y <= player.getPlayerFeet())
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;

        else if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.LANDSCAPE_LEFT && transform.position.x > player.getPlayerFeet())
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
        else if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.LANDSCAPE_LEFT && transform.position.x <= player.getPlayerFeet())
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        else if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.LANDSCAPE_RIGHT && transform.position.x > player.getPlayerFeet())
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        else if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.LANDSCAPE_RIGHT && transform.position.x <= player.getPlayerFeet())
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

    void checkVelocity()
    {
        if(_parent.name == "Player")
        {
            if(rBody.velocity.magnitude > _parent.GetComponent<Rigidbody2D>().velocity.magnitude)
            {
                rBody.velocity = _parent.GetComponent<Rigidbody2D>().velocity;
            }
        }
    }

    void teleportToParent()
    {
        transform.position = _parent.transform.position;
        rBody.velocity = Vector2.zero;
    }

    void lerpToParent()
    {
        if (_parent == null)
            return;
        if (_parent.name == "Player")
        {
            if (Vector2.Distance(transform.position, player.getPlayerFeetPosition()) > _followDistance)
            {
                transform.position = Vector2.Lerp(transform.position, player.getPlayerFeetPosition(), _followSpeed);
                anim.SetBool("Moving", true);
            }
            else
                anim.SetBool("Moving", false);
        }
        else if (Vector2.Distance(transform.position, _parent.transform.position) > _followDistance)
        {
            anim.SetBool("Moving", true);
            transform.position = Vector2.Lerp(transform.position, _parent.transform.position, _followSpeed);
        }
        else
            anim.SetBool("Moving", false);
    }

    void lerpToObject(GameObject target)
    {
        if (Vector2.Distance(transform.position, target.transform.position) > .01f)
        {
            anim.SetBool("Moving", true);
            transform.position = Vector2.Lerp(transform.position, target.transform.position, _followSpeed);
        }
    }

    void swipeCheck(TouchController.SwipeDirection direction)
    {
        if(direction == TouchController.SwipeDirection.DOWN && !IsFollowing && !gCheck.InAir)
        {
            if (LevelManager.Instance.NewCheckpointRequest(gameObject))
            {
                rBody.isKinematic = true;
                walkControl.enabled = false;
                jumpControl.enabled = false;
            }
        }
    }

    void setCheckpoint()
    {
        LevelManager.Instance.setNewCheckpoint();
    }

    public void GainControl()
    {
        SetRenderLayer("FrontOfPlayer");
        anim.SetBool("SwitchingToMinion", true);
        orientControl.syncWithPlayer = false;
    }

    private void triggerCheckpointAnim()
    {
        minionDeath();
        LevelManager.Instance.TriggerCheckpointAnim();
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
        _camera.setFollowObject(player.gameObject);
        player.switchControlToPlayer();
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

    private void syncWithPlayer(StateChange state)
    {
        if (!IsFollowing)
            return;
        switch (state)
        {
            case StateChange.PORTAL_IN:
                ToggleRender(false);
                rBody.gravityScale = 0;
                bodyCollider.enabled = false;
                break;
            case StateChange.PORTAL_OUT:
                ToggleRender(true);
                rBody.gravityScale = 1;
                bodyCollider.enabled = true;
                break;
            case StateChange.CANNON_COLLISION:
                rBody.gravityScale = 1;
                _followDistance = MinionDistance;
                _followSpeed = MinionFollowSpeed;
                break;
            case StateChange.CANNON:
                rBody.gravityScale = 0;
                _followDistance = 0.001f;
                _followSpeed = 0.5f;
                ToggleRender(false);
                break;
            case StateChange.CANNON_FIRE:
                ToggleRender(true);
                break;
            case StateChange.SWALK_ON:
                rBody.gravityScale = 0;
                _followSpeed = 0.2f;
                break;
            case StateChange.SWALK_OFF:
                rBody.gravityScale = 1;
                _followDistance = MinionDistance;
                _followSpeed = MinionFollowSpeed;
                break;
            case StateChange.BOX_IN:
                rBody.gravityScale = 0;
                _followDistance = 0.001f;
                _followSpeed = 0.5f;
                break;
            case StateChange.BOX_OUT:
                rBody.gravityScale = 1;
                _followDistance = MinionDistance;
                _followSpeed = MinionFollowSpeed;
                transform.position = player.transform.position;
                break;
        }
    }

    //Event handling for swipe events
    void OnEnable()
    {
        TouchController.OnSwipe += swipeCheck;
        TouchController.OnHold += flipSprite;
        Player.PlayerStateChange += syncWithPlayer;
    }
    void OnDisable()
    {
        TouchController.OnSwipe -= swipeCheck;
        TouchController.OnHold -= flipSprite;
        Player.PlayerStateChange -= syncWithPlayer;
    }

    //Check for deadly collisions
    void OnCollisionEnter2D(Collision2D collisionEvent)
    {
        if (!IsFollowing)
        {
            if (collisionEvent.gameObject.tag == "Hazard" || collisionEvent.relativeVelocity.magnitude > DeathSpeed)
            {
                DeactivateControl(StateChange.DEATH);
            }
        }
    }


}
