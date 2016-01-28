using UnityEngine;
using System.Collections;

public class Minion : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject[] pushableObjects = GameObject.FindGameObjectsWithTag("Pushable");
        foreach (GameObject pushableObject in pushableObjects)
        {
            if (pushableObject.GetComponent<Collider2D>())
            {
                foreach (Collider2D collider in pushableObject.GetComponents<Collider2D>())
                    Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collider);
            }
        }       
	}
	
	// Update is called once per frame
	void Update () {
        
	}
}
