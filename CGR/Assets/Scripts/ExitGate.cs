using UnityEngine;
using System.Collections;

public class ExitGate : MonoBehaviour {

    GameObject menuCanvas;
	// Use this for initialization
	void Start () {
        menuCanvas = GameObject.Find("MenuCanvas");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D collisionInfo)
    {
        if (collisionInfo.gameObject.name == "Player")
            menuCanvas.GetComponent<MenuCanvas>().VictoryScreen();
    }
}
