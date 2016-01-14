using UnityEngine;
using System.Collections;

public class ThreeStageDoor : MonoBehaviour {

    bool collapsing = false;
    bool growing = false;
    public float timer = 0.2f;
    int currentDoor = 0;
    public bool isActive = false;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        GameObject parent = this.transform.parent.gameObject;
        if (collapsing == true && growing == false)
        {      
            if (transform.childCount == 0 && this.GetComponent<SpriteRenderer>().enabled == true)
            {
                this.GetComponent<ThreeStageDoor>().isActive = true;
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
                    isActive = false;
                    timer = 0.2f;
                    this.GetComponent<SpriteRenderer>().enabled = false;
                    this.GetComponent<BoxCollider2D>().enabled = false;
                }
            }
        }
        else if (collapsing == false && growing == true)
        {
            if (!parent.GetComponent<ThreeStageDoor>() && this.GetComponent<SpriteRenderer>().enabled == false)
            {
                this.GetComponent<ThreeStageDoor>().isActive = true;
                this.transform.GetChild(0).GetComponent<ThreeStageDoor>().isActive = true;
                this.GetComponent<SpriteRenderer>().enabled = true;
                this.GetComponent<BoxCollider2D>().enabled = true;
            }
            else if (isActive == true)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    if (this.transform.childCount != 0)
                        this.transform.GetChild(0).GetComponent<ThreeStageDoor>().isActive = true;
                    isActive = false;
                    timer = 0.2f;
                    this.GetComponent<SpriteRenderer>().enabled = true;
                    this.GetComponent<BoxCollider2D>().enabled = true;
                }
            }
        }
       
        
	}

    void plateDepressed()
    {
        timer = 0.2f;
        collapsing = true;
        growing = false;
        
    }

    void plateReleased()
    {
        timer = 0.2f;
        collapsing = false;
        growing = true;
    }
}
