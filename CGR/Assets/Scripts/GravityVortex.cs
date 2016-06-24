using UnityEngine;
using System.Collections;

public class GravityVortex : MonoBehaviour {

	//Adjustable constants
	public float VORTEXDISTANCE;
	public float VORTEXFORCE;
    public float PulseInterval;

    private float currentForce;
    private float timer;    //for rings
    private float ringInterval;
    private int ringCount;

	// Use this for initialization
	void Start () {
        foreach (var collider in GetComponents<CircleCollider2D>())
        {
            if (collider.isTrigger)
            {
                collider.radius = VORTEXDISTANCE;
            }
        }
        currentForce = VORTEXFORCE;
        ringInterval = PulseInterval / 2;
        timer = ringInterval;
	}

    void Update()
    {
        if (ringCount >= PulseInterval / 2)
            return;
        if (timer >= ringInterval)
        {
            GameObject ring = (GameObject)Instantiate(Resources.Load("Prefabs/Vortex/VortexRing"));
            ring.transform.position = transform.position;
            ring.transform.parent = transform;
            ring.GetComponent<VortexRing>().Setup(VORTEXDISTANCE, PulseInterval);
            timer = 0;
            ringCount++;
        }
        else
            timer += Time.deltaTime;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;
        if(obj.GetComponent<Rigidbody2D>() != null)
        {
            if (obj.GetComponent<Player>() != null && obj.GetComponent<Player>().currentState == StateChange.SWALK_ON)
                return;

            float distance = Vector2.Distance(obj.GetComponent<Transform>().position, this.GetComponent<Transform>().position);
            Vector2 direction = (this.GetComponent<Transform>().position - obj.GetComponent<Transform>().position).normalized;
            obj.GetComponent<Rigidbody2D>().AddForce(direction * currentForce * ((VORTEXDISTANCE - distance) / VORTEXDISTANCE));
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, VORTEXDISTANCE);
    }

}
