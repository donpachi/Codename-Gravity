using UnityEngine;
using System.Collections;

public class MinionArea : MonoBehaviour {

    private Animator anim;

	// Use this for initialization
	void Start () {
        anim = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        anim.SetBool("inArea", true);
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.name == "Player")
            collider.gameObject.GetComponent<Player>().inMinionArea = true;
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        anim.SetBool("inArea", false);
        if (collider.name == "Player")
            collider.gameObject.GetComponent<Player>().inMinionArea = false;
    }
}
