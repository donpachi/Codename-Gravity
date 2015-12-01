using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Portal : MonoBehaviour {

    public GameObject linkedPortal;
    public float transitionSpeed;

    private List<Node> bodies;
    private float distanceThreshold;
    private Vector3 linkedPortalPosition;
    private GameObject playerStatus;
    private bool playerComing;

	// Use this for initialization
	void Start () {
        linkedPortalPosition = linkedPortal.transform.position;
        playerStatus = GameObject.Find("Player");
        distanceThreshold = 0.1f;
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
                                0f);

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
			bool isPlayer = collisionInfo.gameObject.name.Equals("Player");
			if (isPlayer && playerComing)
				return;

            Rigidbody2D tempBody = collisionInfo.rigidbody;
            Collider2D[] colliders = tempBody.GetComponents<Collider2D>();

            if (isPlayer) {
				playerStatus.GetComponent<Player>().InTransitionStatusOn();
                linkedPortal.GetComponent<Portal>().PlayerComing();
				playerStatus.GetComponent<Player>().ToggleRender();
				playerStatus.GetComponent<Walk>().enabled = false;
				playerStatus.GetComponent<SuctionWalk>().enabled = false;
            }
            tempBody.gravityScale = 0;
            tempBody.velocity = new Vector2(0, 0);
            tempBody.Sleep();

            for (int i = 0; i < colliders.Length; i++)
                colliders[i].enabled = false;

            bodies.Add(new Node(tempBody, tempBody.transform.TransformVector(collisionInfo.relativeVelocity), isPlayer));
        }
    }

    void OnTriggerExit2D(Collider2D collisionInfo)
    {
        bool isPlayer = collisionInfo.gameObject.name.Equals("Player");
        if (isPlayer)
            playerComing = false;
    }

    IEnumerator LaunchPlayer(Node entity, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        Collider2D[] colliders = entity.body.GetComponents<Collider2D>();
        float launchAngle = Mathf.Abs(this.transform.rotation.eulerAngles.z - linkedPortal.transform.rotation.eulerAngles.z);
        entity.body.transform.position = new Vector3(entity.body.transform.position.x,
                                           entity.body.transform.position.y,
                                           -2f);
		
        entity.velocity = Quaternion.AngleAxis(launchAngle, Vector3.forward) * entity.velocity;
        entity.body.AddForce(entity.velocity * entity.velocity.magnitude * 5);
		entity.body.gravityScale = 1;

        for (int i = 0; i < colliders.Length; i++)
            colliders[i].enabled = true;

		if (entity.isPlayer) {
			playerStatus.GetComponent<Player>().ToggleRender();
			playerStatus.GetComponent<Player>().InTransitionStatusEnd(); 

			if (playerStatus.GetComponent<Player>().IsSuctioned())
				playerStatus.GetComponent<SuctionWalk>().enabled = true;
			else
				playerStatus.GetComponent<Walk>().enabled = true;
		}
    }

    public void PlayerComing()
    {
        playerComing = true;
    }
}

class Node
{
    public Rigidbody2D body;
    public Vector2 velocity;
	public bool isPlayer;

    public Node(Rigidbody2D rb, Vector2 v, bool p) {
        body = rb;
        velocity = v;
		isPlayer = p;
    }
}
