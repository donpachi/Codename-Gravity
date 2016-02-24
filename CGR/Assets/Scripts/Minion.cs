using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Minion : MonoBehaviour {

    GameObject player;
    GameObject minionAnchor;
    List<GameObject> minions = new List<GameObject>();
    GameObject parent;
    float timer = 0.1f; 

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

        updateList();
       
	}
	
	// Update is called once per frame
	void FixedUpdate () {
            if (parent == null)
                transform.position = Vector2.Lerp(transform.position, minionAnchor.transform.position, 0.1f);
            else if (Vector2.Distance(transform.position, parent.transform.position) > 0.5f)
                transform.position = Vector2.Lerp(transform.position, parent.transform.position, 0.1f);
	}

    void swipeCheck(TouchController.SwipeDirection direction)
    {
        if (direction == TouchController.SwipeDirection.UP && player.GetComponent<Player>().inMinionArea == true)
        {
            this.GetComponent<Minion>().enabled = false;
            updateList();
            if (minions[0] == gameObject)           
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

    void updateList()
    {
        minions = new List<GameObject>();
        foreach (GameObject minion in GameObject.FindGameObjectsWithTag("Minion"))
        {
            minions.Add(minion);
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), minion.GetComponent<Collider2D>());
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
