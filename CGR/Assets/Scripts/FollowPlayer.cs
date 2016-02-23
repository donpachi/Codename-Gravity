using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {
    public GameObject player;
    Vector3 position;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
        position = new Vector3(0, 0, -9);
	}
	
	// Update is called once per frame
	void Update () {
        position.x = player.transform.position.x;
        position.y = player.transform.position.y;

        gameObject.transform.position = position;
	}
}
