using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Minion : MonoBehaviour {

    GameObject player;
    GameObject minionAnchor;
    List<GameObject> minions = new List<GameObject>();
    GameObject parent;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
        minionAnchor = GameObject.Find("MinionAnchor");
        this.GetComponent<Player>().enabled = false;
        this.GetComponent<PlayerJump>().enabled = false;
        this.GetComponent<Walk>().enabled = false;

        foreach (GameObject pushableObject in GameObject.FindGameObjectsWithTag("Pushable"))
        {
            if (pushableObject.GetComponent<Collider2D>())
            {
                foreach (Collider2D collider in pushableObject.GetComponents<Collider2D>())
                    Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collider);
            }
        }

        foreach (GameObject minion in GameObject.FindGameObjectsWithTag("Minion"))
        {
            minions.Add(minion);
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), minion.GetComponent<Collider2D>());
        }

        foreach (GameObject minion in minions)
        {
            GameObject prev = null;
            if (minion == gameObject)
                parent = prev;
            prev = minion;
        }
       
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (parent == null)
            transform.position = Vector2.Lerp(transform.position, minionAnchor.transform.position, 0.1f);
        else
            transform.position = Vector2.Lerp(transform.position, transform.parent.position, 0.1f);
	}

    void swipeCheck(TouchController.SwipeDirection direction)
    {
        if (direction == TouchController.SwipeDirection.UP && player.GetComponent<Player>().inMinionArea == true && gameObject.GetComponentsInChildren<Transform>().Length == 1)
        {
            switchControl();
        }
    }

    void switchControl()
    {
        player.GetComponent<Walk>().enabled = false;
        player.GetComponent<Player>().enabled = false;
        this.GetComponent<Player>().enabled = true;
        this.GetComponent<Rigidbody2D>().gravityScale = 1;
        this.GetComponent<Player>().isMinion = true;
        this.GetComponent<PlayerJump>().enabled = true;
        this.GetComponent<Walk>().enabled = true;
        GameObject.Find("Main Camera").GetComponent<FollowPlayer>().player = this.gameObject;

        this.GetComponent<Minion>().enabled = false;
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
