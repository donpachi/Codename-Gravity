using UnityEngine;
using System.Collections;

public class GravityArea : MonoBehaviour {

    public bool EffectOn;
    public float GravityForce = 10;
    public int Direction;

    private OrientationListener.Orientation areaDirection;
    private Vector2 direvtionVector;

	// Use this for initialization
	void Start () {
        if(Direction == 0)
        {
            areaDirection = OrientationListener.Orientation.PORTRAIT;
            direvtionVector = Vector2.down;
        }
        else if(Direction == 1)
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
	
	// Update is called once per frame
	void Update () {
        //if (GetComponent<AreaEffector2D>().enabled != EffectOn)
        //    GetComponent<AreaEffector2D>().enabled = EffectOn;
    }

    void plateDepressed()
    {
        EffectOn = !EffectOn;
    }

    void plateReleased()
    {

    }

    void addForce(GameObject obj)
    {
        obj.GetComponent<Rigidbody2D>().AddForce(direvtionVector * GravityForce);
    }

    //Object has entered the area
    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;
        if (!EffectOn)
            return;
        if(obj.GetComponent<Rigidbody2D>() != null)
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
            else if (obj.GetComponent<Minion>() != null && !obj.GetComponent<Minion>().isFollowing)
            {
                obj.GetComponent<Animator>().SetInteger("Orientation", (int)areaDirection);
                obj.GetComponent<Minion>().GravityZoneOn();
            }
            else
                return;

            obj.GetComponent<Rigidbody2D>().gravityScale = 0;
        }
    }

    //object is still in the area
    //If area turns off, must restore obj properties
    //If area turns on, must apply properties
    void OnTriggerStay2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;
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
                else if (obj.GetComponent<Minion>() != null && !obj.GetComponent<Minion>().isFollowing)
                {
                    obj.GetComponent<Animator>().SetInteger("Orientation", (int)WorldGravity.Instance.CurrentGravityDirection);
                    obj.GetComponent<Minion>().GravityZoneOff();
                }
                else
                    return;

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
                else if (obj.GetComponent<Minion>() != null && !obj.GetComponent<Minion>().isFollowing)
                {
                    obj.GetComponent<Animator>().SetInteger("Orientation", (int)areaDirection);
                    obj.GetComponent<Minion>().GravityZoneOn();
                }
                else
                    return;

                if(obj.GetComponent<Rigidbody2D>().gravityScale > 0)
                    obj.GetComponent<Rigidbody2D>().gravityScale = 0;
                addForce(obj);
            }
               
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;
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
            else if (obj.GetComponent<Minion>() != null && !obj.GetComponent<Minion>().isFollowing)
            {
                obj.GetComponent<Animator>().SetInteger("Orientation", (int)WorldGravity.Instance.CurrentGravityDirection);
                obj.GetComponent<Minion>().GravityZoneOff();
            }
            else
                return;

            obj.GetComponent<Rigidbody2D>().gravityScale = 1;
        }
    }

}
