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
        playerBody = player.GetComponent<Rigidbody2D>();
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
        if (enterAble && direction == TouchController.SwipeDirection.UP && anim.GetBool("activated") != true)
        {
            playerBody.GetComponent<Transform>().position = launchPosition.position;
            //player.SetActive(false);
            player.GetComponent<Player>().DeactivateControl(StateChange.CANNON);
            anim.SetBool("activated", true);
        }
    }

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
        player.GetComponent<Player>().ReactivateControl(StateChange.CANNON_FIRE);
        player.transform.position = cannonTip.position;
		direction = (player.transform.position - transform.position).normalized;
        playerBody.AddForce(direction * LAUNCHFORCE, ForceMode2D.Impulse);
        enterAble = false;
        anim.SetBool("activated", false);
	}

    void screenTouched()
    {
        Debug.Log("Screen touched");
        if (cannonReady)
        {
            cannonReady = false;
            FirePlayer();
            
        }
    }

    void resetCannon()
    {
        enterAble = false;
        anim.SetBool("activated", false);
    }

    void OnEnable()
    {
        TouchController.OnTap += screenTouched;
        TouchController.OnSwipe += enterCannon;
        LevelManager.OnCheckpointLoad += resetCannon;
    }

    void OnDisable()
    {
        TouchController.OnTap -= screenTouched;
        TouchController.OnSwipe -= enterCannon;
        LevelManager.OnCheckpointLoad -= resetCannon;
    }
}
