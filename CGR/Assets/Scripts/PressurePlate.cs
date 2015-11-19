using UnityEngine;
using System.Collections;

public class PressurePlate : MonoBehaviour {

    bool resizingDoor = false;
    float RESIZERATE = 0.08f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (resizingDoor == true)
            resizeDoor();
	}

    void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.GetComponent<Rigidbody2D>().mass >= 1) 
        {
            
            // Door opens
            foreach (Transform transform in this.GetComponentsInChildren<Transform>()) 
            {
                if (transform.gameObject.name == "ShrinkingDoor")
                {
                    
                    resizingDoor = true;
                }
            }
           
        }
    }

    void resizeDoor()
    {
        // Door opens
        foreach (Transform transform in this.GetComponentsInChildren<Transform>())
        {
            if (transform.gameObject.name == "ShrinkingDoor")
            {
                Vector2 scale = transform.gameObject.transform.localScale;
                if (scale.x <= 0 || scale.y <= 0)
                {
                    Destroy(transform.gameObject);
                }
                if (Mathf.Abs(gameObject.GetComponent<SpriteRenderer>().bounds.max.x) - Mathf.Abs(gameObject.GetComponent<SpriteRenderer>().bounds.min.x) > Mathf.Abs(gameObject.GetComponent<SpriteRenderer>().bounds.max.y) - Mathf.Abs(gameObject.GetComponent<SpriteRenderer>().bounds.min.y))
                {
                    scale = new Vector2(scale.x, scale.y - RESIZERATE);
                    transform.gameObject.transform.position = new Vector2(transform.gameObject.transform.position.x, transform.gameObject.transform.position.y - RESIZERATE);
                }
                else
                {
                    scale = new Vector2(scale.x - RESIZERATE, scale.y);
                    transform.gameObject.transform.position = new Vector2(transform.gameObject.transform.position.x - RESIZERATE, transform.gameObject.transform.position.y);
                }
                transform.gameObject.transform.localScale = scale;
            }
        }
        if (this.GetComponentsInChildren<Transform>().Length == 0)
            resizingDoor = false;
    }

}
