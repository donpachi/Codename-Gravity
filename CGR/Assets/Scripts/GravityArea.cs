using UnityEngine;
using System.Collections;

public class GravityArea : MonoBehaviour {

    public bool EffectOn;
    public float GravityForce = 10;
    public int Direction;
    [Tooltip("0 = Toggle on/off \n1 = Flip Direction \n2 = Cycle Direction")]
    public int Mode;

    private OrientationListener.Orientation areaDirection;
    private Vector2 direvtionVector;

	// Use this for initialization
	void Start () {
        if (Mode < 0 || Mode > 2)
            Mode = 0;
        ChangeOrientation();
    }
	
	// Update is called once per frame
	void Update () {
        //if (GetComponent<AreaEffector2D>().enabled != EffectOn)
        //    GetComponent<AreaEffector2D>().enabled = EffectOn;
    }

    void plateDepressed()
    {
        if (Mode == 0)
            EffectOn = !EffectOn;
        if (Mode == 1)
        {
            if (Direction < 2) Direction += 2;
            else Direction -= 2;
            ChangeOrientation();
        }
        if (Mode == 2)
        {
            Direction += 1;
            if (Direction > 3) Direction = 0;
            ChangeOrientation();
        }

    }

    void plateReleased()
    {

    }

    void addForce(GameObject obj)
    {
        obj.GetComponent<Rigidbody2D>().AddForce(direvtionVector * GravityForce);
    }

    void ChangeOrientation()
    {
        if (Direction == 0)
        {
            areaDirection = OrientationListener.Orientation.PORTRAIT;
            direvtionVector = Vector2.down;
        }
        else if (Direction == 1)
        {
            areaDirection = OrientationListener.Orientation.LANDSCAPE_LEFT;
            direvtionVector = Vector2.left;
        }
        else if (Direction == 2)
        {
            areaDirection = OrientationListener.Orientation.INVERTED_PORTRAIT;
            direvtionVector = Vector2.up;
        }
        else if (Direction == 3)
        {
            areaDirection = OrientationListener.Orientation.LANDSCAPE_RIGHT;
            direvtionVector = Vector2.right;
        }
        else
        {
            Debug.LogError("Please keep direction between 0 - 3");
            areaDirection = OrientationListener.Orientation.PORTRAIT;
            direvtionVector = Vector2.down;
        }
    }

    //Object has entered the area
    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;
        Rigidbody2D[] bodies;
        if (!EffectOn)
            return;
        if (obj.GetComponent<Rigidbody2D>() != null)
        {
            if (obj.GetComponent<Player>() != null)
            {
                if (obj.GetComponent<Player>().IsSuctioned())
                    return;
                obj.GetComponent<Animator>().SetInteger("Orientation", (int)areaDirection);
                obj.GetComponent<Player>().GravityZoneOn();
            }
            else if (obj.GetComponentInChildren<RailBlock>() != null)
            {
                obj.GetComponentInChildren<Animator>().SetInteger("Orientation", (int)areaDirection);
                obj.GetComponent<RailBlock>().GravityZoneOn();
            }
            else if (obj.GetComponent<Minion>() != null && !obj.GetComponent<Minion>().IsFollowing)
            {
                obj.GetComponent<Animator>().SetInteger("Orientation", (int)areaDirection);
                obj.GetComponent<Minion>().GravityZoneOn();
            }

            bodies = obj.GetComponentsInChildren<Rigidbody2D>();
            if (bodies != null)
            {
                foreach (var body in bodies)
                {
                    body.gravityScale = 0;
                }
            }
            obj.GetComponent<Rigidbody2D>().gravityScale = 0;
        }
    }

    //object is still in the area
    //If area turns off, must restore obj properties
    //If area turns on, must apply properties
    void OnTriggerStay2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;
        Rigidbody2D[] bodies;
        if (!EffectOn)
        {
            if (obj.GetComponent<Rigidbody2D>() != null)
            {
                if (obj.GetComponent<Player>() != null)
                {
                    if (obj.GetComponent<Player>().IsSuctioned())
                        return;
                    obj.GetComponent<Animator>().SetInteger("Orientation", (int)WorldGravity.Instance.CurrentGravityDirection);
                    obj.GetComponent<Player>().GravityZoneOff();
                }
                else if (obj.GetComponentInChildren<RailBlock>() != null)
                {
                    obj.GetComponentInChildren<Animator>().SetInteger("Orientation", (int)WorldGravity.Instance.CurrentGravityDirection);
                    obj.GetComponent<RailBlock>().GravityZoneOff();
                }
                else if (obj.GetComponent<Minion>() != null && !obj.GetComponent<Minion>().IsFollowing)
                {
                    obj.GetComponent<Animator>().SetInteger("Orientation", (int)WorldGravity.Instance.CurrentGravityDirection);
                    obj.GetComponent<Minion>().GravityZoneOff();
                }

                bodies = obj.GetComponentsInChildren<Rigidbody2D>();
                if (bodies != null)
                {
                    foreach (var body in bodies)
                    {
                        body.gravityScale = 1;
                    }
                }
                obj.GetComponent<Rigidbody2D>().gravityScale = 1;
            }
        }
        else
        {
            if (obj.GetComponent<Rigidbody2D>() != null)
            {
                if (obj.GetComponent<Player>() != null)
                {
                    if (obj.GetComponent<Player>().IsSuctioned())
                        return;
                    obj.GetComponent<Animator>().SetInteger("Orientation", (int)areaDirection);
                    obj.GetComponent<Player>().GravityZoneOn();
                }
                else if (obj.GetComponentInChildren<RailBlock>() != null)
                {
                    obj.GetComponentInChildren<Animator>().SetInteger("Orientation", (int)areaDirection);
                    obj.GetComponent<RailBlock>().GravityZoneOn();
                }
                else if (obj.GetComponent<Minion>() != null && !obj.GetComponent<Minion>().IsFollowing)
                {
                    obj.GetComponent<Animator>().SetInteger("Orientation", (int)areaDirection);
                    obj.GetComponent<Minion>().GravityZoneOn();
                }

                bodies = obj.GetComponentsInChildren<Rigidbody2D>();
                if (bodies != null)
                {
                    foreach (var body in bodies)
                    {
                        body.gravityScale = 0;
                    }
                }

                obj.GetComponent<Rigidbody2D>().gravityScale = 0;
                addForce(obj);
            }
               
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;
        Rigidbody2D[] bodies;
        if (obj.GetComponent<Rigidbody2D>() != null)
        {
            if (obj.GetComponent<Player>() != null)
            {
                if (obj.GetComponent<Player>().IsSuctioned())
                    return;
                obj.GetComponent<Animator>().SetInteger("Orientation", (int)WorldGravity.Instance.CurrentGravityDirection);
                obj.GetComponent<Player>().GravityZoneOff();
            }
            else if (obj.GetComponentInChildren<RailBlock>() != null)
            {
                obj.GetComponentInChildren<Animator>().SetInteger("Orientation", (int)WorldGravity.Instance.CurrentGravityDirection);
                obj.GetComponent<RailBlock>().GravityZoneOff();
            }
            else if (obj.GetComponent<Minion>() != null && !obj.GetComponent<Minion>().IsFollowing)
            {
                obj.GetComponent<Animator>().SetInteger("Orientation", (int)WorldGravity.Instance.CurrentGravityDirection);
                obj.GetComponent<Minion>().GravityZoneOff();
            }

            bodies = obj.GetComponentsInChildren<Rigidbody2D>();
            if (bodies != null)
            {
                foreach (var body in bodies)
                {
                    body.gravityScale = 1;
                }
            }

            obj.GetComponent<Rigidbody2D>().gravityScale = 1;
        }
    }

}
