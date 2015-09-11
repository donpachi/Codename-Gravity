﻿using UnityEngine;
using System.Collections;

public class WallTurret : MonoBehaviour {

	public GameObject projectile;
	private float timer;
	private Vector3 spawnLocation;

	//Publicly changable variables
	public float FireRate;
	public float InitialDelay;
	public float ProjectileForce;
	public Vector3 SpawnOffset;
	public Vector2 LaunchForce;
	public Quaternion LaunchAngle;

	// Use this for initialization
	void Start () {
		timer = InitialDelay;
		spawnLocation = gameObject.transform.position + SpawnOffset;
	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		if (timer < 0) {
			GameObject newMissile = (GameObject) Instantiate(projectile, spawnLocation, Quaternion.identity);
			newMissile.GetComponent<Rigidbody2D>().AddForce(LaunchForce);
			timer = FireRate;
		}
	}
}