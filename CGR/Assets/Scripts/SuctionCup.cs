using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SuctionCup : MonoBehaviour {

    public float suctionTimer;
    public int SuctionForce;

    private float timer;
    private Player player;

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
        player = FindObjectOfType<Player>();
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
		if (collisionInfo.gameObject.name == "Player") {
            player.SuctionStatusOn(SuctionForce);

            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;

            timer = suctionTimer;
            player.GetComponent<SuctionWalk>().SetTimer(suctionTimer);
            triggerSuctionEvent();
		}
	}

    private void suctionCupBootsEnd()
    {
        timer = 0;

        this.GetComponent<Collider2D>().enabled = true;
        this.GetComponent<SpriteRenderer>().enabled = true;
    }
}