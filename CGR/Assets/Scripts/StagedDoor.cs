using UnityEngine;
using System.Collections;

public class StagedDoor : MonoBehaviour
{
    public bool inverted;
    public float doorSpeed = 0.1f;
    private bool collapsing = false;
    private bool growing = false;
    private float timer = 0;
    int currentDoor = 0;
    public bool isActive = false;

    // Use this for initialization
    void Start()
    {
        timer = doorSpeed;
        if (inverted)
            collapseDoor();
        foreach (Transform child in transform)
            child.GetComponent<StagedDoor>().doorSpeed = this.doorSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (collapsing == true && growing == false)
            collapseDoor();
        else if (collapsing == false && growing == true)
            growDoor();
    }

    void collapseDoor()
    {
        Transform parent = this.transform.parent;
        if (parent == null || !parent.name.Contains("StagedDoor") && this.GetComponent<SpriteRenderer>().enabled == true)
        {
            this.GetComponent<SpriteRenderer>().enabled = false;
            this.GetComponent<BoxCollider2D>().enabled = false;
            foreach (Transform child in transform)
            {
                if (child.parent == transform && child.gameObject != gameObject)
                {
                    child.GetComponent<StagedDoor>().isActive = true;
                    child.gameObject.GetComponent<StagedDoor>().collapsing = true;
                    child.gameObject.GetComponent<StagedDoor>().growing = false;
                }
            }
            collapsing = false;
            isActive = false;
        }

        else if (isActive == true)
        {
            timer -= Time.deltaTime;
            growing = false;
            collapsing = true;
            if (timer <= 0)
            {
                foreach (Transform child in transform)
                {
                    if (child != null && child.parent == transform)
                    {
                        child.gameObject.GetComponent<StagedDoor>().isActive = true;
                        child.gameObject.GetComponent<StagedDoor>().collapsing = true;
                        child.gameObject.GetComponent<StagedDoor>().growing = false;
                    }
                }
                this.GetComponent<SpriteRenderer>().enabled = false;
                this.GetComponent<BoxCollider2D>().enabled = false;
                collapsing = false;
                isActive = false;
                timer = doorSpeed;
            }
        }
    }

    void growDoor()
    {
        Transform parent = this.transform.parent;

        if (this.GetComponent<SpriteRenderer>().enabled == false)
        {
            foreach (Transform child in transform)
            {
                child.GetComponent<StagedDoor>().collapsing = false;
                child.GetComponent<StagedDoor>().growing = true;
                if (child.childCount == 0)
                {
                    child.GetComponent<StagedDoor>().isActive = true;
                    //child.GetComponent<StagedDoor>().timer = 0;
                }
            }
        }


        if (isActive == true)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                if (transform.parent != null && transform.parent.name.Contains("StagedDoor"))
                    transform.parent.GetComponent<StagedDoor>().isActive = true;
                timer = doorSpeed;
                this.GetComponent<SpriteRenderer>().enabled = true;
                this.GetComponent<BoxCollider2D>().enabled = true;
                collapsing = false;
                growing = false;
                isActive = false;
            }
        }
    }

    void plateDepressed()
    {
        //timer = doorSpeed;
        if (inverted) {
            collapsing = false;
            growing = true;
        }
        else {
            collapsing = true;
            growing = false;
        }

    }

    void plateReleased()
    {
        //timer = doorSpeed;

        if (inverted) {
            collapsing = true;
            growing = false;
        }
        else {
            collapsing = false;
            growing = true;
        }
    }
}
