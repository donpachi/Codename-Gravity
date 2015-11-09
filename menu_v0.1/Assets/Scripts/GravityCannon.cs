using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GravityCannon : MonoBehaviour {

	public Animator anim;
	public GameObject cannonTip;

	private float LAUNCHFORCE = 5.0f;
    private GameObject player;
    private Rigidbody2D playerBody;
    private bool cannonReady = false;
	
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
        if (cannonReady && TouchController.Instance.getTouchDirection() != TouchController.TouchLocation.NONE)
            FirePlayer();
    }

	void OnCollisionEnter2D(Collision2D collisionInfo) {
		if (collisionInfo.gameObject.tag == "Player") {
            player.GetComponent<Walk>().enabled = false;

			playerBody = collisionInfo.rigidbody;
            playerBody.gravityScale = 0f;
            Vector3 hiddenPosition = new Vector3 (cannonTip.transform.position.x,
			                                      cannonTip.transform.position.y,
			                                      1);
			playerBody.GetComponent<Transform>().position = (hiddenPosition);
			playerBody.Sleep();
            EnableFireButton();
            

            anim.SetBool("activated", true);
		}
	}

	public void EnableFireButton() {
        cannonReady = true;
	}

	public void FirePlayer() {
		Vector2 direction;
		Vector3 firePosition = new Vector3 (cannonTip.transform.position.x,
		                                   cannonTip.transform.position.y,
		                                   -2);
		playerBody.GetComponent<Transform>().position = (firePosition);
		playerBody.WakeUp();

		player.GetComponent<Player>().LaunchStatusOn();
		direction = (player.GetComponent<Transform>().position - this.GetComponent<Transform> ().position).normalized;
        playerBody.AddForce(direction * LAUNCHFORCE, ForceMode2D.Impulse);
        cannonReady = false;

        anim.SetBool("activated", false);

	}
}
