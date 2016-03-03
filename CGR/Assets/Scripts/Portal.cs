using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Portal : MonoBehaviour {

    public GameObject linkedPortal;
    public float transitionSpeed;
    public Animator anim;

    private List<Node> bodies;
    private Queue<Rigidbody2D> waitForLeave;
    private float distanceThreshold;
    private GameObject playerStatus;
    private bool playerComing;



	// Use this for initialization
	void Start () {
        anim = gameObject.GetComponent<Animator>();
        playerStatus = GameObject.Find("Player");
        distanceThreshold = 0.1f;
        bodies = new List<Node>();
        waitForLeave = new Queue<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (bodies.Count > 0)
        {
            for (int i = 0; i < bodies.Count; i++)
            {
                bodies[i].body.transform.position = new Vector3(Mathf.Lerp(bodies[i].body.transform.position.x, this.transform.position.x, transitionSpeed * Time.deltaTime),
                                Mathf.Lerp(bodies[i].body.transform.position.y, this.transform.position.y, transitionSpeed * Time.deltaTime),
                                0f);

                if ((this.transform.position - bodies[i].body.transform.position).magnitude <= distanceThreshold)
                {
                    bodies[i].body.Sleep();
                    StartCoroutine(LaunchPlayer(bodies[i], 2f));
                    waitForLeave.Enqueue(bodies[i].body);
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
            bool isPlayer = collisionInfo.gameObject.name.Equals("Player");

            if (waitForLeave.Contains(tempBody))
                return ;

            if (isPlayer) {
				playerStatus.GetComponent<Player>().InTransitionStatusOn();
				playerStatus.GetComponent<Player>().ToggleRender();
				playerStatus.GetComponent<Walk>().enabled = false;
				playerStatus.GetComponent<SuctionWalk>().enabled = false;
            }
            else tempBody.GetComponent<Renderer>().enabled = false;
            tempBody.gravityScale = 0;
            tempBody.Sleep();

            for (int i = 0; i < colliders.Length; i++)
                colliders[i].enabled = false;

            linkedPortal.GetComponent<Portal>().SendObject(tempBody, collisionInfo.relativeVelocity, this.transform.rotation.eulerAngles.z, isPlayer);
        }
    }

    void OnTriggerExit2D(Collider2D collisionInfo)
    {
        if (waitForLeave.Count > 0)
            waitForLeave.Dequeue();
    }

    public void SendObject(Rigidbody2D body, Vector2 velocity, float angle, bool isPlayer)
    {
        bodies.Add(new Node(body, velocity, angle, isPlayer));
    }

    IEnumerator LaunchPlayer(Node entity, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        Collider2D[] colliders = entity.body.GetComponents<Collider2D>();
        float launchAngle = this.transform.rotation.eulerAngles.z - entity.angle;
        entity.body.transform.position = new Vector3(entity.body.transform.position.x,
                                           entity.body.transform.position.y,
                                           -2f);
		
        entity.velocity = Quaternion.AngleAxis(launchAngle, Vector3.forward) * entity.velocity;
        entity.body.velocity = entity.velocity;
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

        else entity.body.GetComponent<Renderer>().enabled = true;
    }
}

class Node
{
    public Rigidbody2D body;
    public Vector2 velocity;
    public float angle;
	public bool isPlayer;

    public Node(Rigidbody2D rb, Vector2 v, float a, bool p) {
        body = rb;
        velocity = v;
        angle = a;
		isPlayer = p;
    }
}
