using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;

public delegate void PlayerDied();


public class Player : MonoBehaviour {

    private Rigidbody2D playerRigidBody;
    private LayerMask wallMask;

	// Use this for initialization

	public event PlayerDied OnPlayerDeath;
    public float deathSpeed = 25;
    public bool inAir;
    public float OnGroundRaySize;

	void Awake () {
        playerRigidBody = GetComponent<Rigidbody2D>();
        wallMask = 1 << LayerMask.NameToLayer("Walls");
        inAir = false;
	}
	
	
	void FixedUpdate () {

        groundCheck();



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

    //updates sprite to correct orientation
    void spriteUpdate()
    {
        OrientationListener.Orientation current = OrientationListener.instanceOf.currentOrientation();

        switch (current)
        {
            case OrientationListener.Orientation.PORTRAIT:

                break;
            case OrientationListener.Orientation.LANDSCAPE_LEFT:

                break;
        }

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
}
