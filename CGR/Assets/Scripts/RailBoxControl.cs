using UnityEngine;
using System.Collections;

/// <summary>
/// Script that defines the Box movement and player box interaction.
/// </summary>
public class RailBoxControl : MonoBehaviour {

    //defined in unity
    public float THRUST;
    public float MAXSPEED;
    public bool PlayerControlled;

    Rigidbody2D objectRb;
    private Animator anim;
    GameObject player;
    GameObject mainCamera;

    // Use this for initialization
    void Start () {
        objectRb = gameObject.GetComponentInParent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        player = GameObject.Find("Player");
        mainCamera = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (PlayerControlled && objectRb.velocity.magnitude < MAXSPEED)
            applyMoveForce(THRUST);
    }

    void applyMoveForce(float force)
    {
        TouchController.TouchLocation movementDirection = TouchController.Instance.getTouchDirection();

        switch (movementDirection)
        {
            case TouchController.TouchLocation.LEFT:
                objectRb.AddForce(OrientationListener.instanceOf.getWorldLeftVector() * force, ForceMode2D.Impulse);
                break;
            case TouchController.TouchLocation.RIGHT:
                objectRb.AddForce(OrientationListener.instanceOf.getWorldRightVector() * force, ForceMode2D.Impulse);
                break;
            case TouchController.TouchLocation.NONE:
                break;
        }
    }

    void activateBox()
    {
        objectRb.isKinematic = false;
    }

    void deactivateBox()
    {
        objectRb.isKinematic = true;
    }

    void OnTriggerEnter2D(Collider2D colliderEvent)
    {
        if (anim.GetBool("BoxActive") == true && colliderEvent.gameObject.name == "Player")
        {
            anim.SetBool("HasEntered", true);
            colliderEvent.gameObject.SetActive(false);
            mainCamera.GetComponent<FollowPlayer>().setFollowObject(gameObject);
        }
    }

    void activateControl()
    {
        PlayerControlled = true;
    }

    void deactivateControl()
    {
        player.SetActive(true);
        player.transform.position = transform.position + (Vector3)OrientationListener.instanceOf.getRelativeUpVector();
        player.GetComponent<Player>().gravitySpriteUpdate(OrientationListener.instanceOf.currentOrientation(), 0.0f);
        player.GetComponent<Rigidbody2D>().AddForce(OrientationListener.instanceOf.getRelativeUpVector() * 200);
        mainCamera.GetComponent<FollowPlayer>().setFollowObject(player);
    }

    //Event handling
    void swipeCheck(TouchController.SwipeDirection direction)
    {
        if (PlayerControlled && direction == TouchController.SwipeDirection.UP)
        {
            anim.SetBool("HasEntered", false);
            PlayerControlled = false;
        }
    }

    void OnEnable()
    {
        TouchController.OnSwipe += swipeCheck;
    }
    void OnDisable()
    {
        TouchController.OnSwipe -= swipeCheck;
    }

}
