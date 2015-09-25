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

	void Start () {
        playerRigidBody = GetComponent<Rigidbody2D>();
        wallMask = 1 << LayerMask.NameToLayer("Walls");
        inAir = false;
	}
	
	
	void FixedUpdate () {
        RaycastHit2D groundCheckRay = Physics2D.Raycast(transform.position, Vector2.down, 1, wallMask);

        if (groundCheckRay.collider != null)
            inAir = true;
        else
            inAir = false;
	}

	void addForce(Vector3 vector) {

	}


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
