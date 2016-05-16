using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public float OnGroundRaySize = .5f;
    public bool InAir;
    public bool inGravityArea;
    private float RayOffsetMax = 0.25f;
    private float RayOffsetMagnitude;
    private float colliderLength;
    private Vector2 colliderVector;
    private GameObject[] collidedObjects;
    private CircleCollider2D circleCollider;
    private BoxCollider2D boxCollider;
    private OrientationListener.Orientation gravityAreaDirection;

    public LayerMask wallMask;

    void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        boxCollider = GetComponent<BoxCollider2D>();

        if (boxCollider != null)
        {
            OnGroundRaySize = (boxCollider.size.y / 2) + 0.1f;
            colliderLength = boxCollider.size.x;
        }
        else if (circleCollider != null)
        {
            OnGroundRaySize = circleCollider.radius + 0.1f;
            colliderLength = circleCollider.radius * 2;
        }

        int numOfRays = Mathf.CeilToInt(colliderLength / RayOffsetMax);
        RayOffsetMagnitude = colliderLength / numOfRays;
        numOfRays++;
        collidedObjects = new GameObject[numOfRays];

        inGravityArea = false;
        wallMask = 1 << LayerMask.NameToLayer("Walls") | 1 << LayerMask.NameToLayer("ThroughWalls");
    }

    void FixedUpdate()
    {
        groundCheck();
    }
    //Raycasts down to check for a floor
    void groundCheck()
    {
        Vector2 origin = (Vector2)transform.position;
        Vector2 rayOffset = new Vector2();
        Vector2 direction = new Vector2();
        Vector2 colliderOffset = new Vector2();
        RaycastHit2D groundCheckRay;
        int numOfMissedColliders = 0;

        if (boxCollider != null)
        {
            colliderOffset = boxCollider.offset;
        }
        else if (circleCollider != null)
        {
            colliderOffset = circleCollider.offset;
        }

        getOffsetAndOrigin(ref rayOffset, ref origin, ref direction, colliderOffset);

        for (int i = 0; i < collidedObjects.Length; i++)
        {
            collidedObjects[i] = null;
            groundCheckRay = Physics2D.Raycast(origin, direction, OnGroundRaySize, wallMask);
            Debug.DrawRay(origin, direction * OnGroundRaySize, Color.blue, 0.5f);
            origin += rayOffset;
            if (groundCheckRay.collider != null)
            {
                collidedObjects[i] = groundCheckRay.collider.gameObject;
                InAir = false;
            }
            else
            {
                numOfMissedColliders++;
            }

            if(numOfMissedColliders == collidedObjects.Length)
            {
                InAir = true;
            }
        }
    }

    void getOffsetAndOrigin(ref Vector2 rayOffset, ref Vector2 origin, ref Vector2 direction, Vector2 colliderOffset)
    {
        OrientationListener.Orientation actualOrientation;
        OrientationListener.Orientation currentScreenOrientation = WorldGravity.Instance.CurrentGravityDirection;

        if (!InAir && name == "Player" && GetComponent<Player>().IsSuctioned()) actualOrientation = (OrientationListener.Orientation) GetComponent<Animator>().GetInteger("Orientation");
        else if (inGravityArea) actualOrientation = gravityAreaDirection;
        else actualOrientation = currentScreenOrientation;

        if (actualOrientation == OrientationListener.Orientation.PORTRAIT)
        {
            rayOffset = Vector2.right * RayOffsetMagnitude;
            origin.x = origin.x + colliderOffset.x;
            origin.y = origin.y + colliderOffset.y; 
            origin.x = origin.x - colliderLength / 2;
            direction = Vector2.down;
        }

        else if (actualOrientation == OrientationListener.Orientation.LANDSCAPE_LEFT)
        {
            rayOffset = Vector2.down * RayOffsetMagnitude;
            origin.x = origin.x + colliderOffset.y;
            origin.y = origin.y + colliderOffset.x;
            origin.y = origin.y + colliderLength / 2;
            direction = Vector2.left;
        }

        else if (actualOrientation == OrientationListener.Orientation.INVERTED_PORTRAIT)
        {
            rayOffset = Vector2.left * RayOffsetMagnitude;
            origin.x = origin.x - colliderOffset.x;
            origin.y = origin.y - colliderOffset.y;
            origin.x = origin.x + colliderLength / 2;
            direction = Vector2.up;
        }

        else if (actualOrientation == OrientationListener.Orientation.LANDSCAPE_RIGHT)
        {
            rayOffset = Vector2.up * RayOffsetMagnitude;
            origin.x = origin.x - colliderOffset.y;
            origin.y = origin.y - colliderOffset.x;
            origin.y = origin.y - colliderLength / 2;
            direction = Vector2.right;
        }

    }

    public GameObject[] getCollidedObjects()
    {
        return collidedObjects;
    }

    public OrientationListener.Orientation getGravityAreaOrientation()
    {
        return gravityAreaDirection;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        GravityArea gravityArea = collision.gameObject.GetComponent<GravityArea>();
        if (gravityArea != null)
        {
            inGravityArea = true;
            gravityAreaDirection = gravityArea.getOrientation();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        GravityArea gravityArea = collision.gameObject.GetComponent<GravityArea>();
        if (gravityArea != null)
        {
            inGravityArea = false;
        }
    }
}

