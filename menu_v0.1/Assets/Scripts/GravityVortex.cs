using UnityEngine;
using System.Collections;

public class GravityVortex : MonoBehaviour {
	private GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 playerPosition = player.GetComponent<Transform>().position;
		Vector3 vortexPosition = this.GetComponent<Transform> ().position;

		Debug.Log (playerPosition - vortexPosition);
	}
}
