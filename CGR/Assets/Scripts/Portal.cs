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
    private Portal linkedPortalScript;

    // Use this for initialization
    void Start () {
        playerStatus = GameObject.Find("Player");
        distanceThreshold = 0.1f;
        bodies = new List<Node>();
        waitForLeave = new Queue<GameObject>();
        linkedPortalScript = linkedPortal.GetComponent<Portal>();
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

                if ((transform.position - body.transform.position).magnitude <= distanceThreshold)
                {
                    StartCoroutine(LaunchObject(bodies[i], 2f));
                    waitForLeave.Enqueue(bodies[i].obj);
                    bodies.RemoveAt(i);
                    i -= 1;
                }
            }
        }
	}

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        GameObject obj = collisionInfo.gameObject;

        if (waitForLeave.Contains(obj))
            return ;

        if (!obj.GetComponent<Animator>())
        {
            collisionInfo.gameObject.SetActive(false);
            linkedPortalScript.SendObject(obj, collisionInfo.relativeVelocity, transform.rotation.eulerAngles.z, Node.ObjType.Other);
        }
        else if (obj == playerStatus)
        {
            collisionInfo.gameObject.GetComponent<Player>().DeactivateControl(StateChange.PORTAL_IN);
            linkedPortalScript.SendObject(obj, collisionInfo.relativeVelocity, transform.rotation.eulerAngles.z, Node.ObjType.Player);
        }
        else if (obj.GetComponent<Minion>())
        {
            Minion minion = obj.GetComponent<Minion>();
            if (!minion.IsFollowing)
            {
                minion.DeactivateControl(StateChange.PORTAL_IN);
                linkedPortalScript.SendObject(obj, collisionInfo.relativeVelocity, transform.rotation.eulerAngles.z, Node.ObjType.Minion);
            }
        }
        else
            Debug.LogError("Object in portal not accounted for");
    }

    void OnTriggerExit2D(Collider2D collisionInfo)
    {
        if (waitForLeave.Count > 0)
            waitForLeave.Dequeue();
    }

    void SendObject(GameObject obj, Vector2 velocity, float angle, Node.ObjType type)
    {
        bodies.Add(new Node(obj, velocity, angle, type));
    }

    IEnumerator LaunchObject(Node entity, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        Rigidbody2D body = entity.obj.GetComponent<Rigidbody2D>();
        entity.obj.SetActive(true);

        float launchAngle = transform.rotation.eulerAngles.z - entity.angle;
        body.transform.position = new Vector3(body.transform.position.x,
                                           body.transform.position.y,
                                           -2f);
		
        entity.velocity = Quaternion.AngleAxis(launchAngle, Vector3.forward) * entity.velocity;
        body.velocity = entity.velocity;

		if (entity.type == Node.ObjType.Player) {
            entity.obj.GetComponent<Player>().ReactivateControl(StateChange.PORTAL_OUT);
		}
        if(entity.type == Node.ObjType.Minion)
        {
            entity.obj.GetComponent<Minion>().ReactivateControl(StateChange.PORTAL_OUT);
        }
    }

    void CheckpointRestart()
    {
        StopAllCoroutines();
        if(bodies.Count > 0)
        {
            foreach (var body in bodies)
            {
                if (!body.obj.GetComponent<Animator>())
                {
                    body.obj.SetActive(true);
                }
            }
            bodies.Clear();
        }
        if(waitForLeave.Count > 0)
        {
            waitForLeave.Clear();
        }
    }

    void OnEnable()
    {
        LevelManager.OnCheckpointLoad += CheckpointRestart;
    }
    void OnDisable()
    {
        LevelManager.OnCheckpointLoad -= CheckpointRestart;
    }


}

class Node
{
    public GameObject obj;
    public Vector2 velocity;
    public float angle;
    public enum ObjType { Player, Minion, Other }
    public ObjType type;

    public Node(GameObject go, Vector2 v, float a, ObjType t) {
        obj = go;
        velocity = v;
        angle = a;
		type = t;
    }
}
