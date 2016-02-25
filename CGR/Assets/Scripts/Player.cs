﻿using UnityEngine;
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
    private bool suctionStatus;
    private bool inTransition;
    private bool launched;
    
    public static Player Instance;
	public event PlayerDied OnPlayerDeath;
    public float deathSpeed = 10f;
    public bool inAir;
    public bool inMinionArea;
    public float OnGroundRaySize;
    public float ForwardRaySize;
    public bool isMinion = false;

    void Awake () {
        playerRigidBody = GetComponent<Rigidbody2D>();
        wallMask = 1 << LayerMask.NameToLayer("Walls");
        inMinionArea = false;
        inAir = false;
        suctionStatus = false;
        inTransition = false;
        facingRight = true;
	}
	
	
	void FixedUpdate () {

        if (!inTransition)
        {
            groundCheck();
            ForwardCheck();
        }
        faceDirectionCheck();
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

    //Raycasts down to check for a floor
    void groundCheck()
    {
        if (suctionStatus == false){
            Vector3 rayOffset = new Vector3(0.1f, 0, 0);
            RaycastHit2D groundCheckRay = Physics2D.Raycast(transform.position + rayOffset, OrientationListener.instanceOf.getWorldDownVector(), OnGroundRaySize, wallMask);
            RaycastHit2D groundCheckRay1 = Physics2D.Raycast(transform.position - rayOffset, OrientationListener.instanceOf.getWorldDownVector(), OnGroundRaySize, wallMask);

            if (groundCheckRay.collider != null || groundCheckRay1.collider != null)
            {
                inAir = false;
            }
            else
                inAir = true;
        }
        else {
            RaycastHit2D groundCheckRay = Physics2D.Raycast(transform.position, playerRigidBody.GetComponent<ConstantForce2D>().force.normalized, OnGroundRaySize, wallMask);

            if (groundCheckRay.collider != null)
            {
                inAir = false;
                playerRigidBody.gravityScale = 0.0f;
                playerRigidBody.GetComponent<ConstantForce2D>().enabled = true;
            }
            else
            {
                inAir = true;
                gravitySpriteUpdate(OrientationListener.instanceOf.currentOrientation(), 0);
                playerRigidBody.GetComponent<ConstantForce2D>().enabled = false;
                playerRigidBody.GetComponent<ConstantForce2D>().force = Physics2D.gravity * 3;
                this.GetComponent<SuctionWalk>().GetVectors(OrientationListener.instanceOf.getRelativeDownVector());
            }
        }

    }

    void faceDirectionCheck()
    {
        if (TouchController.Instance.getTouchDirection() == TouchController.TouchLocation.RIGHT && !facingRight)
            flipSprite();
        else if (TouchController.Instance.getTouchDirection() == TouchController.TouchLocation.LEFT && facingRight)
            flipSprite();
    }

    //updates sprite to correct orientation
    //might have to update constant force while suction cups are on
    void gravitySpriteUpdate(OrientationListener.Orientation orientation, float timer)
    {
        Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(0, 90, 0)), 10 * Time.deltaTime);
        Quaternion playerRotation = transform.localRotation;
        //playerRotation.SetLookRotation(Vector3.forward, Vector3.left);
        if (suctionStatus == false || inAir == true)
        {
            switch (orientation)
            {
                case OrientationListener.Orientation.PORTRAIT:
                    playerRotation.SetLookRotation(Vector3.forward, Vector3.up);
                    transform.rotation = playerRotation;
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

    void ForwardCheck()
    {
        Vector2 forwardVector;
        Vector2 currentDownVector = playerRigidBody.GetComponent<ConstantForce2D>().force.normalized;
        float degrees;
        
        if (facingRight)
        {
            degrees = 90;
            float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
            float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);
            forwardVector.x = Mathf.Round( (cos * currentDownVector.x) - (sin * currentDownVector.y) );
            forwardVector.y = Mathf.Round( (sin * currentDownVector.x) + (cos * currentDownVector.y) );
        }

        else
        {
            degrees = -90;
            float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
            float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);
            forwardVector.x = Mathf.Round( (cos * currentDownVector.x) - (sin * currentDownVector.y) );
            forwardVector.y = Mathf.Round( (sin * currentDownVector.x) + (cos * currentDownVector.y) );
        }

        //print(forwardVector.ToString());

        RaycastHit2D forwardCheckRay = Physics2D.Raycast(transform.position, forwardVector, ForwardRaySize, wallMask);
        if (forwardCheckRay.collider != null)
        {
            this.GetComponent<SuctionWalk>().GetVectors(forwardVector);
            this.GetComponent<ConstantForce2D>().force = forwardVector.normalized * 75;
            this.transform.Rotate(new Vector3 (0,0, degrees));
        }
        // raycast based on the boolean value facingRight
        // use constant force value to derive the new left and right vectors
        // reorientate the player
    }

    /*---------------Event Functions Start Here---------------*/
    void OnCollisionEnter2D(Collision2D collisionEvent) {
        if (collisionEvent.gameObject.tag == "Hazard" || collisionEvent.relativeVelocity.magnitude > deathSpeed && collisionEvent.gameObject.layer == 10 || collisionEvent.relativeVelocity.magnitude > 10 && collisionEvent.gameObject.tag == "Boulder")
        {
            TriggerDeath();
		}        

        else if (launched == true)
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

	public void TriggerDeath()
	{
        if (OnPlayerDeath != null && isMinion == false)
        {
            OnPlayerDeath();
        }
        else if (isMinion == true)
        {
            GameObject potato = GameObject.Find("Player");
            potato.GetComponent<Player>().enabled = true;
            GameObject.Find("Main Camera").GetComponent<FollowPlayer>().player = potato;
            potato.GetComponent<Walk>().enabled = true;
            foreach (GameObject minionSpawner in GameObject.FindGameObjectsWithTag("MinionSpawner")) 
            {
                if (GameObject.FindGameObjectsWithTag("Minion").Length == 1)
                    minionSpawner.GetComponent<MinionSpawn>().minionsSpawned--;
            }
            switchControl();
            Destroy(gameObject);
        }
	}

    void switchControl()
    {
        foreach (GameObject minion in GameObject.FindGameObjectsWithTag("Minion"))
        {
            this.GetComponent<Player>().enabled = false;
            this.GetComponent<Rigidbody2D>().gravityScale = 0;
            this.GetComponent<Player>().isMinion = false;
            this.GetComponent<PlayerJump>().enabled = false;
            this.GetComponent<Walk>().enabled = false;
            minion.GetComponent<Minion>().enabled = true;
            //minion.tag = "Minion";
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

    public void SuctionStatusOn()
    {
        suctionStatus = true;
    }

    public void SuctionStatusEnd()
    {
        suctionStatus = false;
        gravitySpriteUpdate(OrientationListener.instanceOf.currentOrientation(), 0);
    }

    public bool IsSuctioned()
    {
        return suctionStatus;
    }

    public void InTransitionStatusOn()
    {
        inTransition = true;
    }

    public void InTransitionStatusEnd()
    {
        inTransition = false;
    }

    public bool IsInTransition()
    {
        return inTransition;
    }
}
