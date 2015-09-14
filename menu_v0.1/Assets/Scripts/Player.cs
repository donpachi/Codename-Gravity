using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;

public delegate void PlayerDied();


public class Player : MonoBehaviour {


	// Use this for initialization

	public event PlayerDied OnPlayerDeath;
    public float deathSpeed = 25;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
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
