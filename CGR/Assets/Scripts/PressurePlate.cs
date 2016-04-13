﻿using UnityEngine;
using System.Collections;
using System;

public class PressurePlate : MonoBehaviour
{

    public bool CanBeUntriggered = false;
    public float ReleaseDelay = 0;
    public Animator anim;
    public GameObject[] list;

    float timer = 0;
    bool timerCountingDown = false;
    bool pressing = false;

    // Use this for initialization
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        timer = ReleaseDelay;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timerCountingDown == true)
            timer -= Time.deltaTime;
        if (CanBeUntriggered == true && !pressing)
            checkIfRelease();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Pushable" || (collider.tag == "Minion" && collider.GetComponent<Minion>().isFollowing == false))
            anim.SetBool("Pressed", true);
        pressing = true;
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Pushable" || (collider.tag == "Minion" && collider.GetComponent<Minion>().isFollowing == false))
        {
            if (CanBeUntriggered == true)
                timerCountingDown = true;
        }
        pressing = false;
    }

    void checkIfRelease()
    {
        if (timer <= 0)
            anim.SetBool("Pressed", false);
    }

    void broadcastDepress()
    {
        timerCountingDown = false;
        timer = ReleaseDelay;
        foreach (GameObject item in list)
            item.SendMessage("plateDepressed");
    }

    void broadcastRelease()
    {       
        timerCountingDown = false;
        foreach (GameObject item in list)
            item.SendMessage("plateReleased");
    }

}
