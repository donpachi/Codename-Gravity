﻿using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public float OnGroundRaySize = .5f;
    public float RayOffsetWidth = .1f;
    public bool InAir { get; private set; }
    private GameObject player;

    private LayerMask wallMask;

    void Start()
    {
        wallMask = 1 << LayerMask.NameToLayer("Walls");
        player = GameObject.Find("Player");
    }

    void FixedUpdate()
    {
        groundCheck();
    }
    //Raycasts down to check for a floor
    void groundCheck()
    {
        if (!player.GetComponent<Player>().IsSuctioned())
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
        else
        {
            Vector3 rayOffset = transform.right * RayOffsetWidth;
            RaycastHit2D groundCheckRay = Physics2D.Raycast(transform.position + rayOffset, transform.up * -1, OnGroundRaySize, wallMask);
            RaycastHit2D groundCheckRay1 = Physics2D.Raycast(transform.position - rayOffset, transform.up * -1, OnGroundRaySize, wallMask);

            Debug.DrawRay(transform.position + rayOffset, transform.up * -OnGroundRaySize, Color.green, 0.5f);
            Debug.DrawRay(transform.position - rayOffset, transform.up * -OnGroundRaySize, Color.red, 0.5f);

            if (groundCheckRay.collider != null || groundCheckRay1.collider != null)
            {
                player.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
                player.GetComponent<Rigidbody2D>().GetComponent<ConstantForce2D>().enabled = true;
                InAir = false;
            }
            else
            {
                player.GetComponent<Player>().gravitySpriteUpdate(GetComponent<WorldGravity>().CurrentGravityDirection, 0);
                player.GetComponent<Rigidbody2D>().gravityScale = 1.0f;
                player.GetComponent<Rigidbody2D>().GetComponent<ConstantForce2D>().enabled = false;
                InAir = true;
            }
        }
    }
}

