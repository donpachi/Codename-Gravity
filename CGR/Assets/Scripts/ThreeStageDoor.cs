using UnityEngine;
using System.Collections;

public class ThreeStageDoor : MonoBehaviour {

    bool collapsing = false;
    bool growing = false;
    float timer = 0.2f;
    int currentDoor = 0;
    public bool isActive = false;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if (collapsing == true && growing == false)
        {
            GameObject parent = this.transform.parent.gameObject;
            
            if (transform.childCount == 0)
            {
                parent.GetComponent<ThreeStageDoor>().isActive = true;
                this.GetComponent<SpriteRenderer>().enabled = false;
                this.GetComponent<BoxCollider2D>().enabled = false;
            }
            
            else if (isActive == true)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    if (parent.GetComponent<ThreeStageDoor>())
                        parent.GetComponent<ThreeStageDoor>().isActive = true;
                    this.GetComponentInChildren<SpriteRenderer>().enabled = false;
                    this.GetComponentInChildren<SpriteRenderer>().enabled = false;
                }
            }
        }
        else if (collapsing == false && growing == true)
        {

        }
       
        
	}

    void plateDepressed()
    {
        collapsing = true;
        growing = false;
        
    }

    void plateReleased()
    {
        collapsing = false;
        growing = true;
    }
}
