using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerJump : MonoBehaviour {

    private Rigidbody2D playerBody;
    private bool canJump;
    public int numberOfJumps = 0;
    public bool limitedJumps = false;

    int singlejumpCount = 0;    //used to debug double jumps
    public float jumpForce = 10;

	// Use this for initialization
	void Start () {
        playerBody = GetComponent<Rigidbody2D>();
        canJump = false;
        if (limitedJumps == true)
            foreach (GameObject minion in GameObject.FindGameObjectsWithTag("Minion"))
            {
                numberOfJumps++;
            }          
	}
	
	// Update is called once per frame
    void FixedUpdate()
    {
        if (canJump && numberOfJumps != 0)
        {
            playerBody.AddForce(OrientationListener.instanceOf.getRelativeUpVector() * jumpForce);
            ++singlejumpCount;
            canJump = false;
            numberOfJumps--;
        }
    }

    void jumpCheck(TouchController.SwipeDirection direction)
    {
        if(direction == TouchController.SwipeDirection.UP && !gameObject.GetComponent<Player>().inAir)
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
