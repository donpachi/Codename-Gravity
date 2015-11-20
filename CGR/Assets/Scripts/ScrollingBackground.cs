using UnityEngine;
using System.Collections;

public class ScrollingBackground : MonoBehaviour {

    public float speed;

    private Rigidbody2D player;
    private float horizontal = 0;
    private float vertical = 0;

    void Start () {
        speed = speed / 10000;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
        horizontal += (player.velocity.x * speed);
        vertical += (player.velocity.y * speed);
        this.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(horizontal, vertical);
    }
}
