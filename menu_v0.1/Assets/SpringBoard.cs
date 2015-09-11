using UnityEngine;
using System.Collections;

public class SpringBoard : MonoBehaviour {

    private GameObject player;

    public float SpringForce;
    public Vector2 Direction;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        Direction = gameObject.GetComponent<Transform>().rotation * Direction;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        player.GetComponent<Rigidbody2D>().AddForce(Direction.normalized * SpringForce);
    }
}
