using UnityEngine;
using System.Collections;

public class ThreeStageDoor : MonoBehaviour {

    bool collapsing = false;
    bool growing = true;
    float timer = 0.66f;
    int currentDoor = 0;

	// Use this for initialization
	void Start () {
        currentDoor = transform.childCount;
        int doorID = 1;
        foreach (DoorID door in this.GetComponentsInChildren<DoorID>())
        {
            door.doorID = doorID;
            doorID++;
        }
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        
        if (collapsing == true && growing == false)
        {
            timer -= Time.deltaTime;
            if (timer % 0.33f == 0 && timer < 0)
            {
                foreach (DoorID door in this.GetComponentsInChildren<DoorID>())
                {
                    if (door.doorID == currentDoor)
                    {
                        door.GetComponent<SpriteRenderer>().enabled = false;
                        door.GetComponent<BoxCollider2D>().enabled = false;
                    }
                }
                currentDoor--;
            }
            else if (timer >= 0)
            {
                this.GetComponent<SpriteRenderer>().enabled = false;
                this.GetComponent<BoxCollider2D>().enabled = false;
                collapsing = false;
            }
        }
        else if (collapsing == false && growing == true)
        {
            timer += Time.deltaTime;
            if (timer == 0.33f)
            {
                foreach (DoorID door in this.GetComponentsInChildren<DoorID>())
                {
                    if (door.doorID == currentDoor)
                    {
                        door.GetComponent<SpriteRenderer>().enabled = false;
                        door.GetComponent<BoxCollider2D>().enabled = false;
                    }
                }
                currentDoor++;
            }
            else if (timer == 0)
            {
                this.GetComponent<SpriteRenderer>().enabled = true;
                this.GetComponent<BoxCollider2D>().enabled = true;
            }
            else if (timer == 0.66f)
            {
                foreach (DoorID door in this.GetComponentsInChildren<DoorID>())
                {
                    if (door.doorID == currentDoor)
                    {
                        door.GetComponent<SpriteRenderer>().enabled = false;
                        door.GetComponent<BoxCollider2D>().enabled = false;
                    }
                }
                currentDoor++;
                growing = false;
            }
        }
	}

    void plateDepressed()
    {
        timer = 0.25f;
        collapsing = true;
        growing = false;
        
    }

    void plateReleased()
    {
        timer = 0.25f;
        collapsing = false;
        growing = true;
    }
}
