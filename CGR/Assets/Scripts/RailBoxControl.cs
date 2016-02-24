using UnityEngine;
using System.Collections;

public class RailBoxControl : MonoBehaviour {

    //defined in unity
    public float THRUST;
    public float MAXSPEED;

    Rigidbody2D objectRb;
    bool entered;
    bool active;

	// Use this for initialization
	void Start () {
        active = false;
        entered = false;
        objectRb = gameObject.GetComponent<Rigidbody2D>();
	}

    // Update is called once per frame
    void Update()
    {
        if (active && entered && objectRb.velocity.magnitude < MAXSPEED)
            applyMoveForce(THRUST);
    }

    void applyMoveForce(float force)
    {
        TouchController.TouchLocation movementDirection = TouchController.Instance.getTouchDirection();

        switch (movementDirection)
        {
            case TouchController.TouchLocation.LEFT:
                objectRb.AddForce(OrientationListener.instanceOf.getWorldLeftVector() * force, ForceMode2D.Impulse);
                break;
            case TouchController.TouchLocation.RIGHT:
                objectRb.AddForce(OrientationListener.instanceOf.getWorldRightVector() * force, ForceMode2D.Impulse);
                break;
            case TouchController.TouchLocation.NONE:
                break;
        }

    }

    void OnCollisionEnter2D(Collision2D collisionEvent)
    {
        if(collisionEvent.gameObject.name == "Player")
        {
            entered = true;
        }
    }



}
