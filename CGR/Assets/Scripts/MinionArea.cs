using UnityEngine;
using System.Collections;

public class MinionArea : MonoBehaviour {
    public bool IsActive;

    private Animator anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (anim.GetBool("On") != IsActive)
            anim.SetBool("On", IsActive);
	}

    //For Switches
    void plateDepressed()
    {
        IsActive = true;
        anim.SetBool("On", true);
    }

    void plateReleased()
    {
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.name == "Player" && anim.GetBool("On"))
        {
            anim.SetBool("inArea", true);
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.name == "Player" && anim.GetBool("On"))
            collider.gameObject.GetComponent<Player>().inMinionArea = true;
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        anim.SetBool("inArea", false);
        if (collider.name == "Player")
            collider.gameObject.GetComponent<Player>().inMinionArea = false;
    }
}
