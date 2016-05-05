using UnityEngine;
using System.Collections;

public class Orientation : MonoBehaviour {

    Animator anim;
    bool InRotation = false;

	void Start () {
        anim = GetComponent<Animator>();
	}

    //updates sprite to correct orientation
    public void updateOrientation(OrientationListener.Orientation orientation, float timer)
    {
        switch (orientation)
        {
            case OrientationListener.Orientation.PORTRAIT:
                anim.SetInteger("Orientation", 0);
                break;
            case OrientationListener.Orientation.LANDSCAPE_RIGHT:
                anim.SetInteger("Orientation", 3);
                break;
            case OrientationListener.Orientation.LANDSCAPE_LEFT:
                anim.SetInteger("Orientation", 1);
                break;
            case OrientationListener.Orientation.INVERTED_PORTRAIT:
                anim.SetInteger("Orientation", 2);
                break;
        }
    }

    /// <summary>
    /// Called by animator. Resets flag when rotation is done
    /// </summary>
    void finishedRotation()
    {
        InRotation = false;
    }

    //Listeners for player
    void OnEnable()
    {
        WorldGravity.GravityChanged += updateOrientation;
    }

    void OnDisable()
    {
        WorldGravity.GravityChanged -= updateOrientation;
    }
}
