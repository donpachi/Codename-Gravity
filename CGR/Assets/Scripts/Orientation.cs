using UnityEngine;
using System.Collections;

public class Orientation : MonoBehaviour {

    Animator anim;
    bool autoUpdate = false;
    public bool syncWithPlayer { get; set; }
    Animator playerAnim;

	void Start () {
        anim = GetComponent<Animator>();
        playerAnim = GameObject.Find("Player").GetComponent<Animator>();
        syncWithPlayer = true;
    }

    void Update()
    {
        if (syncWithPlayer)
            anim.SetInteger("Orientation", playerAnim.GetInteger("Orientation"));
    }

    //updates sprite to correct orientation
    public void updateOrientation(OrientationListener.Orientation orientation, float timer)
    {
        if (!autoUpdate)
            return;
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

    public void AutoUpdate(bool On)
    {
        autoUpdate = On; 
    }

    /// <summary>
    /// Called by animator. Resets flag when rotation is done
    /// </summary>
    void finishedRotation()
    {
        
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
