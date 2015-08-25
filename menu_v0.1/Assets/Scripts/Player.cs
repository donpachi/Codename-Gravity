﻿using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;

public delegate void PlayerDied();

public class Player : MonoBehaviour {

	// Use this for initialization

	public event PlayerDied OnPlayerDeath;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void addForce(Vector3 vector) {

	}


	void OnCollisionEnter(Collision obj) {
		if (obj.gameObject.tag == "Hazard") {
			if(OnPlayerDeath != null)
			{
				OnPlayerDeath();
			}
		}
	}
	

}
