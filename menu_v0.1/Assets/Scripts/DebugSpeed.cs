using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DebugSpeed : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GameObject.Find("/Canvas/SpeedText").GetComponent<Text>().text = "Velocity X: " + GameObject.Find("Player").GetComponent<Rigidbody>().velocity.x;
	}
}
