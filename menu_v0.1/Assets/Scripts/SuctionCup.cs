﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SuctionCup : MonoBehaviour {

    public float suctionTimer;

    private float timer;
    private bool triggered;
    private GameObject player;
    private Rigidbody2D playerBody;
	private Vector2 suctionVector;

    //Event thrown when picked up
    public delegate void SuctionCupActivated();
    public static event SuctionCupActivated SCActivated;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        timer = 0;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if (timer != 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                suctionCupBootsEnd();
            }
        }

	}

	void OnTriggerEnter2D(Collider2D collisionInfo) {
		if (collisionInfo.gameObject.tag == "Player" && !triggered) {
            playerBody = collisionInfo.GetComponent<Rigidbody2D>();
            player.GetComponent<Player>().SuctionStatusOn();
            player.GetComponent<Walk>().enabled = false;
            player.GetComponent<PlayerJump>().enabled = false;
            player.GetComponent<SuctionWalk>().enabled = true;
            player.GetComponent<SuctionWalk>().GetVectors(OrientationListener.instanceOf.getRelativeDownVector());

            this.GetComponent<Collider2D>().enabled = false;
            this.GetComponent<SpriteRenderer>().enabled = false;

            timer = suctionTimer;
            player.GetComponent<SuctionWalk>().SetTimer(suctionTimer);
		}
	}

    private void suctionCupBootsEnd()
    {
        timer = 0;

        this.GetComponent<Collider2D>().enabled = true;
        this.GetComponent<SpriteRenderer>().enabled = true;
    }
}