using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GravityCannon : MonoBehaviour {

	private Animator anim;

    private Transform cannonTip;
    private Transform launchPosition;
    private GameObject player;
    private Rigidbody2D playerBody;
    private float LAUNCHFORCE = 5.0f;
    private bool cannonReady = false;
    private bool enterAble = false;
	
	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player");
        Transform[] components = this.GetComponentsInChildren<Transform>();
        foreach (var i in components)
        {
            //print(i.position);
            if (i.name == "CannonTip")
                cannonTip = i;
            else if (i.name == "LaunchPosition")
                launchPosition = i;
        }

        anim = GetComponentInParent<Animator>();
	}
	
	// Update is called once per frame
	void Update() {
    }

    void enterCannon(TouchController.SwipeDirection direction)
    {
        if (enterAble && direction == TouchController.SwipeDirection.UP)
        {
            player.GetComponent<Walk>().enabled = false;
            playerBody = player.GetComponent<Rigidbody2D>();
            playerBody.gravityScale = 0f;
            playerBody.Sleep();
            playerBody.GetComponent<Collider2D>().enabled = false;
            playerBody.GetComponent<Transform>().position = launchPosition.position;
            player.GetComponent<Player>().ToggleRender();

            anim.SetBool("activated", true);
        }
    }

	//void OnCollisionEnter2D(Collision2D collisionInfo) {
	//	if (collisionInfo.gameObject.name == "Player") {
 //           player.GetComponent<Walk>().enabled = false;
 //           playerBody = collisionInfo.rigidbody;
 //           playerBody.gravityScale = 0f;
 //           playerBody.Sleep();
 //           playerBody.GetComponent<Collider2D>().enabled = false;
 //           playerBody.GetComponent<Transform>().position = launchPosition.position;
 //           player.GetComponent<Player>().ToggleRender();

 //           anim.SetBool("activated", true);
 //           enterAble = true;
 //       }
	//}

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.name == "Player")
        {
            enterAble = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.name == "Player")
        {
            enterAble = false;
        }
    }

    public void EnableFireButton()
    {
        cannonReady = true;
    }

	public void FirePlayer() {
        Vector2 direction;
        player.GetComponent<Player>().ToggleRender();
        playerBody.GetComponent<Transform>().position = cannonTip.position;
        playerBody.GetComponent<Collider2D>().enabled = true;
        player.GetComponent<Player>().LaunchStatusOn();
		direction = (player.GetComponent<Transform>().position - this.GetComponent<Transform>().position).normalized;
        playerBody.AddForce(direction * LAUNCHFORCE, ForceMode2D.Impulse);
        playerBody.angularDrag = 0;
        playerBody.drag = 0;
        enterAble = false;
        anim.SetBool("activated", false);
	}

    void screenTouched(TouchInstanceData data)
    {
        if (cannonReady && data.touchLocation != TouchController.TouchLocation.NONE)
        {
            cannonReady = false;
            FirePlayer();
        }
    }

    void OnEnable()
    {
        TouchController.ScreenTouched += screenTouched;
        TouchController.OnSwipe += enterCannon;
    }

    void OnDisable()
    {
        TouchController.ScreenTouched -= screenTouched;
        TouchController.OnSwipe -= enterCannon;
    }
}
