﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Portal : MonoBehaviour {

    public GameObject linkedPortal;
    public float transitionSpeed;

    private List<Node> bodies;
    private float distanceThreshold;
    private Vector3 linkedPortalPosition;
    private GameObject playerStatus;
    private bool playerComing;

	// Use this for initialization
	void Start () {
        linkedPortalPosition = linkedPortal.transform.position;
        playerStatus = GameObject.FindGameObjectWithTag("Player");
        distanceThreshold = 0.1f;
        bodies = new List<Node>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (bodies.Count > 0)
        {
            for (int i = 0; i < bodies.Count; i++)
            {
                bodies[i].body.transform.position = new Vector3(Mathf.Lerp(bodies[i].body.transform.position.x, linkedPortalPosition.x, transitionSpeed * Time.deltaTime),
                                Mathf.Lerp(bodies[i].body.transform.position.y, linkedPortalPosition.y, transitionSpeed * Time.deltaTime),
                                0f);
                if ((linkedPortalPosition - bodies[i].body.transform.position).magnitude <= distanceThreshold)
                {
                    bodies[i].body.Sleep();
                    StartCoroutine(LaunchPlayer(bodies[i], 2f));
                    bodies.RemoveAt(i);
                    i -= 1;
                }
            }
        }
	}

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.tag != "Water")
        {
            Rigidbody2D tempBody = collisionInfo.rigidbody;
            Collider2D[] colliders = tempBody.GetComponents<Collider2D>();
            bool isPlayer = collisionInfo.gameObject.tag.Equals("Player");

            if (isPlayer && playerComing)
                return;
            if (isPlayer)
            {
                GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<Player>().InTransitionStatusOn();
                linkedPortal.GetComponent<Portal>().PlayerComing();
            }
            tempBody.gravityScale = 0;
            tempBody.velocity = new Vector2(0, 0);
            tempBody.Sleep();
            playerStatus.GetComponent<Player>().ToggleRender();
            playerStatus.GetComponent<Walk>().enabled = false;
            playerStatus.GetComponent<SuctionWalk>().enabled = false;

            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].enabled = false;
            }
            bodies.Add(new Node(tempBody, tempBody.transform.TransformVector(collisionInfo.relativeVelocity)));
        }
    }

    void OnTriggerExit2D(Collider2D collisionInfo)
    {
        bool isPlayer = collisionInfo.gameObject.tag.Equals("Player");
        if (isPlayer)
            playerComing = false;
    }

    IEnumerator LaunchPlayer(Node entity, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        Collider2D[] colliders = entity.body.GetComponents<Collider2D>();
        float launchAngle = (this.transform.rotation.eulerAngles.z - linkedPortal.transform.rotation.eulerAngles.z);
        entity.body.transform.position = new Vector3(entity.body.transform.position.x,
                                           entity.body.transform.position.y,
                                           -2f);

        entity.body.gravityScale = 1;
        entity.velocity = Quaternion.AngleAxis(launchAngle, Vector3.forward) * entity.velocity;
        entity.body.AddForce(entity.velocity * entity.velocity.magnitude * 5);

        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = true;
        }

        if (playerStatus.GetComponent<Player>().IsSuctioned())
            playerStatus.GetComponent<SuctionWalk>().enabled = true;
        playerStatus.GetComponent<Player>().ToggleRender();
        playerStatus.GetComponent<Walk>().enabled = true;
        playerStatus.GetComponent<Player>().InTransitionStatusEnd(); 
    }

    public void PlayerComing()
    {
        playerComing = true;
    }
}

class Node
{
    public Rigidbody2D body;
    public Vector2 velocity;

    public Node(Rigidbody2D rb, Vector2 v)
    {
        body = rb;
        velocity = v;
    }
}
