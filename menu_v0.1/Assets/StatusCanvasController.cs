using UnityEngine;
using System.Collections;

public class StatusCanvasController : MonoBehaviour {

    //Positions looking at the phone from portrait
    private Vector3 topLeft = new Vector3(220, 435, 0);
    private Vector3 topRight = new Vector3(-220, 435, 0);
    private Vector3 bottomLeft = new Vector3(-220, -435, 0);
    private Vector3 bottomRight = new Vector3(220, -435, 0);

    GameObject suctionCounter;
    GameObject gravityCooldown;

	// Use this for initialization
	void Start () {
        suctionCounter = transform.FindChild("SuctionText").gameObject;
        gravityCooldown = transform.FindChild("GravityCooldown").gameObject;

	}
	
	// Update is called once per frame
	void Update () {
	}

    //update all the objects in canvas to the correct orientation
    void updateCanvasObjectOrientation(OrientationListener.Orientation orientation)
    {
        foreach (Transform child in transform)
        {
            switch (orientation)
            {
                case OrientationListener.Orientation.PORTRAIT:
                    child.rotation = Quaternion.AngleAxis(0, Vector3.forward);
                    break;
                case OrientationListener.Orientation.LANDSCAPE_RIGHT:
                    child.rotation = Quaternion.AngleAxis(90, Vector3.forward);
                    break;
                case OrientationListener.Orientation.LANDSCAPE_LEFT:
                    child.rotation = Quaternion.AngleAxis(270, Vector3.forward);
                    break;
                case OrientationListener.Orientation.INVERTED_PORTRAIT:
                    child.rotation = Quaternion.AngleAxis(180, Vector3.forward);
                    break;
            }
        }

    }


    //Listeners for player
    void OnEnable()
    {
        WorldGravity.GravityChanged += updateCanvasObjectOrientation;
    }

    void OnDisable()
    {
        WorldGravity.GravityChanged -= updateCanvasObjectOrientation;
    }

}
