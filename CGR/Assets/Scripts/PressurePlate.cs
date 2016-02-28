using UnityEngine;
using System.Collections;
using System;

public class PressurePlate : MonoBehaviour {

    public bool canBeUntriggered = false;
    public float releaseDelay = 0;
    float timer = 0;
    bool TimerCountingDown = false;
    public Animator anim;

	// Use this for initialization
	void Start () {
        timer = releaseDelay;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (TimerCountingDown == true)
            timer -= Time.deltaTime;
        if (canBeUntriggered == true)
            checkIfRelease();
	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Pushable" || (collider.tag == "Minion" && collider.GetComponent<Minion>().isFollowing == false))
        {
            anim.SetInteger("State", 1);         
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Pushable" || (collider.tag == "Minion" && collider.GetComponent<Minion>().isFollowing == false))
        {
            if (canBeUntriggered == true)
                TimerCountingDown = true;
        }
    }

    void checkIfRelease()
    {
        if (timer <= 0)
        {
            anim.SetInteger("State", 2);
        }
    }

    void broadcastDepress()
    {
        TimerCountingDown = false;
        timer = releaseDelay;
        BroadcastMessage("plateDepressed");
        anim.SetInteger("State", 0);
        Debug.Log("adsf");
    }

    void broadcastRelease()
    {
        anim.SetInteger("State", 0);
        TimerCountingDown = false;
        BroadcastMessage("plateReleased");
        timer = releaseDelay;
    }

}
