using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;

public delegate void PlayerDied();


public class Player : MonoBehaviour {

    private Rigidbody2D playerRigidBody;
    private LayerMask wallMask;
    private bool facingRight;

    public static Player Instance;
	public event PlayerDied OnPlayerDeath;
    public float deathSpeed = 25;
    public bool inAir;
    public float OnGroundRaySize;

	void Awake () {
        playerRigidBody = GetComponent<Rigidbody2D>();
        wallMask = 1 << LayerMask.NameToLayer("Walls");
        inAir = false;
        facingRight = true;
	}
	
	
	void FixedUpdate () {

        groundCheck();
        faceDirectionCheck();

	}

    //Raycasts down to check for a floor
    void groundCheck()
    {
        RaycastHit2D groundCheckRay = Physics2D.Raycast(transform.position, OrientationListener.instanceOf.getRelativeDownVector(), OnGroundRaySize, wallMask);

        if (groundCheckRay.collider != null)
            inAir = false;
        else
            inAir = true;
    }

    void faceDirectionCheck()
    {
        if (TouchController.Instance.getTouchDirection() == TouchController.TouchLocation.RIGHT && !facingRight)
            flipSprite();
        else if (TouchController.Instance.getTouchDirection() == TouchController.TouchLocation.LEFT && facingRight)
            flipSprite();
    }

    //updates sprite to correct orientation
    void gravitySpriteUpdate(OrientationListener.Orientation orientation)
    {
        Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(0, 90, 0)), 10 * Time.deltaTime);
        Quaternion playerRotation = transform.localRotation;
        //playerRotation.SetLookRotation(Vector3.forward, Vector3.left);
        switch (orientation)
        {
            case OrientationListener.Orientation.PORTRAIT:
                playerRotation.SetLookRotation(Vector3.forward, Vector3.up);
                break;
            case OrientationListener.Orientation.LANDSCAPE_RIGHT:
                playerRotation.SetLookRotation(Vector3.forward, Vector3.left);
                transform.rotation = playerRotation;
                break;
            case OrientationListener.Orientation.LANDSCAPE_LEFT:
                playerRotation.SetLookRotation(Vector3.forward, Vector3.right);
                transform.rotation = playerRotation;
                break;
            case OrientationListener.Orientation.INVERTED_PORTRAIT:
                playerRotation.SetLookRotation(Vector3.forward, Vector3.down);
                transform.rotation = playerRotation;
                break;
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

        if (collisionEvent.gameObject.tag == "Hazard" || collisionEvent.relativeVelocity.magnitude > deathSpeed)
        {
			if(OnPlayerDeath != null)
			{
				OnPlayerDeath();
			}
		}

        
	}

    void OnTriggerEnter2D(Collider2D colliderEvent)
    {
        if (colliderEvent.gameObject.tag == "SpikeTop")
        {
            if (OnPlayerDeath != null)
            {
                OnPlayerDeath();
            }
        }
    }

	public void TriggerDeath()
	{
		if(OnPlayerDeath != null)
		{
			OnPlayerDeath();
		}
	}

    //Listeners for player
    void OnEnable()
    {
        WorldGravity.GravityChanged += gravitySpriteUpdate;
    }

    void OnDisable()
    {
        WorldGravity.GravityChanged -= gravitySpriteUpdate;
    }

}
