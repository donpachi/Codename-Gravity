using UnityEngine;
using System.Collections;

public class PressurePlate : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void onCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.GetComponent<Rigidbody2D>().mass >= 1) 
        {
            // Door opens
        }
    }
}
