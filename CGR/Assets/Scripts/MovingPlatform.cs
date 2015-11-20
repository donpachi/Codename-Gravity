using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour {

    public bool MoveRight;
    public bool MoveDown;
    public int XDistance;
    public int YDistance;
    private int XDistRemain;
    private int YDistRemain;
    public float speed;
    GameObject parent;

	// Use this for initialization
	void Start () {
        XDistRemain = XDistance;
        YDistRemain = YDistance;
        parent = this.GetComponentInParent<Transform>().gameObject;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (parent.name == "PressurePlate")
        {

        }
        Vector2 vector = this.transform.position;
        if (MoveRight == false)
        {
            vector = new Vector2(vector.x - speed, vector.y);
            XDistRemain--;
            if (XDistRemain <= 0)
            {
                MoveRight = true;
                XDistRemain = XDistance;
            }
        }
        else 
        {
            vector = new Vector2(vector.x + speed, vector.y);
            XDistRemain--;
            if (XDistRemain <= 0)
            {
                MoveRight = false;
                XDistRemain = XDistance;
            }
        }

        if (MoveDown == true)
        {
            vector = new Vector2(vector.x, vector.y - speed);
            YDistRemain--;
            if (YDistRemain <= 0)
            {
                MoveDown = false;
                YDistRemain = YDistance;
            }
        }
        else
        {
            vector = new Vector2(vector.x, vector.y + speed);
            YDistRemain--;
            if (YDistRemain <= 0)
            {
                MoveDown = true;
                YDistRemain = YDistance;
            }
        }
        
        
        this.transform.position = vector;
	}
}
