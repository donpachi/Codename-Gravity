using UnityEngine;
using System.Collections;

public class DestroyObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D (Collision2D collision){
		//Just destroys the object for now. may want to change to an animation
		Destroy (gameObject);
	}

}
