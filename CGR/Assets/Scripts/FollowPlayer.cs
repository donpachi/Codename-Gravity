using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {
    public GameObject player;
    Vector3 position;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
        position = new Vector3(player.transform.position.x, player.transform.position.y, -9);
        transform.position = position;
    }
	
	// Update is called once per frame
	void Update () {
        Vector2 newPostion = Vector2.Lerp(transform.position, player.transform.position, 0.5f);

        position.x = newPostion.x;
        position.y = newPostion.y;

        transform.position = position;
	}
    
    /// <summary>
    /// Sets the focal point of the camera
    /// </summary>
    /// <param name="focusPoint"></param>
    public void setFollowObject(GameObject focusPoint)
    {
        player = focusPoint;
    }
}
