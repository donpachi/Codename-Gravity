using UnityEngine;
using System.Collections;

public class SuctionCup : MonoBehaviour {

	public GameObject player;
    public float suctionTimer;

	private Rigidbody2D playerBody;
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
            playerBody = collisionInfo.GetComponent<Rigidbody2D>();
			playerBody.gravityScale = 0.0f;
            playerBody.GetComponent<ConstantForce2D>().relativeForce = suctionVector * 2;
            this.gameObject.SetActive(false);
            player.GetComponent<Controls>().SuctionStatusOn();

            Invoke("suctionCupBootsEnd", suctionTimer);
		}
	}

    private void suctionCupBootsEnd()
    {
        playerBody.GetComponent<ConstantForce2D>().relativeForce = new Vector2(0, 0);
        playerBody.gravityScale = 1.0f;
        player.GetComponent<Controls>().SuctionStatusEnd();
    }
}
