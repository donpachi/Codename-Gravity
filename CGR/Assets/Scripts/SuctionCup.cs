using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SuctionCup : MonoBehaviour {

    public float suctionTimer;
    public int SuctionForce;

    private float timer;
    private bool triggered;
    private GameObject player;
    private Rigidbody2D playerBody;

    //Event thrown when picked up
    public delegate void SuctionCupActivated(float time);   //give it a length for the timer
    public static event SuctionCupActivated SCActivated;

    void triggerSuctionEvent()
    {
        if (SCActivated != null)
            SCActivated(suctionTimer);
    }

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
        timer = 0;
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        if (timer != 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                suctionCupBootsEnd();
            }
        }

	}
    /// <summary>
    /// When player enters this will set the suction force, turn off all unwanted components
    /// and turn on suction walk
    /// </summary>
    /// <param name="collisionInfo"></param>
	void OnTriggerEnter2D(Collider2D collisionInfo) {
		if (collisionInfo.gameObject.name == "Player" && !triggered) {
            playerBody = collisionInfo.GetComponent<Rigidbody2D>();
            player.GetComponent<Player>().SuctionStatusOn(SuctionForce);

            this.GetComponent<Collider2D>().enabled = false;
            this.GetComponent<SpriteRenderer>().enabled = false;

            timer = suctionTimer;
            player.GetComponent<SuctionWalk>().SetTimer(suctionTimer);
		}
	}

    private void suctionCupBootsEnd()
    {
        timer = 0;

        this.GetComponent<Collider2D>().enabled = true;
        this.GetComponent<SpriteRenderer>().enabled = true;
    }
}