using UnityEngine;
using System.Collections;

public class WorldGravity : MonoBehaviour {
    public float GRAVITYVALUE = 25f;
    public float GRAVITYCOOLDOWN = 5f;

    private bool gravityOnCooldown;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void initialize()
    {
        Physics2D.gravity = OrientationListener.
    }
}
