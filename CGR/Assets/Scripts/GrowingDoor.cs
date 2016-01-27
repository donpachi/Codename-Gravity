using UnityEngine;
using System.Collections;

public class GrowingDoor : MonoBehaviour {

    GameObject parent;
    public float resizespeed = 0.08f;
    bool shrinking = false;
    bool growing = false;
    Vector2 originalScale;
    string originalOrientation;

    // Use this for initialization
    void Start()
    {
        originalScale = transform.localScale;
        //parent = this.GetComponentInParent<Transform>().gameObject;
        parent = this.transform.parent.gameObject;
        if (gameObject.transform.localScale.x > gameObject.transform.localScale.y)
        {
            originalOrientation = "widthwise";
        }
        else
        {
            originalOrientation = "heightwise";
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (shrinking == true && growing == false)
        {
            shrinkDoor();
            growing = false;
        }
        if (shrinking == false && growing == true)
        {
            growDoor();
            shrinking = false;
        }
    }

    void plateDepressed()
    {
        shrinking = false;
        growing = true;
    }

    void shrinkDoor()
    {
        Vector2 scale = transform.localScale;
        if (scale.x <= originalScale.x || scale.y <= originalScale.y)
        {
        //    //this.GetComponent<SpriteRenderer>().enabled = false;
        //    //this.GetComponent<BoxCollider2D>().enabled = false;
            shrinking = false;
            return;
        }
        if (originalOrientation == "widthwise")
        {
            scale = new Vector2(scale.x - resizespeed * 5, scale.y);
        }
        else
        {
            scale = new Vector2(scale.x, scale.y - resizespeed * 5);
        }
        transform.localScale = scale;
    }

    void plateReleased()
    {
        growing = false;
        shrinking = true;
    }

    void growDoor()
    {
        //this.GetComponent<SpriteRenderer>().enabled = true;
        //this.GetComponent<BoxCollider2D>().enabled = true;
        Vector2 scale = transform.localScale;
        Debug.Log("DoorSize " + this.GetComponent<BoxCollider2D>().size.y + " ParentSize " + parent.GetComponent<BoxCollider2D>().size.y);
        if (this.transform.localScale.y >= parent.transform.localScale.y)
        {
            growing = false;
            return;
        }
        if (originalOrientation == "widthwise")
        {
            scale = new Vector2(scale.x + resizespeed * 5, scale.y);
        }
        else
        {
            scale = new Vector2(scale.x, scale.y + resizespeed * 5);
        }
        transform.localScale = scale;
    }
}
