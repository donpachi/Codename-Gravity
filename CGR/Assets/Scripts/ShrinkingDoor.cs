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
        PressurePlate.Enter += triggerDoorShrink;
        PressurePlate.Exit += triggerDoorGrow;
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
	void Update () {
        if (shrinking == true)
        {
            shrinkDoor();
            growing = false;
        }
        if (growing == true)
        {
            growDoor();
            shrinking = false;
        }
	}

    void triggerDoorShrink()
    {
        shrinking = true;
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
        Debug.Log("Shrinking: " + transform.localScale);
        transform.localScale = scale;
    }

    void triggerDoorGrow()
    {
        growing = true;
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
        Debug.Log("Growing:" + transform.localScale);
        transform.localScale = scale;
    }
}
