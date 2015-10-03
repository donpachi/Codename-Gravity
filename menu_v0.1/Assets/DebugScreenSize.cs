using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DebugScreenSize : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.GetComponent<Text>().text = "Height: " + Screen.height + " Width: " + Screen.width
                                                    + "\n ";
	}
}
