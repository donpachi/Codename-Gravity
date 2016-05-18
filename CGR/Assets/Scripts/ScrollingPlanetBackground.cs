using UnityEngine;
using System.Collections;

public class ScrollingPlanetBackground : MonoBehaviour {

    public float speed;
    public float naturalSpeedX;
    public float naturalSpeedY;
    private Rigidbody2D player;
    private Player character;
    private float horizontal = 0;
    private float vertical = 0;

    void Start()
    {
        speed /= 10000;
        naturalSpeedX /= 10000;
        naturalSpeedY /= 10000;
        player = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        character = player.GetComponent<Player>();
        horizontal = transform.localPosition.x;
        vertical = transform.localPosition.y;
        AddListener(character);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0 && player.velocity.magnitude > 1)
        {
            horizontal -= (player.velocity.x * speed);
            vertical -= (player.velocity.y * speed);
        }
        horizontal -= naturalSpeedX;
        vertical -= naturalSpeedY;
        if (horizontal > 20.48f)
            horizontal = -20.48f;
        if (horizontal < -20.48f)
            horizontal = 20.48f;
        if (vertical > 20.48f)
            vertical = -20.48f;
        if (vertical < -20.48f)
            vertical = 20.48f;
        this.GetComponent<Transform>().localPosition = new Vector3(horizontal, vertical, 9.7f);
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