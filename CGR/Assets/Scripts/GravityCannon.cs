using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GravityCannon : MonoBehaviour {

	public Animator anim;

    private Transform cannonTip;
    private Transform launchPosition;
    private GameObject player;
    private Rigidbody2D playerBody;
    private float LAUNCHFORCE = 5.0f;
    private bool cannonReady = false;
	
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
        Transform[] components = this.GetComponentsInChildren<Transform>();
        foreach (var i in components)
        {
            //print(i.position);
            if (i.name == "CannonTip")
                cannonTip = i;
            else if (i.name == "LaunchPosition")
                launchPosition = i;
        }
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
            player.GetComponent<Walk>().enabled = false;
            playerBody = collisionInfo.rigidbody;
            playerBody.gravityScale = 0f;
			playerBody.Sleep();
            playerBody.GetComponent<Transform>().position = launchPosition.position;
            player.GetComponent<Player>().ToggleRender();

            anim.SetBool("activated", true);
		}
	}

    void EnableFireButton()
    {
        cannonReady = true;
    }

	public void FirePlayer() {
        Vector2 direction;
        player.GetComponent<Player>().ToggleRender();
        playerBody.GetComponent<Transform>().position = cannonTip.position;
		playerBody.WakeUp();
		player.GetComponent<Player>().LaunchStatusOn();
		direction = (player.GetComponent<Transform>().position - this.GetComponent<Transform>().position).normalized;
        playerBody.AddForce(direction * LAUNCHFORCE, ForceMode2D.Impulse);

        anim.SetBool("activated", false);

	}
}
