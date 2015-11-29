using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour {

    public bool MoveRight;
    public bool MoveDown;
    public int XDistance = -1;
    public int YDistance = -1;
    private int XDistRemain;
    private int YDistRemain;
    public float speed;
    GameObject parent;
    public bool isActive;
    private float RAYCASTDISTANCE = 0.5f;
    GameObject player;
    Player playerScript;
    private bool playerOnTop = false;
    Vector2 moveDifference;

	// Use this for initialization
	void Start () {
        XDistRemain = XDistance;
        YDistRemain = YDistance;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<Player>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (isActive == true)
        {
            Vector2 vector = this.transform.position;
            Vector2 originalPosition = this.transform.position;
            if (MoveRight == false && XDistance != -1)
            {
                if (XDistRemain == 0)
                {
                    MoveRight = true;
                    XDistRemain = XDistance;
                }
                vector = new Vector2(vector.x - speed, vector.y);
                XDistRemain--;
                
            }
            else if (MoveRight == true && XDistance != -1)
            {
                if (XDistRemain == 0)
                {
                    MoveRight = false;
                    XDistRemain = XDistance;
                }
                vector = new Vector2(vector.x + speed, vector.y);
                XDistRemain--;
                
            }

            if (MoveDown == true && YDistance != -1)
            {
                if (YDistRemain == 0)
                {
                    MoveDown = false;
                    YDistRemain = YDistance;
                }
                vector = new Vector2(vector.x, vector.y - speed);
                YDistRemain--;
                
            }
            else if (MoveDown == false && YDistance != -1)
            {
                if (YDistRemain == 0)
                {
                    MoveDown = true;
                    YDistRemain = YDistance;
                }
                vector = new Vector2(vector.x, vector.y + speed);
                YDistRemain--;
                
            }

            moveDifference = new Vector2(originalPosition.x - vector.x, originalPosition.y - vector.y);
            if (playerOnTop == true)
            {
                player.transform.position = new Vector2(player.transform.position.x - moveDifference.x, player.transform.position.y - moveDifference.y);
            }

            this.transform.position = vector;

            
        }
	}

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
            playerOnTop = false;
    }

    void playerOn()
    {
        playerOnTop = true;
    }

    void plateDepressed()
    {
        isActive = true;
    }
}
