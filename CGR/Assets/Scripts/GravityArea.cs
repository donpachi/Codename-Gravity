using UnityEngine;
using System.Collections;

public class GravityArea : MonoBehaviour {

    public bool EffectOn;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {

    }

    void plateDepressed()
    {
        EffectOn = !EffectOn;
    }

    void plateReleased()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!EffectOn)
            return;
        if(collision.gameObject.GetComponent<Rigidbody2D>() != null)
        {
            collision.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (!EffectOn)
        {
            if (collision.gameObject.GetComponent<Rigidbody2D>() != null)
            {
                collision.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody2D>() != null)
        {
            collision.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
        }
    }

}
