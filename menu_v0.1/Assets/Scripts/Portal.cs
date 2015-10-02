using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Portal : MonoBehaviour {

    public GameObject linkedPortal;
    public float transitionSpeed;

    private List<Node> bodies;
    private float distanceThreshold;
    private Vector3 linkedPortalPosition;

	// Use this for initialization
	void Start () {
        linkedPortalPosition = linkedPortal.transform.position;
        distanceThreshold = 3.1f;
        bodies = new List<Node>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (bodies.Count > 0)
        {
            for (int i = 0; i < bodies.Count; i++)
            {
                bodies[i].body.transform.position = new Vector3(Mathf.Lerp(bodies[i].body.transform.position.x, linkedPortalPosition.x, transitionSpeed * Time.deltaTime),
                                Mathf.Lerp(bodies[i].body.transform.position.y, linkedPortalPosition.y, transitionSpeed * Time.deltaTime),
                                1f);

                if ((linkedPortalPosition - bodies[i].body.transform.position).magnitude <= distanceThreshold)
                {
                    bodies[i].body.Sleep();
                    StartCoroutine(LaunchPlayer(bodies[i].body, bodies[i].velocity, 2f));
                    bodies.RemoveAt(i);
                    i -= 1;
                }
            }
        }
	}

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.tag != "Water")
        {
            Rigidbody2D tempBody = collisionInfo.rigidbody;
            Vector3 hiddenPosition = new Vector3(tempBody.transform.position.x,
                                        tempBody.transform.position.y,
                                        1f);

            tempBody.gravityScale = 0;
            tempBody.Sleep();
            tempBody.transform.position = hiddenPosition;
            tempBody.GetComponent<Collider2D>().enabled = false;
            
            bodies.Add(new Node(true, tempBody, collisionInfo.relativeVelocity));
        }
    }

    IEnumerator LaunchPlayer(Rigidbody2D body, Vector3 velocity, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        float launchAngle = linkedPortal.transform.rotation.eulerAngles.z;
        body.transform.position = new Vector3(body.transform.position.x,
                                           body.transform.position.y,
                                           -2f);
        body.gravityScale = 1;
        body.GetComponent<Collider2D>().enabled = true;
        velocity = Quaternion.AngleAxis(launchAngle, Vector3.forward) * velocity;
        body.velocity = velocity;
    }
}

class Node
{
    public bool inTransition = false;
    public Rigidbody2D body;
    public Vector3 velocity;

    public Node(bool transition, Rigidbody2D rb, Vector3 v)
    {
        inTransition = transition;
        body = rb;
        velocity = v;
    }
}
