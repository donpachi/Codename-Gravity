using UnityEngine;
using System.Collections;

public class ScrollingBackground : MonoBehaviour {

    public float speed;

    private Rigidbody2D player;
    private Player character;
    private float horizontal = 0;
    private float vertical = 0;

    void Start () {
        speed = speed / 10000;
        player = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        character = player.GetComponent<Player>();
        AddListener(character);
    }
	
	// Update is called once per frame
	void Update () {
		if (Time.timeScale != 0 && player.velocity.magnitude > 1) {
            Debug.Log(player.velocity.magnitude);
			horizontal += (player.velocity.x * speed);
			vertical += (player.velocity.y * speed);
		}
        this.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(horizontal, vertical);
    }

    private void AddListener(Player character)
    {
        character.OnPlayerDeath += HandleOnPlayerDeath;
    }

    private void HandleOnPlayerDeath()
    {
        player.Sleep();
    }
}
