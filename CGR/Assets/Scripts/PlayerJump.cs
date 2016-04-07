﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerJump : MonoBehaviour {

    private Rigidbody2D playerBody;
    private bool _jumpRequest;
    private bool _jumping;

    public float jumpForce = 10;

    private Animator anim;

	// Use this for initialization
	void Start () {
        playerBody = GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
    void FixedUpdate()
    {
        if (!_jumping && _jumpRequest)
        {
            anim.SetBool("Jumping", true);
            playerBody.AddForce(OrientationListener.instanceOf.getRelativeUpVector() * jumpForce);
            _jumping = true;
        }
    }

    public void JumpFinished()
    {
        _jumping = false;
        anim.SetBool("Jumping", false);
        _jumpRequest = false;
    }

    void jumpCheck(TouchController.SwipeDirection direction)
    {
        if (direction == TouchController.SwipeDirection.UP && !GetComponent<GroundCheck>().InAir)
        {
            _jumpRequest = !GetComponent<GroundCheck>().InAir;
        }
    }

    void jumpCheck()
    {
        if (!GetComponent<GroundCheck>().InAir)
        {
            _jumpRequest = !GetComponent<GroundCheck>().InAir;
        }
    }

    //Event handling for swipe events
    void OnEnable()
    {
        //TouchController.OnSwipe += jumpCheck;
        TouchController.OnTap += jumpCheck;
    }
    void OnDisable()
    {
        //TouchController.OnSwipe -= jumpCheck;
        TouchController.OnTap += jumpCheck;
    }
}
