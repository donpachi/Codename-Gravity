using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerJump : MonoBehaviour {

    private Rigidbody2D playerBody;
    private bool canJump;

    int singlejumpCount = 0;    //used to debug double jumps
    public float jumpForce = 10;

    private Animator anim;

	// Use this for initialization
	void Start () {
        playerBody = GetComponent<Rigidbody2D>();
        canJump = false;
        anim = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
    void FixedUpdate()
    {
        if (canJump)
        {
            anim.SetBool("Jumping", true);
            playerBody.AddForce(OrientationListener.instanceOf.getRelativeUpVector() * jumpForce);
            ++singlejumpCount;
            canJump = false;
            anim.SetBool("Jumping", false);
        }
    }

    void jumpCheck(TouchController.SwipeDirection direction)
    {

        
        if (direction == TouchController.SwipeDirection.UP && !gameObject.GetComponent<Player>().inAir)
        {
            canJump = true;
        }
    }

    //Event handling for swipe events
    void OnEnable()
    {
        TouchController.OnSwipe += jumpCheck;
    }
    void OnDisable()
    {
        TouchController.OnSwipe -= jumpCheck;
    }
}
