using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour {

    public string XDirection;
    public string YDirection;
    public int XDistance;
    public int YDistance;
    private int XDistRemain;
    private int YDistRemain;
    public float speed;

	// Use this for initialization
	void Start () {
        XDistRemain = XDistance;
        YDistRemain = YDistance;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Vector2 vector = new Vector2(gameObject.GetComponent<Rigidbody2D>().position.x, gameObject.GetComponent<Rigidbody2D>().position.y);
        if (XDirection != null && XDirection == "left")
        {
            vector = new Vector2(vector.x - speed, vector.y);
            XDistRemain--;
            if (XDistRemain == 0)
            {
                XDirection = "right";
                XDistRemain = XDistance;
            }
        }
        else if (YDirection != null && XDirection == "right") 
        {
            vector = new Vector2(vector.x + speed, vector.y);
            XDistRemain--;
            if (XDistRemain == 0)
            {
                XDirection = "left";
                XDistRemain = XDistance;
            }
        }

        if (YDirection != null && YDirection == "down")
        {
            vector = new Vector2(vector.x, vector.y - speed);
            YDistRemain--;
            if (YDistRemain == 0)
            {
                YDirection = "up";
                YDistRemain = YDistance;
            }
        }
        else if (YDirection != null && YDirection == "up")
        {
            vector = new Vector2(vector.x, vector.y + speed);
            YDistRemain--;
            if (YDistRemain == 0)
            {
                YDirection = "down";
                YDistRemain = YDistance;
            }
        }
        
        
        this.GetComponent<Rigidbody2D>().MovePosition(vector);
	}
}
