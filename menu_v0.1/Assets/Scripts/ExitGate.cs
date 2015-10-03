using UnityEngine;
using System.Collections;

public class ExitGate : MonoBehaviour {

    GameObject victory;
    GameObject controller;
	// Use this for initialization
	void Start () {
        controller = GameObject.Find("ControlCanvas");
        victory = GameObject.Find("VictoryCanvas");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Player")
        {
            Time.timeScale = 0;
            victory.GetComponent<Canvas>().enabled = true;
            controller.GetComponent<Canvas>().enabled = false;
        }
    }
}
