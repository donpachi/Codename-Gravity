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
            playerBody.AddForce(OrientationListener.instanceOf.getRelativeUpVector() * jumpForce);
            _jumping = true;
            _jumpRequest = false;
        }
        else if (_jumping && !gCheck.InAir)
            JumpFinished();
    }

    public void JumpFinished()
    {
        _jumping = false;
        anim.SetBool("Jumping", false);
    }

    void jumpCheck()
    {
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
