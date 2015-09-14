using UnityEngine;
using System.Collections;

public class SuctionCup : MonoBehaviour {

	public GameObject player;
    public int suctionTimer = 10;

	private ConstantForce2D playerConsForce;
	private Vector2 suctionVector;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D collisionInfo) {
		if (collisionInfo.gameObject.tag == "Player") {
			suctionVector = Physics2D.gravity;
			collisionInfo.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
			playerConsForce = player.GetComponent<ConstantForce2D>();
			playerConsForce.relativeForce = suctionVector * 2;
            this.gameObject.SetActive(false);
            player.GetComponent<Controls>().SuctionCupOn();

            Invoke("SuctionStatusEnd", suctionTimer);

            playerConsForce.relativeForce = new Vector2(0, 0);
            collisionInfo.GetComponent<Rigidbody2D>().gravityScale = 1.0f;

            player.GetComponent<Controls>().SuctionStatusEnd();
		}
	}
}
