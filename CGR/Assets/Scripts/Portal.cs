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
        distanceThreshold = 1.1f;
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
                    StartCoroutine(LaunchPlayer(bodies[i], 2f));
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
            Collider2D[] colliders = tempBody.GetComponents<Collider2D>();
            bool isPlayer = collisionInfo.gameObject.tag.Equals("Player");

            tempBody.gravityScale = 0;
            tempBody.Sleep();
            GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<Player>().ToggleRender();
            GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<Walk>().enabled = false;

            if (isPlayer)
                GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<Player>().InTransitionStatusOn(); 
            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].enabled = false;
            }
            bodies.Add(new Node(tempBody, collisionInfo.relativeVelocity));
        }
    }

    IEnumerator LaunchPlayer(Node entity, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        Collider2D[] colliders = entity.body.GetComponents<Collider2D>();
        float launchAngle = linkedPortal.transform.rotation.eulerAngles.z;
        entity.body.transform.position = new Vector3(entity.body.transform.position.x,
                                           entity.body.transform.position.y,
                                           -2f);
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = true;
        }

        entity.body.gravityScale = 1;
        entity.velocity = Quaternion.AngleAxis(launchAngle, Vector3.forward) * entity.velocity;
        entity.body.velocity = entity.velocity;
        GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<Player>().ToggleRender();
        GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<Walk>().enabled = true;
        GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<Player>().InTransitionStatusEnd(); 
    }
}

class Node
{
    public Rigidbody2D body;
    public Vector3 velocity;

    public Node(Rigidbody2D rb, Vector3 v)
    {
        body = rb;
        velocity = v;
    }
}
