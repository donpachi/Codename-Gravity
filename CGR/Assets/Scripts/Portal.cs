using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Portal : MonoBehaviour {

    public GameObject linkedPortal;
    public float transitionSpeed;

    private List<Node> bodies;
    private Queue<GameObject> waitForLeave;
    private float distanceThreshold;
    private GameObject playerStatus;
    private bool playerComing;

	// Use this for initialization
	void Start () {
        playerStatus = GameObject.Find("Player");
        distanceThreshold = 0.1f;
        bodies = new List<Node>();
        waitForLeave = new Queue<GameObject>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (bodies.Count > 0)
        {
            for (int i = 0; i < bodies.Count; i++)
            {
                Rigidbody2D body = bodies[i].obj.GetComponent<Rigidbody2D>();
                body.transform.position = new Vector3(Mathf.Lerp(body.transform.position.x, this.transform.position.x, transitionSpeed * Time.deltaTime),
                                Mathf.Lerp(body.transform.position.y, this.transform.position.y, transitionSpeed * Time.deltaTime),
                                0f);

                if ((this.transform.position - body.transform.position).magnitude <= distanceThreshold)
                {
                    body.Sleep();
                    StartCoroutine(LaunchPlayer(bodies[i], 2f));
                    waitForLeave.Enqueue(bodies[i].obj);
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
            GameObject obj = collisionInfo.gameObject;
            bool isPlayer = collisionInfo.gameObject.name.Equals("Player");

            if (waitForLeave.Contains(obj))
                return ;

            if (isPlayer) {
				playerStatus.GetComponent<Player>().InTransitionStatusOn();
                playerStatus.GetComponent<GroundCheck>().enabled = false;
				playerStatus.GetComponent<Walk>().enabled = false;
				playerStatus.GetComponent<SuctionWalk>().enabled = false;
            }

            collisionInfo.gameObject.SetActive(false);

            linkedPortal.GetComponent<Portal>().SendObject(obj, collisionInfo.relativeVelocity, this.transform.rotation.eulerAngles.z, isPlayer);
        }
    }

    void OnTriggerExit2D(Collider2D collisionInfo)
    {
        if (waitForLeave.Count > 0)
            waitForLeave.Dequeue();
    }

    public void SendObject(GameObject obj, Vector2 velocity, float angle, bool isPlayer)
    {
        bodies.Add(new Node(obj, velocity, angle, isPlayer));
    }

    IEnumerator LaunchPlayer(Node entity, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        Rigidbody2D body = entity.obj.GetComponent<Rigidbody2D>();
        entity.obj.SetActive(true);

        float launchAngle = this.transform.rotation.eulerAngles.z - entity.angle;
        body.transform.position = new Vector3(body.transform.position.x,
                                           body.transform.position.y,
                                           -2f);
		
        entity.velocity = Quaternion.AngleAxis(launchAngle, Vector3.forward) * entity.velocity;
        body.velocity = entity.velocity;

        if (!playerStatus.GetComponent<Player>().IsLaunched())
		    body.gravityScale = 1.0f;

		if (entity.isPlayer) {
			playerStatus.GetComponent<Player>().InTransitionStatusEnd();
            playerStatus.GetComponent<GroundCheck>().enabled = true;

            if (playerStatus.GetComponent<Player>().IsSuctioned())
				playerStatus.GetComponent<SuctionWalk>().enabled = true;
			else
				playerStatus.GetComponent<Walk>().enabled = true;
		}
    }
}

class Node
{
    public GameObject obj;
    public Vector2 velocity;
    public float angle;
	public bool isPlayer;

    public Node(GameObject go, Vector2 v, float a, bool p) {
        obj = go;
        velocity = v;
        angle = a;
		isPlayer = p;
    }
}
