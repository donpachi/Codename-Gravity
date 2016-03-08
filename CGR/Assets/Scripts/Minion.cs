using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Minion : MonoBehaviour {

    public float MinionDistance;
    public float MinionFollowSpeed;

    GameObject player;
    GameObject minionAnchor;
    List<GameObject> minions = new List<GameObject>();
    GameObject parent;
    float timer = 0.1f;
    public bool isFollowing = true;
    float playerPosDiff;
    Vector2 prevPlayerLocation;
    private Animator anim;

	// Use this for initialization
	void Start () {
        anim = gameObject.GetComponent<Animator>();
        player = GameObject.Find("Player");
        minionAnchor = GameObject.Find("MinionAnchor");
        this.GetComponent<Player>().enabled = false;
        this.GetComponent<PlayerJump>().enabled = false;
        this.GetComponent<Walk>().enabled = false;

        updateList();
       
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        RaycastHit2D groundCheckRay = Physics2D.Raycast(transform.position, OrientationListener.instanceOf.getWorldDownVector(), 0.5f);
        if (player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle") && checkIfSameHeight() && groundCheckRay.collider != null && groundCheckRay.collider.name.Contains("MovingPlatform"))
        {
            if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.PORTRAIT || OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.INVERTED_PORTRAIT)
            {
                playerPosDiff = Mathf.Abs(prevPlayerLocation.x - player.transform.position.x);
                if (prevPlayerLocation.x > player.transform.position.x)
                    transform.position = new Vector2(transform.position.x - playerPosDiff, transform.position.y);
                else
                    transform.position = new Vector2(transform.position.x + playerPosDiff, transform.position.y);
            }
            else
            {
                playerPosDiff = Mathf.Abs(prevPlayerLocation.y - player.transform.position.y);
                if (prevPlayerLocation.y > player.transform.position.y)
                    transform.position = new Vector2(transform.position.x, transform.position.y + playerPosDiff);
                else
                    transform.position = new Vector2(transform.position.x, transform.position.y - playerPosDiff);
            }
        }
        else
            lerpToPlayer();

        checkGravityScale();
        prevPlayerLocation = player.transform.position;
	}

    public void SetParent(GameObject parentObj)
    {
        parent = parentObj;
    }

    void checkGravityScale()
    {
        if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.PORTRAIT && transform.position.y > player.GetComponent<Player>().getPlayerFeet())
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
        else if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.PORTRAIT && transform.position.y <= player.GetComponent<Player>().getPlayerFeet())
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        else if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.INVERTED_PORTRAIT && transform.position.y > player.GetComponent<Player>().getPlayerFeet())
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        else if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.INVERTED_PORTRAIT && transform.position.y <= player.GetComponent<Player>().getPlayerFeet())
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;

        else if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.LANDSCAPE_LEFT && transform.position.x > player.GetComponent<Player>().getPlayerFeet())
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
        else if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.LANDSCAPE_LEFT && transform.position.x <= player.GetComponent<Player>().getPlayerFeet())
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        else if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.LANDSCAPE_RIGHT && transform.position.x > player.GetComponent<Player>().getPlayerFeet())
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        else if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.LANDSCAPE_RIGHT && transform.position.x <= player.GetComponent<Player>().getPlayerFeet())
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
    }

    bool checkIfSameHeight()
    {
        if ((OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.PORTRAIT || OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.INVERTED_PORTRAIT) && Mathf.Abs((player.transform.position.y - 0.25f) - transform.position.y) > 0.01f)
            return false;
        if ((OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.LANDSCAPE_LEFT || OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.LANDSCAPE_RIGHT) && Mathf.Abs((player.transform.position.y - 0.25f) - transform.position.y) > 0.01f)
            return false;
        return true;
    }

    void lerpToPlayer()
    {
        if (parent == null)
        {
            if (Vector2.Distance(transform.position, player.GetComponent<Player>().getPlayerFeetVector()) > MinionDistance)
            {
                transform.position = Vector2.Lerp(transform.position, player.GetComponent<Player>().getPlayerFeetVector(), MinionFollowSpeed);
                anim.SetBool("Moving", true);
            }
            else
                anim.SetBool("Moving", false);

        }
        else if (Vector2.Distance(transform.position, parent.transform.position) > MinionDistance)
        {
            anim.SetBool("Moving", true);
            transform.position = Vector2.Lerp(transform.position, parent.transform.position, MinionFollowSpeed);
        }
        else
            anim.SetBool("Moving", false);
            
    }

    void swipeCheck(TouchController.SwipeDirection direction)
    {
        if (direction == TouchController.SwipeDirection.UP && player.GetComponent<Player>().inMinionArea == true)
        {
            this.GetComponent<Minion>().enabled = false;
            updateList();
            if (minions[0] == gameObject)
                anim.SetBool("SwitchingToMinion", true);
        }
    }

    void switchControlToPlayer()
    {
        Debug.Log("asdfkljtr");
        player.GetComponent<Walk>().enabled = false;
        player.GetComponent<Player>().enabled = false;
        player.GetComponent<Rigidbody2D>().isKinematic = false;
        this.GetComponent<Player>().enabled = true;
        this.GetComponent<Rigidbody2D>().gravityScale = 1;
        this.GetComponent<Player>().isMinion = true;
        this.GetComponent<PlayerJump>().enabled = true;
        this.GetComponent<Walk>().enabled = true;
        GameObject.Find("Main Camera").GetComponent<FollowPlayer>().player = this.gameObject;
        isFollowing = false;
        this.GetComponent<Minion>().enabled = false;
        anim.SetBool("SwitchingToMinion", false);
    }

    void updateList()
    {
        minions = new List<GameObject>();
        foreach (GameObject minion in GameObject.FindGameObjectsWithTag("Minion"))
        {
            minions.Add(minion);
        }
        GameObject prev = null;
        foreach (GameObject minion in minions)
        {
            
            if (minion == gameObject)
                parent = prev;
            prev = minion;
        }
    }

    //Event handling for swipe events
    void OnEnable()
    {
        TouchController.OnSwipe += swipeCheck;
    }
    void OnDisable()
    {
        TouchController.OnSwipe -= swipeCheck;
    }


}
