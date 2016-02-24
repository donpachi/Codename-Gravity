using UnityEngine;
using System.Collections;
using System;

public class PressurePlate : MonoBehaviour {

    public bool canBeUntriggered = false;
    public float releaseDelay = 0;
    public float timer = 0;
    bool TimerCountingDown = false;

	// Use this for initialization
	void Start () {
        timer = releaseDelay;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (TimerCountingDown == true)
            timer -= Time.deltaTime;
        checkIfRelease();
	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Pushable")
        {
            TimerCountingDown = false;
            timer = releaseDelay;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Pushable")
        {
            if (canBeUntriggered == true)
                TimerCountingDown = true;
        }
    }

    void checkIfRelease()
    {
        if (timer <= 0)
        {
            TimerCountingDown = false;
            timer = releaseDelay;
        }
    }

}
