using UnityEngine;
using System.Collections;

public class GravityVortex : MonoBehaviour {

	//Adjustable constants
	public float VORTEXDISTANCE;
	public float VORTEXFORCE;

    private float currentForce;
    private float timer;

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
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//float distance = Vector2.Distance (player.GetComponent<Transform>().position, this.GetComponent<Transform> ().position);

		//if (distance < VORTEXDISTANCE && !player.GetComponent<Player>().IsInTransition()) {
		//	Vector2 direction = (this.GetComponent<Transform>().position - player.GetComponent<Transform> ().position).normalized;
		//	player.GetComponent<Rigidbody2D>().AddForce(direction * VORTEXFORCE * (distance / VORTEXDISTANCE));
		//}
	}

    void OnTriggerStay2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;
        if(obj.GetComponent<Player>() != null)
        {
            float distance = Vector2.Distance(obj.GetComponent<Transform>().position, this.GetComponent<Transform>().position);
            if (!obj.GetComponent<Player>().IsInTransition())
            {
                Vector2 direction = (this.GetComponent<Transform>().position - obj.GetComponent<Transform>().position).normalized;
                obj.GetComponent<Rigidbody2D>().AddForce(direction * currentForce * ((VORTEXDISTANCE - distance) / VORTEXDISTANCE));
                Debug.Log("Distance: " + distance + " Force Applied: " + currentForce * ((VORTEXDISTANCE - distance) / VORTEXDISTANCE));
            }
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, VORTEXDISTANCE);
    }

}
