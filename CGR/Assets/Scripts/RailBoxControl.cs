using UnityEngine;
using System.Collections;

/// <summary>
/// Script that defines the Box movement behaviour.
/// </summary>
public class RailBoxControl : MonoBehaviour {

    //defined in unity
    public float THRUST;
    public float MAXSPEED;

    Rigidbody2D objectRb;
    bool hasEntered;

	// Use this for initialization
	void Start () {
        hasEntered = false;
        objectRb = gameObject.GetComponent<Rigidbody2D>();
	}

    // Update is called once per frame
    void FixedUpdate()
    {
        if (hasEntered)
        {
            if (objectRb.velocity.magnitude < MAXSPEED)
                applyMoveForce(THRUST);
        }
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
            hasEntered = true;
        }
    }



}
