using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;

public delegate void PlayerDied();


public class Player : MonoBehaviour {

    private Rigidbody2D playerRigidBody;
    private Walk walk;
    private SuctionWalk sWalk;
    private ConstantForce2D sForce;
    private LayerMask wallMask;
    private CircleCollider2D playerCollider;
    private float drag = 0.5f;
    private float angularDrag = 0.05f;
    private bool facingRight;
    private bool inTransition;
    private bool launched;
    private Animator anim;
    private GroundCheck gCheck;
    private Renderer[] potatoParts;

	public event PlayerDied OnPlayerDeath;
    public float deathSpeed = 10f;
    public bool inMinionArea;
    public float OnGroundRaySize;
    public float ForwardRaySize;
    public bool isMinion = false;
    public bool InRotation;
    public bool suctionStatus { get; private set; }
    public bool gravityZone { get; private set; }
    public enum StateChange { CANNON, CANNON_COLLISION, PORTAL, MINION, SWALK, BOX, CHECKPOINT }   //The script that wants to effect player

    void Awake () {
        anim = this.GetComponent<Animator>();
        walk = GetComponent<Walk>();
        sWalk = GetComponent<SuctionWalk>();
        sForce = GetComponent<ConstantForce2D>();
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CircleCollider2D>();
        drag = playerRigidBody.drag;
        angularDrag = playerRigidBody.angularDrag;
        wallMask = 1 << LayerMask.NameToLayer("Walls");
        gCheck = GetComponent<GroundCheck>();
        potatoParts = GetComponentsInChildren<Renderer>();

        inMinionArea = false;
        suctionStatus = false;
        inTransition = false;
        facingRight = true;
        InRotation = false;
        GravityZoneOff();
    }

    public void CheckpointRespawn(Transform spawnPoint)
    {
        transform.position = spawnPoint.position;
        anim.SetBool("Dying", false);
    }

    public float getPlayerFeet()
    {
        if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.PORTRAIT)
            return transform.position.y - 0.25f;
        else if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.INVERTED_PORTRAIT)
            return transform.position.y + 0.25f;
        else if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.LANDSCAPE_LEFT)
            return transform.position.x - 0.25f;
        else 
            return transform.position.x + 0.25f;
    }

    public Vector2 getPlayerFeetVector()
    {
        if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.PORTRAIT)
            return new Vector2(transform.position.x, transform.position.y - 0.25f);
        else if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.INVERTED_PORTRAIT)
            return new Vector2(transform.position.x, transform.position.y + 0.25f);
        else if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.LANDSCAPE_LEFT)
            return new Vector2(transform.position.x - 0.25f, transform.position.y);
        else
            return new Vector2(transform.position.x + 0.25f, transform.position.y);
    }

    /// <summary>
    /// Called by animator. Resets flag when rotation is done
    /// </summary>
    void finishedRotation()
    {
        InRotation = false;
    }

    //updates sprite to correct orientation
    //might have to update constant force while suction cups are on
    public void updatePlayerOrientation(OrientationListener.Orientation orientation, float timer)
    {
        if (gravityZone == true || isMinion == true)
            return;
        if (suctionStatus == false || gCheck.InAir == true)
        {
            switch (orientation)
            {
                case OrientationListener.Orientation.PORTRAIT:
                    anim.SetInteger("Orientation", 0);
                    break;
                case OrientationListener.Orientation.LANDSCAPE_RIGHT:
                    anim.SetInteger("Orientation", 3);
                    break;
                case OrientationListener.Orientation.LANDSCAPE_LEFT:
                    anim.SetInteger("Orientation", 1);
                    break;
                case OrientationListener.Orientation.INVERTED_PORTRAIT:
                    anim.SetInteger("Orientation", 2);
                    break;
            }
        }
    }

    //Flip character while moving left and right
    void flipSprite()
    {
        facingRight = !facingRight;
        Vector3 playerScale = transform.localScale;
        //if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.PORTRAIT
        //        || OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.INVERTED_PORTRAIT)
            playerScale.x *= -1;
        //else
        //    playerScale.y *= -1;

        transform.localScale = playerScale;
    }

    /*---------------Event Functions Start Here---------------*/
    void OnCollisionEnter2D(Collision2D collisionEvent) {
        if (collisionEvent.gameObject.tag == "Hazard" || collisionEvent.relativeVelocity.magnitude > deathSpeed && collisionEvent.gameObject.layer == 10 || collisionEvent.relativeVelocity.magnitude > deathSpeed && collisionEvent.gameObject.tag == "Boulder")
        {
            TriggerDeath("deadly/hazard collision");
		}        

        else if (launched == true && collisionEvent.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            ReactivateControl(StateChange.CANNON_COLLISION);
        }
	}

    void OnTriggerEnter2D(Collider2D colliderEvent)
    {
        if (colliderEvent.gameObject.tag == "SpikeTop")
        {
            TriggerDeath("SpikeTop");
        }
    }

    void OnTriggerExit2D(Collider2D colliderEvent)
    {
        if (colliderEvent.gameObject.tag == "Boundary")
        {
            TriggerDeath("Boundary");
        }
    }

    public void TriggerDeath(String reason)
	{
        anim.SetBool("Dying", true);
        Debug.Log("Player died because of: " + reason);
	}

    void killPlayer()
    {
        if (OnPlayerDeath != null && isMinion == false)
        {
            OnPlayerDeath();
        }
        else if (isMinion == true)
        {
            enabled = true;
            switchControlToPlayer();
            LevelManager.Instance.RemoveMinion(gameObject, true);
        }
    }
    void swipeCheck(TouchController.SwipeDirection direction)
    {
        if (direction == TouchController.SwipeDirection.UP)
        {
            if (inMinionArea == true && LevelManager.Instance.GetMinionCount() != 0 && !isMinion)
            {
                switchControlToMinion();
            }
        }
        else if(direction == TouchController.SwipeDirection.DOWN && !isMinion && !gCheck.InAir)
        {
            LevelManager.Instance.NewCheckpointRequest(gameObject);
        }
    }

    void switchControlToMinion()
    {
        GameObject controllingMinion = LevelManager.Instance.GetMinion();
        walk.enabled = false;
        playerRigidBody.isKinematic = true;
        anim.SetBool("Moving", false);
        controllingMinion.GetComponent<Animator>().SetBool("SwitchingToMinion", true);
        WorldGravity.Instance.enabled = false;
        isMinion = true;
    }

    public void switchControlToPlayer()
    {
        try
        {
            Camera.main.gameObject.GetComponent<FollowPlayer>().setFollowObject(gameObject);
        }
        catch(NullReferenceException e)
        {
            Debug.LogError("Camera Obj: " + Camera.main.gameObject + " FollowP script: " + Camera.main.gameObject.GetComponent<FollowPlayer>() + " Player GameObject: " + gameObject);
        }        
            playerRigidBody.isKinematic = false;
            playerRigidBody.gravityScale = 1.0f;
            walk.enabled = true;
            WorldGravity.Instance.enabled = true;
            isMinion = false;
    }

    public void ReactivateControl(StateChange state)
    {
        if (state == StateChange.SWALK)
        {
            suctionStatus = false;
            sWalk.enabled = false;
            sForce.enabled = false;
        }
        else if (state == StateChange.CANNON)
        {
            ToggleRender(true);
            playerCollider.enabled = true;
        }
        else if(state == StateChange.CANNON_COLLISION)
        {
            playerRigidBody.drag = drag;
            playerRigidBody.angularDrag = angularDrag;
            launched = false;
        }
        else if (state == StateChange.PORTAL)
        {
            inTransition = false;
            ToggleRender(true);
            playerCollider.enabled = true;
        }
        else if(state == StateChange.BOX)
        {

        }
        else if(state == StateChange.CHECKPOINT)
        {
            playerRigidBody.drag = drag;
            playerRigidBody.angularDrag = angularDrag;
            ToggleRender(true);
            playerCollider.enabled = true;
            inTransition = false;
            launched = false;
            isMinion = false;
        }

        if (!launched)
        {
            if (suctionStatus)
            {
                sWalk.enabled = true;
                sForce.enabled = true;
            }
            else
            {
                walk.enabled = true;
                playerRigidBody.gravityScale = 1;
            }
            if(state != StateChange.CHECKPOINT)
                updatePlayerOrientation(WorldGravity.Instance.CurrentGravityDirection, 0);
        }
    }

    public void DeactivateControl(StateChange state)
    {
        walk.enabled = false;
        sWalk.enabled = false;
        sForce.enabled = false;
        playerRigidBody.gravityScale = 0;
        playerRigidBody.Sleep();

        if (state == StateChange.CANNON)
        {
            launched = true;
            playerRigidBody.drag = 0;
            playerRigidBody.angularDrag = 0;
            ToggleRender(false);
            playerCollider.enabled = false;
        }
        if(state == StateChange.PORTAL)
        {
            inTransition = true;
            ToggleRender(false);
            playerCollider.enabled = false;
        }
        if(state == StateChange.BOX)
        {

        }
    }

    public PlayerState SavePlayerState()
    {
        PlayerState data = new PlayerState();
        data.suctionStatus = suctionStatus;
        if(suctionStatus)
            data.suctionTimer = sWalk.GetTimer();
        data.orientation = anim.GetInteger("Orientation");

        return data;
    }

    public void LoadPlayerState(PlayerState state)
    {
        suctionStatus = state.suctionStatus;
        if (suctionStatus)
            sWalk.SetTimer(state.suctionTimer);
        gravityZone = state.gravityZone;
        anim.SetInteger("Orientation", state.orientation);  //currently doesnt do much
        ReactivateControl(StateChange.CHECKPOINT);
    }

    void screenTouched(TouchInstanceData data)
    {
        if (isMinion)
            return;

        if (data.touchLocation == TouchController.TouchLocation.RIGHT && !facingRight)
            flipSprite();
        else if (data.touchLocation == TouchController.TouchLocation.LEFT && facingRight)
            flipSprite();
    }

    //Listeners for player
    void OnEnable()
    {
        WorldGravity.GravityChanged += updatePlayerOrientation;
        TouchController.OnSwipe += swipeCheck;
        TouchController.ScreenTouched += screenTouched;
    }

    void OnDisable()
    {
        WorldGravity.GravityChanged -= updatePlayerOrientation;
        TouchController.OnSwipe -= swipeCheck;
        TouchController.ScreenTouched -= screenTouched;
    }

    public void ToggleRender(bool state)
    {
        foreach (Renderer i in potatoParts)
            i.enabled = state;
    }

    public void LaunchStatusOn()
    {
        launched = true;
    }

    public bool IsLaunched()
    {
        return launched;
    }

    /// <summary>
    /// Turns on suction status and sets players constant force to requested force
    /// </summary>
    /// <param name="force"></param>
    public void SuctionStatusOn(int force)
    {
        suctionStatus = true;
        sForce.relativeForce = new Vector2(0, -1) * force;
        walk.enabled = false;
        sWalk.enabled = true;
    }

    public void SuctionStatusEnd()
    {
        suctionStatus = false;
    }

    public void GravityZoneOn()
    {
        gravityZone = true;
    }

    public void GravityZoneOff()
    {
        gravityZone = false;
    }

    public bool IsSuctioned()
    {
        return suctionStatus;
    }

    public bool IsInTransition()
    {
        return inTransition;
    }
}

public class PlayerState
{
    public float suctionTimer;
    public bool suctionStatus;
    public bool gravityZone;
    public int orientation;
}
