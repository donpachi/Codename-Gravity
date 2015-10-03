﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GravityCannon : MonoBehaviour {

	private GameObject player;
	private Rigidbody2D playerBody;
	public Animator anim;
	public GameObject cannonTip;
	public GameObject[] buttons;

	private float LAUNCHFORCE = 5.0f;
	
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnCollisionEnter2D(Collision2D collisionInfo) {
		if (collisionInfo.gameObject.tag == "Player") {
			playerBody = collisionInfo.rigidbody;
			Vector3 hiddenPosition = new Vector3 (cannonTip.transform.position.x,
			                                      cannonTip.transform.position.y,
			                                      1);
			playerBody.GetComponent<Transform>().position = (hiddenPosition);
			player.GetComponent<Controls>().resetMovementFlags();
			playerBody.gravityScale = 0f;
			playerBody.Sleep();

			anim.SetBool("activated", true);

			for (int i = 0; i < buttons.Length - 1; i++)
				buttons[i].SetActive(false);
		}
	}

	public void EnableFireButton() {
		buttons[buttons.Length - 1].SetActive(true);
	}

	public void FireDown() {
		Vector2 direction;
		Vector3 firePosition = new Vector3 (cannonTip.transform.position.x,
		                                   cannonTip.transform.position.y,
		                                   -2);
		playerBody.GetComponent<Transform>().position = (firePosition);
		playerBody.WakeUp();

		player.GetComponent<Controls>().launchStatusOn();
		direction = (player.GetComponent<Transform>().position - this.GetComponent<Transform> ().position).normalized;
		player.GetComponent<Controls>().addForce(direction * LAUNCHFORCE, ForceMode2D.Impulse);

		anim.SetBool("activated", false);
		
		for (int i = 0; i < buttons.Length - 1; i++)
			buttons[i].SetActive(true);

		buttons[buttons.Length - 1].SetActive(false);

	}
}
