using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerJump : MonoBehaviour {

    private Rigidbody2D playerBody;
    private bool _jumpRequest;
    private bool _jumping;
    private GroundCheck gCheck;

    public float jumpForce = 10;

    private Animator anim;

	// Use this for initialization
	void Start () {
        playerBody = GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        gCheck = GetComponent<GroundCheck>();
	}
	
	// Update is called once per frame
    void FixedUpdate()
    {
        if (!_jumping && _jumpRequest)
        {
            anim.SetBool("Jumping", true);
            _jumpRequest = false;
            _jumping = true;
        }
    }

    public void JumpFinished()
    {
        _jumping = false;
        anim.SetBool("Jumping", false);
    }

    void jump()
    {
        playerBody.AddForce(OrientationListener.instanceOf.getRelativeUpVector() * jumpForce);
        playerBody.AddForce(playerBody.transform.forward * 1.0f);
    }

    void jumpCheck()
    {
        Debug.Log("Tap happened");
        if (!gCheck.InAir)
        {
            _jumpRequest = true;
        }
    }

    //Event handling for swipe events
    void OnEnable()
    {
        TouchController.OnTap += jumpCheck;
    }
    void OnDisable()
    {
        TouchController.OnTap -= jumpCheck;
    }
}
