using UnityEngine;
using System.Collections;
using UnityEngine.UI;       //FOR DEBUG REMOVE LATER

public class Walk : MonoBehaviour {

    public float THRUST = 0.5f;
    public float INAIRTHRUST = 0.1f;
    public float MAXSPEED = 10f;
    public float MAXFLOATSPEED = 2f;
    public float ForwardRaySize = 0.5f;
    public LayerMask ForwardRayMask;

    private Rigidbody2D rBody;
    private Animator anim;
    private float minWalkSpeed = 0.1f;
    private PinchtoZoom cameraZoom;
    private GroundCheck gCheck;

    // Use this for initialization
    void Start ()
    {
        rBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        cameraZoom = GameObject.Find("Main Camera").GetComponent<PinchtoZoom>();
        gCheck = GetComponent<GroundCheck>();
    }
    
    void FixedUpdate()
    {
        if (Input.touchCount == 0 || rBody.velocity.magnitude < minWalkSpeed)
            anim.SetBool("Moving", false);
    }

    void applyMoveForce(float force, TouchController.TouchLocation direction)
    {
        switch (direction)
        {
            case TouchController.TouchLocation.LEFT:
                rBody.AddForce(transform.right * -force, ForceMode2D.Impulse);
                break;
            case TouchController.TouchLocation.RIGHT:
                rBody.AddForce(transform.right * force, ForceMode2D.Impulse);
                break;
            case TouchController.TouchLocation.NONE:
                break;
        }
    }

    /// <summary>
    /// Listens for every screen touch, just returns when its more than 1
    /// </summary>
    /// <param name="data"></param>
    void screenTouched(TouchInstanceData data)
    {
        if (TouchController.Instance.GetTouchCount() > 1)
        {
            anim.SetBool("Moving", false);
            return;
        }

        if (forwardCheck(data.touchLocation))
            return;

        if (gCheck.InAir && rBody.velocity.magnitude < MAXFLOATSPEED && !cameraZoom.Zooming)
        {
            applyMoveForce(INAIRTHRUST, data.touchLocation);
        }
        else if (rBody.velocity.magnitude < MAXSPEED && !gCheck.InAir && !cameraZoom.Zooming)
        {
            applyMoveForce(THRUST, data.touchLocation);
            if (data.touchLocation != TouchController.TouchLocation.NONE)
                anim.SetBool("Moving", true);
        }
    }

    /// <summary>
    /// Forward raycast to prevent walking when hitting a wall
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    bool forwardCheck(TouchController.TouchLocation direction)
    {
        Vector2 forwardRay;
        if (direction == TouchController.TouchLocation.LEFT)
            forwardRay = transform.right * -1;
        else if (direction == TouchController.TouchLocation.RIGHT)
            forwardRay = transform.right;
        else
            return false;

        RaycastHit2D forwardCheckRay = Physics2D.Raycast(transform.position, forwardRay, ForwardRaySize, ForwardRayMask);
        Debug.DrawRay(transform.position, forwardRay * ForwardRaySize, Color.cyan, 0.5f);
        if (forwardCheckRay.collider != null)
        {
            return true;
        }
        return false;
    }

    public void StopWalkAnim()
    {
        anim.SetBool("Moving", false);
    }

    void OnEnable()
    {
        TouchController.OnHold += screenTouched;
    }

    void OnDisable()
    {
        TouchController.OnHold -= screenTouched;
    }

}
