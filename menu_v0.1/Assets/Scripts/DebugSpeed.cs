using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DebugSpeed : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        GameObject.Find("SpeedText").GetComponent<Text>().text = "X: " + GameObject.Find("Player").GetComponent<Rigidbody2D>().velocity.x + "Y: " + GameObject.Find("Player").GetComponent<Rigidbody2D>().velocity.y;
	}
}
