using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;

public delegate void PlayerDied();


public class Player : MonoBehaviour {

    private Rigidbody2D playerRigidBody;
    private LayerMask wallMask;
    private const float drag = 0.5f;
    private const float angularDrag = 0.05f;
    private bool facingRight;
    private bool inTransition;
    private bool launched;
    private Animator anim;
    
    public static Player Instance;
	public event PlayerDied OnPlayerDeath;
    public float deathSpeed = 10f;
    public bool inMinionArea;
    public float OnGroundRaySize;
    public float ForwardRaySize;
    public bool isMinion = false;
    public bool IsDead { get; private set; }
    public bool InRotation;
    public bool suctionStatus { get; private set; }
    public bool gravityZone { get; private set; }

    void Awake () {
        anim = this.GetComponent<Animator>();
        playerRigidBody = GetComponent<Rigidbody2D>();
        wallMask = 1 << LayerMask.NameToLayer("Walls");
        inMinionArea = false;
        suctionStatus = false;
        inTransition = false;
        facingRight = true;
        InRotation = false;
        GravityZoneOff();
    }

	void FixedUpdate () {
    }

    public void RespawnAt(Transform spawnPoint)
    {
        IsDead = false;

        transform.position = spawnPoint.position;
    }

    public void Kill()
    {
        IsDead = true;
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

    //void faceDirectionCheck()
    //{
    //    if (TouchController.Instance.getTouchDirection() == TouchController.TouchLocation.RIGHT && !facingRight)
    //        flipSprite();
    //    else if (TouchController.Instance.getTouchDirection() == TouchController.TouchLocation.LEFT && facingRight)
    //        flipSprite();
    //}

    //updates sprite to correct orientation
    //might have to update constant force while suction cups are on
    public void updatePlayerOrientation(OrientationListener.Orientation orientation, float timer)
    {
        if (gravityZone == true)
            return;
        if (suctionStatus == false || this.GetComponent<GroundCheck>().InAir == true)
        {
            switch (orientation)
            {
                case OrientationListener.Orientation.PORTRAIT:
                    //playerRotation.SetLookRotation(Vector3.forward, Vector3.up);
                    //transform.rotation = playerRotation;
                    anim.SetInteger("Orientation", 0);
                    break;
                case OrientationListener.Orientation.LANDSCAPE_RIGHT:
                    //playerRotation.SetLookRotation(Vector3.forward, Vector3.left);
                    //transform.rotation = playerRotation;
                    anim.SetInteger("Orientation", 3);
                    break;
                case OrientationListener.Orientation.LANDSCAPE_LEFT:
                    //playerRotation.SetLookRotation(Vector3.forward, Vector3.right);
                    //transform.rotation = playerRotation;
                    anim.SetInteger("Orientation", 1);
                    break;
                case OrientationListener.Orientation.INVERTED_PORTRAIT:
                    //playerRotation.SetLookRotation(Vector3.forward, Vector3.down);
                    //transform.rotation = playerRotation;
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
            TriggerDeath();
		}        

        else if (launched == true && collisionEvent.gameObject.tag == "Wall")
        {
            playerRigidBody.gravityScale = 1.0f;
            this.GetComponent<Walk>().enabled = true;
            playerRigidBody.drag = drag;
            playerRigidBody.angularDrag = angularDrag;
            launched = false;
        }
	}

    void OnTriggerEnter2D(Collider2D colliderEvent)
    {
        if (colliderEvent.gameObject.tag == "SpikeTop")
        {
            TriggerDeath();
        }
    }

    void OnTriggerExit2D(Collider2D colliderEvent)
    {
        if (colliderEvent.gameObject.tag == "Hazard")
        {
            TriggerDeath();
        }
    }

    public void TriggerDeath()
	{
        anim.SetBool("Dying", true);
        
	}

    public void killPlayer()
    {
        if (OnPlayerDeath != null && isMinion == false)
        {
            OnPlayerDeath();
        }
        else if (isMinion == true)
        {
            GameObject potato = GameObject.Find("Player");
            potato.GetComponent<Player>().enabled = true;
            //GameObject.Find("Main Camera").GetComponent<FollowPlayer>().player = potato;
            Camera.current.gameObject.GetComponent<FollowPlayer>().setFollowObject(potato);
            potato.GetComponent<Walk>().enabled = true;
            foreach (GameObject minionSpawner in GameObject.FindGameObjectsWithTag("MinionSpawner"))
            {
                if (GameObject.FindGameObjectsWithTag("Minion").Length == 1)
                    minionSpawner.GetComponent<MinionSpawn>().minionsSpawned--;
            }
            switchControlToPlayer();
            LevelManager.Instance.RemoveMinion(gameObject);
            Destroy(gameObject);
        }
    }
    void swipeCheck(TouchController.SwipeDirection direction)
    {
        if (direction == TouchController.SwipeDirection.UP)
        {
            if (inMinionArea == true && LevelManager.Instance.GetMinionCount() != 0 && !isMinion)
            {
                isMinion = true;
                switchControlToMinion();
            }
        }
        else if(direction == TouchController.SwipeDirection.DOWN && !isMinion)
        {
            if (!GetComponent<GroundCheck>().InAir)
            {
                LevelManager.Instance.NewCheckpointRequest(gameObject);
            }
        }
    }

    void switchControlToMinion()
    {
        GameObject controllingMinion = LevelManager.Instance.GetMinion();
        GetComponent<Walk>().enabled = false;
        GetComponent<Rigidbody2D>().isKinematic = true;
        GetComponent<Animator>().SetBool("Moving", false);
        controllingMinion.GetComponent<Animator>().SetBool("SwitchingToMinion", true);
    }

    public void switchControlToPlayer()
    {
        GetComponent<Rigidbody2D>().isKinematic = false;
        GetComponent<Rigidbody2D>().gravityScale = 1.0f;
        GetComponent<Walk>().enabled = true;
        isMinion = false;
    }

    void screenTouched(TouchInstanceData data)
    {
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

    public void ToggleRender()
    {
        Renderer[] potatoParts = this.GetComponentsInChildren<Renderer>();
        foreach (Renderer i in potatoParts)
            i.enabled = !i.enabled;
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
        GetComponent<ConstantForce2D>().enabled = true;
        GetComponent<ConstantForce2D>().relativeForce = new Vector2(0, -1) * force;
        GetComponent<Walk>().enabled = false;
        GetComponent<SuctionWalk>().enabled = true; ;
    }

    public void SuctionStatusEnd()
    {
        suctionStatus = false;
        //gravitySpriteUpdate(OrientationListener.instanceOf.currentOrientation(), 0);
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

    /// <summary>
    /// On when in Portal
    /// </summary>
    public void InTransitionStatusOn()
    {
        inTransition = true;
    }
    /// <summary>
    /// Off when u leave portal
    /// </summary>
    public void InTransitionStatusEnd()
    {
        inTransition = false;
    }

    public bool IsInTransition()
    {
        return inTransition;
    }
}
