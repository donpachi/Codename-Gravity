using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public float OnGroundRaySize = .5f;
    public float RayOffsetWidth = .1f;
    public bool InAir { get; private set; }

    private LayerMask wallMask;

    void Start()
    {
        wallMask = 1 << LayerMask.NameToLayer("Walls");
    }

    void FixedUpdate()
    {
        groundCheck();
    }
    //Raycasts down to check for a floor
    void groundCheck()
    {
        Vector3 rayOffset = OrientationListener.instanceOf.getRelativeRightVector() * RayOffsetWidth;
        RaycastHit2D groundCheckRay = Physics2D.Raycast(transform.position + rayOffset, OrientationListener.instanceOf.getWorldDownVector(), OnGroundRaySize, wallMask);
        RaycastHit2D groundCheckRay1 = Physics2D.Raycast(transform.position - rayOffset, OrientationListener.instanceOf.getWorldDownVector(), OnGroundRaySize, wallMask);

        Debug.DrawRay(transform.position + rayOffset, OrientationListener.instanceOf.getWorldDownVector() * OnGroundRaySize, Color.green, 1);
        Debug.DrawRay(transform.position - rayOffset, OrientationListener.instanceOf.getWorldDownVector() * OnGroundRaySize, Color.red, 1);

        if (groundCheckRay.collider != null || groundCheckRay1.collider != null)
        {
            InAir = false;
        }
        else
            InAir = true;
    }

}

