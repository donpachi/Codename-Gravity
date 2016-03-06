using UnityEngine;
using System.Collections;

/// <summary>
/// Basic Camera script.
/// Camera will lerp toward spcified game object
/// </summary>
public class FollowPlayer : MonoBehaviour {
    public GameObject player;
    public float CameraSpeed = 0.5f;
    public Vector2 
        Smoothing,
        Margin;

    Vector3 position;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
        position = new Vector3(player.transform.position.x, player.transform.position.y, -9);
        transform.position = position;
    }
	
	// Update is called once per frame
	void Update () {
        Vector2 newPostion = Vector2.Lerp(transform.position, player.transform.position, CameraSpeed);

        var x = transform.position.x;
        var y = transform.position.y;

        if (Mathf.Abs(x - player.transform.position.x) > Margin.x)
            x = Mathf.Lerp(x, player.transform.position.x, Smoothing.x * Time.deltaTime);

        if (Mathf.Abs(x - player.transform.position.y) > Margin.y)
            y = Mathf.Lerp(y, player.transform.position.y, Smoothing.y * Time.deltaTime);


        position.x = newPostion.x;
        position.y = newPostion.y;

        transform.position = position;
        //transform.position = new Vector3(x, y, transform.position.z);
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
