using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {
    GameObject player;
    Vector3 position;

	// Use this for initialization
	void Start () {
        position = new Vector3(0, 0, -9);
	}
	
	// Update is called once per frame
	void Update () {
        position.x = Player.Instance.transform.position.x;
        position.y = Player.Instance.transform.position.y;

        gameObject.transform.position = position;
	}
}
