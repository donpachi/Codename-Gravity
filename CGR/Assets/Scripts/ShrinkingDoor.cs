using UnityEngine;
using System.Collections;
using System;


public class ShrinkingDoor : MonoBehaviour {

    GameObject parent;
    public float resizespeed = 0.08f;
    bool shrinking = false;
    bool growing = false;
    Vector2 originalScale;
    string originalOrientation;

	// Use this for initialization
	void Start () {
        originalScale = transform.localScale;
        parent = this.GetComponentInParent<Transform>().gameObject;
        if (Mathf.Abs(gameObject.GetComponent<SpriteRenderer>().bounds.max.x) - Mathf.Abs(gameObject.GetComponent<SpriteRenderer>().bounds.min.x) > Mathf.Abs(gameObject.GetComponent<SpriteRenderer>().bounds.max.y) - Mathf.Abs(gameObject.GetComponent<SpriteRenderer>().bounds.min.y))
        {
            originalOrientation = "widthwise";
        }
        else
        {
            originalOrientation = "heightwise";
        }
	}
	
	// Update is called once per frame
	void FixedUpdate () {
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
        shrinking = true;
        growing = false;
    }

    void shrinkDoor()
    {
        Vector2 scale = transform.localScale;
        if (scale.x <= 0.1 || scale.y <= 0.1)
        {
            shrinking = false;
            return;
        }
        if (originalOrientation == "widthwise")
        {
            scale = new Vector2(scale.x - resizespeed, scale.y);
            transform.position = new Vector2(transform.gameObject.transform.position.x - resizespeed, transform.gameObject.transform.position.y);
        }
        else
        {
            scale = new Vector2(scale.x, scale.y - resizespeed);
            transform.position = new Vector2(transform.gameObject.transform.position.x, transform.gameObject.transform.position.y - resizespeed);
        }
        transform.localScale = scale;
    }

    void plateReleased()
    {
        growing = true;
        shrinking = false;
    }

    void growDoor()
    {
        Vector2 scale = transform.localScale;
        if (scale.x >= originalScale.x && scale.y >= originalScale.y)
        {
            growing = false;
            return;
        }
        if (originalOrientation == "widthwise")
        {
            scale = new Vector2(scale.x + resizespeed, scale.y);
            transform.position = new Vector2(transform.gameObject.transform.position.x + resizespeed, transform.gameObject.transform.position.y);
        }
        else
        {
            scale = new Vector2(scale.x, scale.y + resizespeed);
            transform.position = new Vector2(transform.gameObject.transform.position.x, transform.gameObject.transform.position.y + resizespeed);
        }
        transform.localScale = scale;
    }
}
