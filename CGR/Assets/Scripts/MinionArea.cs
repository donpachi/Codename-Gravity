using UnityEngine;
using System.Collections;

public class MinionArea : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
	}

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.name == "Player")
            collider.gameObject.GetComponent<Player>().inMinionArea = true;
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.name == "Player")
            collider.gameObject.GetComponent<Player>().inMinionArea = false;
    }
}
