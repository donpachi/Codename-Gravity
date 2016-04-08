using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public float OnGroundRaySize = .5f;
    public float RayOffsetWidth = .1f;
    public bool InAir { get; private set; }

    public LayerMask wallMask;

    void Start()
    {
        wallMask = 1 << LayerMask.NameToLayer("Walls") | 1 << LayerMask.NameToLayer("ThroughWalls");
    }

    void FixedUpdate()
    {
        groundCheck();
    }
    //Raycasts down to check for a floor
    void groundCheck()
    {
        if(GetComponent<Player>() != null && GetComponent<Player>().IsSuctioned())
        {
            Vector3 rayOffset = transform.right * RayOffsetWidth;
            RaycastHit2D groundCheckRay = Physics2D.Raycast(transform.position + rayOffset, transform.up * -1, OnGroundRaySize, wallMask);
            RaycastHit2D groundCheckRay1 = Physics2D.Raycast(transform.position - rayOffset, transform.up * -1, OnGroundRaySize, wallMask);

            Debug.DrawRay(transform.position + rayOffset, transform.up * -OnGroundRaySize, Color.blue, 0.5f);
            Debug.DrawRay(transform.position - rayOffset, transform.up * -OnGroundRaySize, Color.magenta, 0.5f);

            if (groundCheckRay.collider != null || groundCheckRay1.collider != null)
            {
                GetComponent<Rigidbody2D>().gravityScale = 0.0f;
                GetComponent<Rigidbody2D>().GetComponent<ConstantForce2D>().enabled = true;
                InAir = false;
            }
            else
            {
                GetComponent<Player>().updatePlayerOrientation(WorldGravity.Instance.CurrentGravityDirection, 0);
                GetComponent<Rigidbody2D>().gravityScale = 1.0f;
                GetComponent<Rigidbody2D>().GetComponent<ConstantForce2D>().enabled = false;
                InAir = true;
            }
        }
        else
        {
            Vector3 rayOffset = transform.right * RayOffsetWidth;
            RaycastHit2D groundCheckRay = Physics2D.Raycast(transform.position + rayOffset, transform.up * -1, OnGroundRaySize, wallMask);
            RaycastHit2D groundCheckRay1 = Physics2D.Raycast(transform.position - rayOffset, transform.up * -1, OnGroundRaySize, wallMask);

            Debug.DrawRay(transform.position + rayOffset, transform.up * -1 * OnGroundRaySize, Color.green, 1);
            Debug.DrawRay(transform.position - rayOffset, transform.up * -1 * OnGroundRaySize, Color.red, 1);

            if (groundCheckRay.collider != null || groundCheckRay1.collider != null)
            {
                InAir = false;
            }
            else
                InAir = true;
        }
    }
}

