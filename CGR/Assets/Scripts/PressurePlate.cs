using UnityEngine;
using System.Collections;
using System;

public class PressurePlate : MonoBehaviour {

    public bool canBeUntriggered = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {

	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        BroadcastMessage("plateDepressed");
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (canBeUntriggered == true)
        {
            BroadcastMessage("plateReleased");
        }
    }

}
