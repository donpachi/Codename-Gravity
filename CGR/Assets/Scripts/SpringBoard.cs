using UnityEngine;
using System.Collections;

public class SpringBoard : MonoBehaviour {

    private GameObject player;

    public float SpringForce;
    public Vector2 Direction;
    public Animator anim;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
        Direction = gameObject.GetComponent<Transform>().rotation * Direction;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.name == "Player")
        {
            anim.SetBool("Activated", true);
            if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.PORTRAIT || OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.INVERTED_PORTRAIT)
                player.GetComponent<Rigidbody2D>().velocity = new Vector2(player.GetComponent<Rigidbody2D>().velocity.x, 0);
            else
                player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, player.GetComponent<Rigidbody2D>().velocity.y);
            player.GetComponent<Rigidbody2D>().AddForce(Direction.normalized * SpringForce);
        }
    }

    private void AnimateSpring()
    {
        anim.SetBool("Activated", false);
    }
}
