using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GravityCannon : MonoBehaviour {

	private Animator anim;
	private GameObject player;
	private Rigidbody2D playerBody;
	public GameObject cannonTip;
	public GameObject[] buttons;

	private bool activated = false;
	private float LAUNCHFORCE = 5f;
	
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnCollisionEnter2D(Collision2D collisionInfo) {
		playerBody = collisionInfo.rigidbody;
		if (collisionInfo.gameObject.tag == "Player") {
			Vector3 newPosition = new Vector3 (this.transform.position.x + 2.5f,
			                                   this.transform.position.y + 1.0f,
				                               1);
			playerBody.GetComponent<Transform>().position = (newPosition);
			player.GetComponent<Controls>().resetMovementFlags();
			playerBody.gravityScale = 0f;
			playerBody.Sleep();

			activated = true;
			anim.SetBool("activated", true);

			for (int i = 0; i < buttons.Length - 1; i++) {
				buttons[i].SetActive(false);
			}
			buttons[buttons.Length - 1].SetActive(true);
		}
	}

	public void FireDown() {
		Vector3 newPosition = new Vector3 (cannonTip.transform.position.x,
		                                   cannonTip.transform.position.y,
		                                   -2);
		playerBody.GetComponent<Transform>().position = (newPosition);
		playerBody.WakeUp();
		playerBody.gravityScale = 1f;

		activated = false;
		anim.SetBool("activated", false);
		
		for (int i = 0; i < buttons.Length - 1; i++) {
			buttons[i].SetActive(true);
		}
		buttons[buttons.Length - 1].SetActive(false);

	}
}
