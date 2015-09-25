using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour {

    public GameObject linkedPortal;
    public float transitionSpeed;
    public float distanceThreshold;

    private Rigidbody2D playerBody;
    private Vector3 playerVelocity;
    private Vector3 linkedPortalPosition;
    private float distanceToPortal;
    private bool inTransition = false;

	// Use this for initialization
	void Start () {
        linkedPortalPosition = linkedPortal.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (inTransition)
        {
            distanceToPortal = (linkedPortalPosition - playerBody.transform.position).magnitude;
            if (distanceToPortal <= distanceThreshold)
            {
                playerBody.transform.position = new Vector3(    linkedPortalPosition.x,
                                                                linkedPortalPosition.y,
                                                                -2);
                playerBody.Sleep();
                inTransition = false;
                Invoke("LaunchPlayer", 2);
            }
        }
	}

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.tag != "Water")
        {
            playerBody = collisionInfo.rigidbody;
            playerVelocity = collisionInfo.relativeVelocity;

            Vector3 hiddenPosition = new Vector3(playerBody.transform.position.x,
                                        playerBody.transform.position.y,
                                        1);
            
            playerBody.gravityScale = 0;
            playerBody.Sleep();
            playerBody.transform.position = hiddenPosition;
            Transition();
        }
    }

    void Transition()
    {
        Vector3 direction = linkedPortalPosition - playerBody.transform.position;
        
        inTransition = true;
        playerBody.GetComponent<Collider2D>().enabled = false;
        playerBody.AddForce(direction.normalized * direction.magnitude * transitionSpeed);
    }

    void LaunchPlayer()
    {
        float launchAngle = linkedPortal.transform.rotation.eulerAngles.z;
        playerBody.gravityScale = 1;
        playerBody.GetComponent<Collider2D>().enabled = true;
        playerVelocity = Quaternion.AngleAxis(launchAngle, Vector3.forward) * playerVelocity;
        playerBody.velocity = playerVelocity;
    }
}
