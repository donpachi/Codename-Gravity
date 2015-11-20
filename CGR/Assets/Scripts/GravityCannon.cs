using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GravityCannon : MonoBehaviour {

	public Animator anim;
	public GameObject cannonTip;

    private GameObject player;
    private Rigidbody2D playerBody;
    private float LAUNCHFORCE = 5.0f;
    private bool cannonReady = false;
	
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
        if (cannonReady && TouchController.Instance.getTouchDirection() != TouchController.TouchLocation.NONE)
        {
            cannonReady = false;
            FirePlayer();
        }
    }

	void OnCollisionEnter2D(Collision2D collisionInfo) {
		if (collisionInfo.gameObject.tag == "Player") {
            print("Collided");
            player.GetComponent<Walk>().enabled = false;
            Vector3 hiddenPosition = new Vector3(cannonTip.transform.position.x + 1,
                                                 cannonTip.transform.position.y + 1,
                                                 -2);
            playerBody = collisionInfo.rigidbody;
            playerBody.gravityScale = 0f;
			playerBody.Sleep();
            playerBody.GetComponent<Transform>().position = (hiddenPosition);
            player.GetComponent<Player>().ToggleRender();
            print("Start timer");

            anim.SetBool("activated", true);
		}
	}

    void EnableFireButton()
    {
        print("finish timer");
        cannonReady = true;
    }

	public void FirePlayer() {
        Vector2 direction;
		Vector3 firePosition = new Vector3 (cannonTip.transform.position.x,
		                                   cannonTip.transform.position.y,
		                                   -2);
        player.GetComponent<Player>().ToggleRender();
        playerBody.GetComponent<Transform>().position = (firePosition);
		playerBody.WakeUp();
		player.GetComponent<Player>().LaunchStatusOn();
		direction = (player.GetComponent<Transform>().position - this.GetComponent<Transform> ().position).normalized;
        playerBody.AddForce(direction * LAUNCHFORCE, ForceMode2D.Impulse);

        anim.SetBool("activated", false);

	}
}
