using UnityEngine;
using System.Collections;

public class BreakableWall : MonoBehaviour {

	public Animator anima;
	public float breakThreshold;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnCollisionEnter2D(Collision2D collisionInfo) {
		if ((collisionInfo.gameObject.tag == "pushable" || collisionInfo.gameObject.tag == "Boulder" || collisionInfo.gameObject.tag == "Hazard") && collisionInfo.relativeVelocity.magnitude > breakThreshold) {
            this.GetComponent<Collider2D>().enabled = false;
            anima.SetTrigger("destroy");
		}
	}

	public void eraseWall()
	{
		this.gameObject.SetActive(false);
	}
}
